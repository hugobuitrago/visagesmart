<template>
  <MainLayout>
    <section class="space-y-4">
      <article class="rounded-xl border border-black/5 bg-white/80 p-5 shadow-sm backdrop-blur dark:border-white/10 dark:bg-[#212121]/90">
        <div class="flex flex-col gap-4 lg:flex-row lg:items-center lg:justify-between">
          <div>
            <p class="text-sm font-semibold text-zinc-900 dark:text-white">Profissionais</p>
            <p class="mt-1 text-sm text-zinc-500 dark:text-zinc-400">
              Dados principais na lateral e serviços vinculados em uma área interna do profissional.
            </p>
          </div>

          <div class="flex flex-col gap-3 sm:flex-row">
            <input
              v-model="search"
              type="search"
              placeholder="Buscar por nome, telefone, e-mail, CPF ou RG"
              class="min-w-0 rounded-lg border border-black/10 bg-white px-3 py-2 text-sm text-zinc-800 outline-none transition focus:border-[color:var(--color-brand)] dark:border-white/10 dark:bg-[#1b1b1b] dark:text-white"
              @keydown.enter="applySearch"
            />
            <button
              class="rounded-lg border border-black/10 px-4 py-2 text-sm font-medium text-zinc-700 transition hover:border-[color:var(--color-brand)] hover:text-[color:var(--color-brand)] dark:border-white/10 dark:text-zinc-200"
              @click="applySearch"
            >
              Buscar
            </button>
            <button
              class="rounded-lg bg-[color:var(--color-brand)] px-4 py-2 text-sm font-medium text-white transition hover:opacity-90"
              @click="startCreate"
            >
              Novo profissional
            </button>
          </div>
        </div>

        <div class="mt-5 overflow-hidden rounded-xl border border-black/5 dark:border-white/10">
          <table class="min-w-full divide-y divide-black/5 text-sm dark:divide-white/10">
            <thead class="bg-zinc-50/90 dark:bg-white/5">
              <tr class="text-left text-zinc-500 dark:text-zinc-300">
                <th class="px-4 py-3 font-medium">Nome</th>
                <th class="px-4 py-3 font-medium">Contato</th>
                <th class="px-4 py-3 font-medium">Documentos</th>
                <th class="px-4 py-3 font-medium">Agenda</th>
                <th class="px-4 py-3 font-medium text-right">Ações</th>
              </tr>
            </thead>
            <tbody class="divide-y divide-black/5 bg-white/80 dark:divide-white/10 dark:bg-transparent">
              <tr v-if="isLoading">
                <td colspan="5" class="px-4 py-8 text-center text-zinc-500 dark:text-zinc-400">Carregando profissionais...</td>
              </tr>
              <tr v-else-if="errorMessage">
                <td colspan="5" class="px-4 py-8 text-center text-rose-600 dark:text-rose-300">{{ errorMessage }}</td>
              </tr>
              <tr v-else-if="professionals.length === 0">
                <td colspan="5" class="px-4 py-8 text-center text-zinc-500 dark:text-zinc-400">Nenhum profissional encontrado.</td>
              </tr>
              <tr v-for="professional in professionals" :key="professional.id" class="align-top">
                <td class="px-4 py-4">
                  <p class="font-semibold text-zinc-900 dark:text-white">{{ professional.name }}</p>
                  <p class="mt-1 text-xs text-zinc-500 dark:text-zinc-400">{{ professional.servicesCount }} serviço(s) vinculado(s)</p>
                </td>
                <td class="px-4 py-4 text-zinc-600 dark:text-zinc-300">
                  <p>{{ professional.phone }}</p>
                  <p class="mt-1 text-xs">{{ professional.email }}</p>
                </td>
                <td class="px-4 py-4 text-zinc-600 dark:text-zinc-300">
                  <p>CPF: {{ professional.cpf }}</p>
                  <p class="mt-1 text-xs">RG: {{ professional.rg }}</p>
                </td>
                <td class="px-4 py-4 text-zinc-600 dark:text-zinc-300">
                  <div class="space-y-1">
                    <p v-for="line in summarizeAvailability(professional.availability)" :key="line" class="text-xs">{{ line }}</p>
                  </div>
                </td>
                <td class="px-4 py-4">
                  <div class="flex justify-end gap-2">
                    <button
                      class="rounded-lg border border-black/10 px-3 py-2 text-xs font-medium text-zinc-700 transition hover:border-[color:var(--color-brand)] hover:text-[color:var(--color-brand)] dark:border-white/10 dark:text-zinc-200"
                      @click="openProfessional(professional.id)"
                    >
                      Abrir
                    </button>
                    <button
                      class="rounded-lg border border-black/10 px-3 py-2 text-xs font-medium text-zinc-700 transition hover:border-[color:var(--color-brand)] hover:text-[color:var(--color-brand)] dark:border-white/10 dark:text-zinc-200"
                      @click="startEdit(professional.id)"
                    >
                      Editar
                    </button>
                    <button
                      class="rounded-lg border border-rose-200 px-3 py-2 text-xs font-medium text-rose-600 transition hover:bg-rose-50 dark:border-rose-400/30 dark:text-rose-300 dark:hover:bg-rose-500/10"
                      @click="removeProfessional(professional)"
                    >
                      Excluir
                    </button>
                  </div>
                </td>
              </tr>
            </tbody>
          </table>
        </div>

        <div class="mt-4 flex flex-col gap-3 text-sm text-zinc-600 dark:text-zinc-300 sm:flex-row sm:items-center sm:justify-between">
          <p>Página {{ pagination.page }} de {{ pagination.totalPages }} · {{ pagination.totalItems }} profissionais</p>
          <div class="flex items-center gap-2">
            <button class="rounded-lg border border-black/10 px-3 py-2 transition disabled:cursor-not-allowed disabled:opacity-50 dark:border-white/10" :disabled="pagination.page <= 1 || isLoading" @click="changePage(pagination.page - 1)">Anterior</button>
            <button class="rounded-lg border border-black/10 px-3 py-2 transition disabled:cursor-not-allowed disabled:opacity-50 dark:border-white/10" :disabled="pagination.page >= pagination.totalPages || isLoading" @click="changePage(pagination.page + 1)">Próxima</button>
          </div>
        </div>
      </article>

    </section>

    <div v-if="isFormPanelOpen" class="fixed inset-0 z-40 bg-black/35 backdrop-blur-[1px]" @click="closeFormPanel" />

    <aside :class="['fixed right-0 top-0 z-50 flex h-full w-full max-w-2xl flex-col border-l border-black/10 bg-[color:var(--color-surface)] shadow-2xl transition-transform duration-300 dark:border-white/10 dark:bg-[#171717]', isFormPanelOpen ? 'translate-x-0' : 'translate-x-full']">
      <div class="flex items-start justify-between gap-4 border-b border-black/5 px-5 py-5 dark:border-white/10">
        <div>
          <p class="text-sm font-semibold text-zinc-900 dark:text-white">{{ isEditing ? 'Editar profissional' : 'Novo profissional' }}</p>
          <p class="mt-1 text-sm text-zinc-500 dark:text-zinc-400">Dados principais do profissional.</p>
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
            <button type="submit" class="flex-1 rounded-lg bg-[color:var(--color-brand)] px-4 py-3 text-sm font-medium text-white transition hover:opacity-90 disabled:cursor-not-allowed disabled:opacity-60" :disabled="isSaving">{{ isSaving ? 'Salvando...' : isEditing ? 'Salvar alterações' : 'Cadastrar profissional' }}</button>
          </div>
        </form>
      </div>
    </aside>
  </MainLayout>
</template>

<script setup lang="ts">
import { onMounted, reactive, ref } from 'vue'
import { useRouter } from 'vue-router'
import MainLayout from '@/layouts/MainLayout.vue'

type AvailabilityPeriod = { startTime: string; endTime: string }
type AvailabilityDay = { dayOfWeek: number; periods: AvailabilityPeriod[] }
type ProfessionalListItem = { id: string; name: string; phone: string; email: string; cpf: string; rg: string; servicesCount: number; availability: AvailabilityDay[] }
type ProfessionalDetail = { id: string; name: string; phone: string; email: string; cpf: string; rg: string; services: { serviceId: string; serviceName?: string; durationInMinutes: number; price: number; commissionAmount: number }[]; availability: AvailabilityDay[] }
type PagedResponse<T> = { items: T[]; page: number; pageSize: number; totalItems: number; totalPages: number }
type EditableAvailabilityPeriod = AvailabilityPeriod & { localId: string }
type EditableAvailabilityDay = { localId: string; dayOfWeek: number; periods: EditableAvailabilityPeriod[] }

const apiBaseUrl = `${import.meta.env.VITE_API_BASE_URL ?? 'http://localhost:5222'}/api`
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

const professionals = ref<ProfessionalListItem[]>([])
const search = ref('')
const appliedSearch = ref('')
const isLoading = ref(false)
const isSaving = ref(false)
const isFormPanelOpen = ref(false)
const errorMessage = ref('')
const formError = ref('')
const editingId = ref<string | null>(null)
const pagination = reactive({ page: 1, pageSize: 10, totalItems: 0, totalPages: 1 })
const form = reactive({ name: '', phone: '', email: '', cpf: '', rg: '', availability: [] as EditableAvailabilityDay[] })
const isEditing = ref(false)

function createPeriod(startTime = '08:00', endTime = '12:00'): EditableAvailabilityPeriod {
  return { localId: crypto.randomUUID(), startTime, endTime }
}
function createDay(dayOfWeek = 1): EditableAvailabilityDay {
  return { localId: crypto.randomUUID(), dayOfWeek, periods: [createPeriod()] }
}
function resetForm() {
  editingId.value = null
  isEditing.value = false
  formError.value = ''
  form.name = ''
  form.phone = ''
  form.email = ''
  form.cpf = ''
  form.rg = ''
  form.availability = [createDay()]
}
function closeFormPanel() {
  isFormPanelOpen.value = false
  resetForm()
}
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

async function fetchProfessionals(page = 1) {
  isLoading.value = true
  errorMessage.value = ''
  try {
    const params = new URLSearchParams({ page: page.toString(), pageSize: pagination.pageSize.toString() })
    if (appliedSearch.value.trim()) params.set('search', appliedSearch.value.trim())
    const response = await fetch(`${apiBaseUrl}/professionals?${params}`)
    if (!response.ok) throw new Error('Não foi possível carregar os profissionais.')
    const data: PagedResponse<ProfessionalListItem> = await response.json()
    professionals.value = data.items
    pagination.page = data.page
    pagination.pageSize = data.pageSize
    pagination.totalItems = data.totalItems
    pagination.totalPages = data.totalPages
  } catch (error) {
    errorMessage.value = error instanceof Error ? error.message : 'Erro inesperado ao carregar profissionais.'
  } finally { isLoading.value = false }
}
function applySearch() { appliedSearch.value = search.value; fetchProfessionals(1) }
function changePage(page: number) { fetchProfessionals(page) }

async function startEdit(id: string) {
  formError.value = ''
  try {
    const response = await fetch(`${apiBaseUrl}/professionals/${id}`)
    if (!response.ok) throw new Error('Não foi possível carregar o profissional para edição.')
    const professional: ProfessionalDetail = await response.json()
    editingId.value = professional.id
    isEditing.value = true
    isFormPanelOpen.value = true
    form.name = professional.name
    form.phone = professional.phone
    form.email = professional.email
    form.cpf = professional.cpf
    form.rg = professional.rg
    form.availability = professional.availability.map((day: AvailabilityDay) => ({
      localId: crypto.randomUUID(),
      dayOfWeek: day.dayOfWeek,
      periods: day.periods.map((period: AvailabilityPeriod) => ({ localId: crypto.randomUUID(), startTime: period.startTime, endTime: period.endTime })),
    }))
  } catch (error) {
    formError.value = error instanceof Error ? error.message : 'Erro inesperado ao carregar profissional.'
  }
}
function startCreate() { resetForm(); isFormPanelOpen.value = true }

async function saveProfessional() {
  formError.value = validateForm()
  if (formError.value) return
  isSaving.value = true
  const wasEditing = isEditing.value
  const currentPage = pagination.page
  try {
    const response = await fetch(isEditing.value && editingId.value ? `${apiBaseUrl}/professionals/${editingId.value}` : `${apiBaseUrl}/professionals`, {
      method: isEditing.value ? 'PUT' : 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(toPayload()),
    })
    if (!response.ok) {
      const problem = await response.json().catch(() => null)
      const detail = problem?.errors?.professional?.[0] ?? 'Não foi possível salvar o profissional.'
      throw new Error(detail)
    }
    closeFormPanel()
    await fetchProfessionals(wasEditing ? currentPage : 1)
  } catch (error) {
    formError.value = error instanceof Error ? error.message : 'Erro inesperado ao salvar profissional.'
  } finally { isSaving.value = false }
}

async function removeProfessional(professional: ProfessionalListItem) {
  const confirmed = window.confirm(`Excluir o profissional ${professional.name}?`)
  if (!confirmed) return
  try {
    const response = await fetch(`${apiBaseUrl}/professionals/${professional.id}`, { method: 'DELETE' })
    if (!response.ok) throw new Error('Não foi possível excluir o profissional.')
    const targetPage = professionals.value.length === 1 && pagination.page > 1 ? pagination.page - 1 : pagination.page
    await fetchProfessionals(targetPage)
    if (editingId.value === professional.id) closeFormPanel()
  } catch (error) {
    formError.value = error instanceof Error ? error.message : 'Erro inesperado ao excluir profissional.'
  }
}
function openProfessional(id: string) { router.push(`/professionals/${id}`) }

onMounted(async () => {
  resetForm()
  await fetchProfessionals()
})
</script>
