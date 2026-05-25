<template>
  <MainLayout>
    <section class="space-y-4">
      <article class="rounded-xl border border-black/5 bg-white/80 p-5 shadow-sm backdrop-blur dark:border-white/10 dark:bg-[#212121]/90">
        <div class="flex flex-col gap-4 lg:flex-row lg:items-start lg:justify-between">
          <div>
            <button class="text-sm text-zinc-500 transition hover:text-[color:var(--color-brand)] dark:text-zinc-400" @click="router.push('/professionals')">
              Voltar para profissionais
            </button>
            <p class="mt-3 text-xs font-semibold uppercase tracking-[0.28em] text-zinc-500 dark:text-zinc-400">Perfil do profissional</p>
            <h1 class="mt-2 text-3xl font-semibold text-zinc-900 dark:text-white">{{ professional?.name ?? 'Carregando...' }}</h1>
            <p class="mt-2 text-sm text-zinc-500 dark:text-zinc-400">
              Visualize dados cadastrais, serviços vinculados, agenda e indicadores.
            </p>
          </div>

          <div class="flex gap-2">
            <button class="rounded-lg border border-black/10 px-4 py-2 text-sm font-medium text-zinc-700 transition hover:border-[color:var(--color-brand)] hover:text-[color:var(--color-brand)] dark:border-white/10 dark:text-zinc-200" @click="activeTab = 'dados'">
              Dados
            </button>
            <button class="rounded-lg bg-[color:var(--color-brand)] px-4 py-2 text-sm font-medium text-white transition hover:opacity-90" @click="openEditPanel">
              Editar profissional
            </button>
          </div>
        </div>

        <div class="mt-5 flex flex-wrap gap-2">
          <button
            v-for="tab in tabs"
            :key="tab.id"
            class="rounded-lg px-4 py-2 text-sm font-medium transition"
            :class="activeTab === tab.id ? 'bg-[color:var(--color-brand)] text-white' : 'border border-black/10 text-zinc-700 hover:border-[color:var(--color-brand)] hover:text-[color:var(--color-brand)] dark:border-white/10 dark:text-zinc-200'"
            @click="activeTab = tab.id"
          >
            {{ tab.label }}
          </button>
        </div>
      </article>

      <article v-if="isLoading" class="rounded-xl border border-black/5 bg-white/80 p-8 text-center text-zinc-500 shadow-sm backdrop-blur dark:border-white/10 dark:bg-[#212121]/90 dark:text-zinc-400">
        Carregando profissional...
      </article>

      <article v-else-if="errorMessage" class="rounded-xl border border-rose-200 bg-rose-50 p-6 text-rose-700 shadow-sm dark:border-rose-400/30 dark:bg-rose-500/10 dark:text-rose-200">
        {{ errorMessage }}
      </article>

      <template v-else-if="professional">
        <article v-if="activeTab === 'dados'" class="rounded-xl border border-black/5 bg-white/80 p-5 shadow-sm backdrop-blur dark:border-white/10 dark:bg-[#212121]/90">
          <div class="grid gap-4 md:grid-cols-2 xl:grid-cols-3">
            <div class="rounded-lg border border-black/5 p-4 dark:border-white/10">
              <p class="text-xs font-semibold uppercase tracking-[0.2em] text-zinc-500 dark:text-zinc-400">Telefone</p>
              <p class="mt-2 text-sm font-medium text-zinc-900 dark:text-white">{{ professional.phone }}</p>
            </div>
            <div class="rounded-lg border border-black/5 p-4 dark:border-white/10">
              <p class="text-xs font-semibold uppercase tracking-[0.2em] text-zinc-500 dark:text-zinc-400">E-mail</p>
              <p class="mt-2 text-sm font-medium text-zinc-900 dark:text-white">{{ professional.email }}</p>
            </div>
            <div class="rounded-lg border border-black/5 p-4 dark:border-white/10">
              <p class="text-xs font-semibold uppercase tracking-[0.2em] text-zinc-500 dark:text-zinc-400">CPF</p>
              <p class="mt-2 text-sm font-medium text-zinc-900 dark:text-white">{{ professional.cpf }}</p>
            </div>
            <div class="rounded-lg border border-black/5 p-4 dark:border-white/10">
              <p class="text-xs font-semibold uppercase tracking-[0.2em] text-zinc-500 dark:text-zinc-400">RG</p>
              <p class="mt-2 text-sm font-medium text-zinc-900 dark:text-white">{{ professional.rg }}</p>
            </div>
            <div class="rounded-lg border border-black/5 p-4 dark:border-white/10">
              <p class="text-xs font-semibold uppercase tracking-[0.2em] text-zinc-500 dark:text-zinc-400">Serviços vinculados</p>
              <p class="mt-2 text-sm font-medium text-zinc-900 dark:text-white">{{ professional.services.length }}</p>
            </div>
          </div>
        </article>

        <article v-else-if="activeTab === 'servicos'" class="rounded-xl border border-black/5 bg-white/80 p-5 shadow-sm backdrop-blur dark:border-white/10 dark:bg-[#212121]/90">
          <div class="flex flex-col gap-3 md:flex-row md:items-center md:justify-between">
            <div>
              <p class="text-sm font-semibold text-zinc-900 dark:text-white">Serviços do profissional</p>
              <p class="mt-1 text-sm text-zinc-500 dark:text-zinc-400">Selecione um serviço pré-cadastrado, informe tempo, valor e comissão.</p>
            </div>
            <button class="rounded-lg border border-black/10 px-3 py-2 text-xs font-medium text-zinc-700 transition hover:border-[color:var(--color-brand)] hover:text-[color:var(--color-brand)] dark:border-white/10 dark:text-zinc-200" @click="addProfessionalService">
              Adicionar serviço
            </button>
          </div>

          <div class="mt-4 space-y-3">
            <div v-if="selectedProfessionalServices.length === 0" class="rounded-lg border border-dashed border-black/10 p-4 text-sm text-zinc-500 dark:border-white/10 dark:text-zinc-400">
              Nenhum serviço vinculado ainda.
            </div>

            <div v-for="(item, index) in selectedProfessionalServices" :key="item.localId" class="grid gap-3 rounded-lg border border-black/5 p-4 dark:border-white/10 md:grid-cols-[1.5fr_0.8fr_0.8fr_0.8fr_auto]">
              <label class="space-y-2">
                <span class="text-xs font-medium uppercase tracking-[0.2em] text-zinc-500 dark:text-zinc-400">Serviço</span>
                <select v-model="item.serviceId" class="w-full rounded-lg border border-black/10 bg-white px-3 py-2 text-sm text-zinc-800 outline-none transition focus:border-[color:var(--color-brand)] dark:border-white/10 dark:bg-[#1b1b1b] dark:text-white">
                  <option value="">Selecione</option>
                  <option v-for="service in availableServices" :key="service.id" :value="service.id">{{ service.name }}</option>
                </select>
              </label>
              <label class="space-y-2">
                <span class="text-xs font-medium uppercase tracking-[0.2em] text-zinc-500 dark:text-zinc-400">Tempo</span>
                <input v-model.number="item.durationInMinutes" type="number" min="1" class="w-full rounded-lg border border-black/10 bg-white px-3 py-2 text-sm text-zinc-800 outline-none transition focus:border-[color:var(--color-brand)] dark:border-white/10 dark:bg-[#1b1b1b] dark:text-white" />
              </label>
              <label class="space-y-2">
                <span class="text-xs font-medium uppercase tracking-[0.2em] text-zinc-500 dark:text-zinc-400">Valor</span>
                <input v-model.number="item.price" type="number" min="0" step="0.01" class="w-full rounded-lg border border-black/10 bg-white px-3 py-2 text-sm text-zinc-800 outline-none transition focus:border-[color:var(--color-brand)] dark:border-white/10 dark:bg-[#1b1b1b] dark:text-white" />
              </label>
              <label class="space-y-2">
                <span class="text-xs font-medium uppercase tracking-[0.2em] text-zinc-500 dark:text-zinc-400">Comissão</span>
                <input v-model.number="item.commissionAmount" type="number" min="0" step="0.01" class="w-full rounded-lg border border-black/10 bg-white px-3 py-2 text-sm text-zinc-800 outline-none transition focus:border-[color:var(--color-brand)] dark:border-white/10 dark:bg-[#1b1b1b] dark:text-white" />
              </label>
              <button type="button" class="mt-auto rounded-lg border border-rose-200 px-3 py-2 text-xs font-medium text-rose-600 transition hover:bg-rose-50 dark:border-rose-400/30 dark:text-rose-300 dark:hover:bg-rose-500/10" @click="removeProfessionalService(index)">Remover</button>
            </div>
          </div>

          <div v-if="servicesFormError" class="mt-4 rounded-lg border border-rose-200 bg-rose-50 px-3 py-3 text-sm text-rose-700 dark:border-rose-400/30 dark:bg-rose-500/10 dark:text-rose-200">{{ servicesFormError }}</div>

          <div class="mt-4 flex justify-end">
            <button class="rounded-lg bg-[color:var(--color-brand)] px-4 py-3 text-sm font-medium text-white transition hover:opacity-90 disabled:cursor-not-allowed disabled:opacity-60" :disabled="isSavingServices" @click="saveProfessionalServices">
              {{ isSavingServices ? 'Salvando...' : 'Salvar serviços do profissional' }}
            </button>
          </div>
        </article>

        <article v-else-if="activeTab === 'agenda'" class="rounded-xl border border-black/5 bg-white/80 p-5 shadow-sm backdrop-blur dark:border-white/10 dark:bg-[#212121]/90">
          <p class="text-sm font-semibold text-zinc-900 dark:text-white">Agenda semanal</p>
          <div class="mt-4 grid gap-3 lg:grid-cols-2">
            <div v-for="line in summarizeAvailability(professional.availability)" :key="line" class="rounded-lg border border-black/5 p-4 text-sm text-zinc-700 dark:border-white/10 dark:text-zinc-200">
              {{ line }}
            </div>
          </div>
        </article>

        <article v-else class="rounded-xl border border-black/5 bg-white/80 p-5 shadow-sm backdrop-blur dark:border-white/10 dark:bg-[#212121]/90">
          <p class="text-sm font-semibold text-zinc-900 dark:text-white">Demonstrativo</p>
          <div class="mt-4 grid gap-4 md:grid-cols-3">
            <div class="rounded-lg border border-black/5 p-4 dark:border-white/10">
              <p class="text-xs font-semibold uppercase tracking-[0.2em] text-zinc-500 dark:text-zinc-400">Serviços vinculados</p>
              <p class="mt-2 text-2xl font-semibold text-zinc-900 dark:text-white">{{ professional.services.length }}</p>
            </div>
            <div class="rounded-lg border border-black/5 p-4 dark:border-white/10">
              <p class="text-xs font-semibold uppercase tracking-[0.2em] text-zinc-500 dark:text-zinc-400">Tempo médio</p>
              <p class="mt-2 text-2xl font-semibold text-zinc-900 dark:text-white">{{ averageDuration }} min</p>
            </div>
            <div class="rounded-lg border border-black/5 p-4 dark:border-white/10">
              <p class="text-xs font-semibold uppercase tracking-[0.2em] text-zinc-500 dark:text-zinc-400">Comissão potencial</p>
              <p class="mt-2 text-2xl font-semibold text-zinc-900 dark:text-white">{{ commissionTotal }}</p>
            </div>
          </div>
        </article>
      </template>
    </section>

    <div v-if="isFormPanelOpen" class="fixed inset-0 z-40 bg-black/35 backdrop-blur-[1px]" @click="closeFormPanel" />

    <aside :class="['fixed right-0 top-0 z-50 flex h-full w-full max-w-2xl flex-col border-l border-black/10 bg-[color:var(--color-surface)] shadow-2xl transition-transform duration-300 dark:border-white/10 dark:bg-[#171717]', isFormPanelOpen ? 'translate-x-0' : 'translate-x-full']">
      <div class="flex items-start justify-between gap-4 border-b border-black/5 px-5 py-5 dark:border-white/10">
        <div>
          <p class="text-sm font-semibold text-zinc-900 dark:text-white">Editar profissional</p>
          <p class="mt-1 text-sm text-zinc-500 dark:text-zinc-400">Atualize os dados principais e a agenda.</p>
        </div>
        <button class="rounded-lg border border-black/10 px-3 py-2 text-xs font-medium text-zinc-700 transition hover:border-[color:var(--color-brand)] hover:text-[color:var(--color-brand)] dark:border-white/10 dark:text-zinc-200" @click="closeFormPanel">Fechar</button>
      </div>

      <div class="flex-1 overflow-y-auto px-5 py-5">
        <form class="space-y-5" @submit.prevent="saveProfessional">
          <div class="grid gap-4 md:grid-cols-2">
            <label class="space-y-2">
              <span class="text-sm font-medium text-zinc-700 dark:text-zinc-200">Nome</span>
              <input v-model="form.name" type="text" class="w-full rounded-lg border border-black/10 bg-white px-3 py-2 text-sm text-zinc-800 outline-none transition focus:border-[color:var(--color-brand)] dark:border-white/10 dark:bg-[#1b1b1b] dark:text-white" />
            </label>
            <label class="space-y-2">
              <span class="text-sm font-medium text-zinc-700 dark:text-zinc-200">Telefone</span>
              <input v-model="form.phone" type="text" class="w-full rounded-lg border border-black/10 bg-white px-3 py-2 text-sm text-zinc-800 outline-none transition focus:border-[color:var(--color-brand)] dark:border-white/10 dark:bg-[#1b1b1b] dark:text-white" />
            </label>
          </div>

          <div class="grid gap-4 md:grid-cols-2">
            <label class="space-y-2">
              <span class="text-sm font-medium text-zinc-700 dark:text-zinc-200">CPF</span>
              <input v-model="form.cpf" type="text" class="w-full rounded-lg border border-black/10 bg-white px-3 py-2 text-sm text-zinc-800 outline-none transition focus:border-[color:var(--color-brand)] dark:border-white/10 dark:bg-[#1b1b1b] dark:text-white" />
            </label>
            <label class="space-y-2">
              <span class="text-sm font-medium text-zinc-700 dark:text-zinc-200">RG</span>
              <input v-model="form.rg" type="text" class="w-full rounded-lg border border-black/10 bg-white px-3 py-2 text-sm text-zinc-800 outline-none transition focus:border-[color:var(--color-brand)] dark:border-white/10 dark:bg-[#1b1b1b] dark:text-white" />
            </label>
          </div>

          <label class="space-y-2">
            <span class="text-sm font-medium text-zinc-700 dark:text-zinc-200">E-mail</span>
            <input v-model="form.email" type="email" class="w-full rounded-lg border border-black/10 bg-white px-3 py-2 text-sm text-zinc-800 outline-none transition focus:border-[color:var(--color-brand)] dark:border-white/10 dark:bg-[#1b1b1b] dark:text-white" />
          </label>

          <div class="space-y-3">
            <div class="flex items-center justify-between gap-3">
              <div>
                <p class="text-sm font-medium text-zinc-700 dark:text-zinc-200">Dias e horários de atendimento</p>
                <p class="mt-1 text-xs text-zinc-500 dark:text-zinc-400">Concentre a agenda por dia da semana e adicione quantos períodos forem necessários.</p>
              </div>
              <button type="button" class="rounded-lg border border-black/10 px-3 py-2 text-xs font-medium text-zinc-700 transition hover:border-[color:var(--color-brand)] hover:text-[color:var(--color-brand)] dark:border-white/10 dark:text-zinc-200" @click="addDay">Adicionar dia</button>
            </div>

            <div v-for="(day, dayIndex) in form.availability" :key="day.localId" class="rounded-lg border border-black/5 p-4 dark:border-white/10">
              <div class="flex flex-col gap-3 md:flex-row md:items-center md:justify-between">
                <div class="flex flex-1 flex-col gap-3 md:flex-row md:items-center">
                  <label class="space-y-2 md:min-w-48">
                    <span class="text-xs font-medium uppercase tracking-[0.2em] text-zinc-500 dark:text-zinc-400">Dia da semana</span>
                    <select v-model.number="day.dayOfWeek" class="w-full rounded-lg border border-black/10 bg-white px-3 py-2 text-sm text-zinc-800 outline-none transition focus:border-[color:var(--color-brand)] dark:border-white/10 dark:bg-[#1b1b1b] dark:text-white">
                      <option v-for="option in weekDays" :key="option.value" :value="option.value">{{ option.label }}</option>
                    </select>
                  </label>
                  <p class="text-xs text-zinc-500 dark:text-zinc-400">{{ day.periods.length }} período(s) configurado(s)</p>
                </div>
                <button type="button" class="self-start rounded-lg border border-rose-200 px-3 py-2 text-xs font-medium text-rose-600 transition hover:bg-rose-50 dark:border-rose-400/30 dark:text-rose-300 dark:hover:bg-rose-500/10" @click="removeDay(dayIndex)">Remover dia</button>
              </div>

              <div class="mt-4 space-y-3">
                <div v-for="(period, periodIndex) in day.periods" :key="period.localId" class="grid gap-3 md:grid-cols-[1fr_1fr_auto]">
                  <label class="space-y-2">
                    <span class="text-xs font-medium uppercase tracking-[0.2em] text-zinc-500 dark:text-zinc-400">Início</span>
                    <input v-model="period.startTime" type="time" class="w-full rounded-lg border border-black/10 bg-white px-3 py-2 text-sm text-zinc-800 outline-none transition focus:border-[color:var(--color-brand)] dark:border-white/10 dark:bg-[#1b1b1b] dark:text-white" />
                  </label>
                  <label class="space-y-2">
                    <span class="text-xs font-medium uppercase tracking-[0.2em] text-zinc-500 dark:text-zinc-400">Fim</span>
                    <input v-model="period.endTime" type="time" class="w-full rounded-lg border border-black/10 bg-white px-3 py-2 text-sm text-zinc-800 outline-none transition focus:border-[color:var(--color-brand)] dark:border-white/10 dark:bg-[#1b1b1b] dark:text-white" />
                  </label>
                  <button type="button" class="mt-auto rounded-lg border border-black/10 px-3 py-2 text-xs font-medium text-zinc-700 transition hover:border-rose-300 hover:text-rose-600 dark:border-white/10 dark:text-zinc-200 dark:hover:border-rose-400/30 dark:hover:text-rose-300" @click="removePeriod(dayIndex, periodIndex)">Remover período</button>
                </div>
              </div>

              <button type="button" class="mt-4 rounded-lg border border-black/10 px-3 py-2 text-xs font-medium text-zinc-700 transition hover:border-[color:var(--color-brand)] hover:text-[color:var(--color-brand)] dark:border-white/10 dark:text-zinc-200" @click="addPeriod(dayIndex)">Adicionar período</button>
            </div>
          </div>

          <div v-if="formError" class="rounded-lg border border-rose-200 bg-rose-50 px-3 py-3 text-sm text-rose-700 dark:border-rose-400/30 dark:bg-rose-500/10 dark:text-rose-200">{{ formError }}</div>

          <div class="sticky bottom-0 flex gap-3 border-t border-black/5 bg-[color:var(--color-surface)] pt-4 dark:border-white/10 dark:bg-[#171717]">
            <button type="button" class="flex-1 rounded-lg border border-black/10 px-4 py-3 text-sm font-medium text-zinc-700 transition hover:border-[color:var(--color-brand)] hover:text-[color:var(--color-brand)] dark:border-white/10 dark:text-zinc-200" @click="closeFormPanel">Cancelar</button>
            <button type="submit" class="flex-1 rounded-lg bg-[color:var(--color-brand)] px-4 py-3 text-sm font-medium text-white transition hover:opacity-90 disabled:cursor-not-allowed disabled:opacity-60" :disabled="isSaving">{{ isSaving ? 'Salvando...' : 'Salvar alterações' }}</button>
          </div>
        </form>
      </div>
    </aside>
  </MainLayout>
</template>

<script setup lang="ts">
import { computed, onMounted, reactive, ref } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import MainLayout from '@/layouts/MainLayout.vue'

type AvailabilityPeriod = { startTime: string; endTime: string }
type AvailabilityDay = { dayOfWeek: number; periods: AvailabilityPeriod[] }
type ServiceOption = { id: string; name: string; category: string }
type ProfessionalService = { serviceId: string; serviceName?: string; durationInMinutes: number; price: number; commissionAmount: number }
type ProfessionalDetail = { id: string; name: string; phone: string; email: string; cpf: string; rg: string; services: ProfessionalService[]; availability: AvailabilityDay[] }
type EditableAvailabilityPeriod = AvailabilityPeriod & { localId: string }
type EditableAvailabilityDay = { localId: string; dayOfWeek: number; periods: EditableAvailabilityPeriod[] }
type EditableProfessionalService = { localId: string; serviceId: string; durationInMinutes: number; price: number; commissionAmount: number }

const apiBaseUrl = `${import.meta.env.VITE_API_BASE_URL ?? 'http://localhost:5222'}/api`
const route = useRoute()
const router = useRouter()
const weekDays = [
  { value: 1, label: 'Segunda-feira' },
  { value: 2, label: 'Terça-feira' },
  { value: 3, label: 'Quarta-feira' },
  { value: 4, label: 'Quinta-feira' },
  { value: 5, label: 'Sexta-feira' },
  { value: 6, label: 'Sábado' },
  { value: 0, label: 'Domingo' },
]
const tabs = [
  { id: 'dados', label: 'Dados' },
  { id: 'servicos', label: 'Serviços' },
  { id: 'agenda', label: 'Agenda' },
  { id: 'demonstrativo', label: 'Demonstrativo' },
] as const

const professional = ref<ProfessionalDetail | null>(null)
const availableServices = ref<ServiceOption[]>([])
const selectedProfessionalServices = ref<EditableProfessionalService[]>([])
const activeTab = ref<(typeof tabs)[number]['id']>('dados')
const isLoading = ref(false)
const isSaving = ref(false)
const isSavingServices = ref(false)
const isFormPanelOpen = ref(false)
const errorMessage = ref('')
const formError = ref('')
const servicesFormError = ref('')
const form = reactive({ name: '', phone: '', email: '', cpf: '', rg: '', availability: [] as EditableAvailabilityDay[] })

const averageDuration = computed(() => {
  if (!professional.value || professional.value.services.length === 0) return 0
  const total = professional.value.services.reduce((sum, item) => sum + item.durationInMinutes, 0)
  return Math.round(total / professional.value.services.length)
})
const commissionTotal = computed(() => {
  const total = professional.value?.services.reduce((sum, item) => sum + item.commissionAmount, 0) ?? 0
  return total.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })
})

function createPeriod(startTime = '08:00', endTime = '12:00'): EditableAvailabilityPeriod {
  return { localId: crypto.randomUUID(), startTime, endTime }
}
function createDay(dayOfWeek = 1): EditableAvailabilityDay {
  return { localId: crypto.randomUUID(), dayOfWeek, periods: [createPeriod()] }
}
function createProfessionalService(): EditableProfessionalService {
  return { localId: crypto.randomUUID(), serviceId: '', durationInMinutes: 30, price: 0, commissionAmount: 0 }
}
function resetEditForm() {
  formError.value = ''
  form.name = professional.value?.name ?? ''
  form.phone = professional.value?.phone ?? ''
  form.email = professional.value?.email ?? ''
  form.cpf = professional.value?.cpf ?? ''
  form.rg = professional.value?.rg ?? ''
  form.availability = (professional.value?.availability ?? []).map((day) => ({
    localId: crypto.randomUUID(),
    dayOfWeek: day.dayOfWeek,
    periods: day.periods.map((period) => ({ localId: crypto.randomUUID(), startTime: period.startTime, endTime: period.endTime })),
  }))
}
function openEditPanel() {
  resetEditForm()
  isFormPanelOpen.value = true
}
function closeFormPanel() { isFormPanelOpen.value = false; formError.value = '' }
function addDay() {
  const usedDays = new Set(form.availability.map((item) => item.dayOfWeek))
  const nextDay = weekDays.find((item) => !usedDays.has(item.value))?.value ?? 1
  form.availability.push(createDay(nextDay))
}
function removeDay(dayIndex: number) { form.availability.splice(dayIndex, 1) }
function addPeriod(dayIndex: number) { form.availability[dayIndex].periods.push(createPeriod('13:00', '18:00')) }
function removePeriod(dayIndex: number, periodIndex: number) {
  const day = form.availability[dayIndex]
  if (day.periods.length === 1) { day.periods[0] = createPeriod(); return }
  day.periods.splice(periodIndex, 1)
}
function summarizeAvailability(availability: AvailabilityDay[]) {
  return [...availability].sort((a, b) => a.dayOfWeek - b.dayOfWeek).map((day) => {
    const label = weekDays.find((item) => item.value === day.dayOfWeek)?.label ?? 'Dia'
    const periods = day.periods.map((period) => `${period.startTime} às ${period.endTime}`).join(' · ')
    return `${label}: ${periods}`
  })
}
function toPayload() {
  return {
    name: form.name,
    phone: form.phone,
    email: form.email,
    cpf: form.cpf,
    rg: form.rg,
    availability: form.availability.map((day) => ({
      dayOfWeek: day.dayOfWeek,
      periods: day.periods.map((period) => ({ startTime: period.startTime, endTime: period.endTime })),
    })),
  }
}
function validateForm() {
  if (!form.name.trim()) return 'Informe o nome do profissional.'
  if (!form.phone.trim()) return 'Informe o telefone.'
  if (!form.email.trim()) return 'Informe o e-mail.'
  if (!form.cpf.trim()) return 'Informe o CPF.'
  if (!form.rg.trim()) return 'Informe o RG.'
  if (form.availability.length === 0) return 'Adicione ao menos um dia de atendimento.'
  const usedDays = new Set<number>()
  for (const day of form.availability) {
    if (usedDays.has(day.dayOfWeek)) return 'Não repita o mesmo dia da semana mais de uma vez.'
    usedDays.add(day.dayOfWeek)
    if (day.periods.length === 0) return 'Cada dia precisa ter pelo menos um período.'
    for (const period of day.periods) {
      if (!period.startTime || !period.endTime) return 'Todos os períodos precisam ter início e fim.'
      if (period.startTime >= period.endTime) return 'A hora inicial precisa ser menor que a final.'
    }
  }
  return ''
}

async function fetchAvailableServices() {
  const response = await fetch(`${apiBaseUrl}/services`)
  if (response.ok) availableServices.value = await response.json()
}
async function fetchProfessional() {
  isLoading.value = true
  errorMessage.value = ''
  try {
    const response = await fetch(`${apiBaseUrl}/professionals/${route.params.id}`)
    if (!response.ok) throw new Error('Não foi possível carregar o profissional.')
    const loadedProfessional: ProfessionalDetail = await response.json()
    professional.value = loadedProfessional
    selectedProfessionalServices.value = (loadedProfessional.services ?? []).map((item) => ({
      localId: crypto.randomUUID(),
      serviceId: item.serviceId,
      durationInMinutes: item.durationInMinutes,
      price: item.price,
      commissionAmount: item.commissionAmount,
    }))
    resetEditForm()
  } catch (error) {
    errorMessage.value = error instanceof Error ? error.message : 'Erro inesperado ao carregar profissional.'
  } finally {
    isLoading.value = false
  }
}
function addProfessionalService() { selectedProfessionalServices.value.push(createProfessionalService()) }
function removeProfessionalService(index: number) { selectedProfessionalServices.value.splice(index, 1) }

async function saveProfessionalServices() {
  const currentProfessional = professional.value
  if (!currentProfessional) return
  servicesFormError.value = ''
  for (const item of selectedProfessionalServices.value) {
    if (!item.serviceId) { servicesFormError.value = 'Selecione o serviço em todas as linhas.'; return }
    if (item.durationInMinutes <= 0) { servicesFormError.value = 'O tempo do serviço deve ser maior que zero.'; return }
    if (item.price < 0 || item.commissionAmount < 0) { servicesFormError.value = 'Valores e comissão não podem ser negativos.'; return }
  }
  isSavingServices.value = true
  try {
    const response = await fetch(`${apiBaseUrl}/professionals/${currentProfessional.id}/services`, {
      method: 'PUT',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(selectedProfessionalServices.value.map((item) => ({
        serviceId: item.serviceId,
        durationInMinutes: item.durationInMinutes,
        price: item.price,
        commissionAmount: item.commissionAmount,
      }))),
    })
    if (!response.ok) {
      const problem = await response.json().catch(() => null)
      const detail = problem?.errors?.professional?.[0] ?? 'Não foi possível salvar os serviços do profissional.'
      throw new Error(detail)
    }
    await fetchProfessional()
  } catch (error) {
    servicesFormError.value = error instanceof Error ? error.message : 'Erro inesperado ao salvar os serviços do profissional.'
  } finally {
    isSavingServices.value = false
  }
}

async function saveProfessional() {
  const currentProfessional = professional.value
  if (!currentProfessional) return
  formError.value = validateForm()
  if (formError.value) return
  isSaving.value = true
  try {
    const response = await fetch(`${apiBaseUrl}/professionals/${currentProfessional.id}`, {
      method: 'PUT',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(toPayload()),
    })
    if (!response.ok) {
      const problem = await response.json().catch(() => null)
      const detail = problem?.errors?.professional?.[0] ?? 'Não foi possível salvar o profissional.'
      throw new Error(detail)
    }
    isFormPanelOpen.value = false
    await fetchProfessional()
  } catch (error) {
    formError.value = error instanceof Error ? error.message : 'Erro inesperado ao salvar profissional.'
  } finally {
    isSaving.value = false
  }
}

onMounted(async () => {
  await Promise.all([fetchAvailableServices(), fetchProfessional()])
})
</script>
