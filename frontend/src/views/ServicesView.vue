<template>
  <MainLayout>
    <section>
      <article class="rounded-xl border border-black/5 bg-white/80 p-5 shadow-sm backdrop-blur dark:border-white/10 dark:bg-[#212121]/90">
        <div class="flex flex-col gap-4 lg:flex-row lg:items-center lg:justify-between">
          <div>
            <p class="text-sm font-semibold text-zinc-900 dark:text-white">Serviços</p>
            <p class="mt-1 text-sm text-zinc-500 dark:text-zinc-400">
              Cadastre, edite e organize os serviços disponíveis no salão e na barbearia.
            </p>
          </div>

          <div class="flex flex-col gap-3 sm:flex-row">
            <input
              v-model="search"
              type="search"
              placeholder="Buscar por nome ou categoria"
              class="min-w-0 rounded-lg border border-black/10 bg-white px-3 py-2 text-sm text-zinc-800 outline-none transition focus:border-[color:var(--color-brand)] dark:border-white/10 dark:bg-[#1b1b1b] dark:text-white"
            />
            <button
              class="rounded-lg bg-[color:var(--color-brand)] px-4 py-2 text-sm font-medium text-white transition hover:opacity-90"
              @click="startCreate"
            >
              Novo serviço
            </button>
          </div>
        </div>

        <div class="mt-5 overflow-hidden rounded-xl border border-black/5 dark:border-white/10">
          <table class="min-w-full divide-y divide-black/5 text-sm dark:divide-white/10">
            <thead class="bg-zinc-50/90 dark:bg-white/5">
              <tr class="text-left text-zinc-500 dark:text-zinc-300">
                <th class="px-4 py-3 font-medium">Serviço</th>
                <th class="px-4 py-3 font-medium">Categoria</th>
                <th class="px-4 py-3 font-medium text-right">Ações</th>
              </tr>
            </thead>
            <tbody class="divide-y divide-black/5 bg-white/80 dark:divide-white/10 dark:bg-transparent">
              <tr v-if="isLoading">
                <td colspan="3" class="px-4 py-8 text-center text-zinc-500 dark:text-zinc-400">
                  Carregando serviços...
                </td>
              </tr>
              <tr v-else-if="errorMessage">
                <td colspan="3" class="px-4 py-8 text-center text-rose-600 dark:text-rose-300">
                  {{ errorMessage }}
                </td>
              </tr>
              <tr v-else-if="filteredServices.length === 0">
                <td colspan="3" class="px-4 py-8 text-center text-zinc-500 dark:text-zinc-400">
                  Nenhum serviço encontrado.
                </td>
              </tr>
              <tr v-for="service in filteredServices" :key="service.id">
                <td class="px-4 py-4">
                  <p class="font-semibold text-zinc-900 dark:text-white">{{ service.name }}</p>
                </td>
                <td class="px-4 py-4">
                  <span class="rounded-md bg-[color:var(--color-brand-soft)] px-2 py-1 text-xs font-medium text-[color:var(--color-brand)] dark:bg-white/10 dark:text-white">
                    {{ service.category }}
                  </span>
                </td>
                <td class="px-4 py-4">
                  <div class="flex justify-end gap-2">
                    <button
                      class="rounded-lg border border-black/10 px-3 py-2 text-xs font-medium text-zinc-700 transition hover:border-[color:var(--color-brand)] hover:text-[color:var(--color-brand)] dark:border-white/10 dark:text-zinc-200"
                      @click="startEdit(service)"
                    >
                      Editar
                    </button>
                    <button
                      class="rounded-lg border border-rose-200 px-3 py-2 text-xs font-medium text-rose-600 transition hover:bg-rose-50 dark:border-rose-400/30 dark:text-rose-300 dark:hover:bg-rose-500/10"
                      @click="removeService(service)"
                    >
                      Excluir
                    </button>
                  </div>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </article>
    </section>

    <div
      v-if="isFormPanelOpen"
      class="fixed inset-0 z-40 bg-black/35 backdrop-blur-[1px]"
      @click="closeFormPanel"
    />

    <aside
      :class="[
        'fixed right-0 top-0 z-50 flex h-full w-full max-w-xl flex-col border-l border-black/10 bg-[color:var(--color-surface)] shadow-2xl transition-transform duration-300 dark:border-white/10 dark:bg-[#171717]',
        isFormPanelOpen ? 'translate-x-0' : 'translate-x-full'
      ]"
    >
      <div class="flex items-start justify-between gap-4 border-b border-black/5 px-5 py-5 dark:border-white/10">
        <div>
          <p class="text-sm font-semibold text-zinc-900 dark:text-white">
            {{ editingId ? 'Editar serviço' : 'Novo serviço' }}
          </p>
          <p class="mt-1 text-sm text-zinc-500 dark:text-zinc-400">
            Gerencie o catálogo de serviços sem sair da listagem.
          </p>
        </div>
        <button
          class="rounded-lg border border-black/10 px-3 py-2 text-xs font-medium text-zinc-700 transition hover:border-[color:var(--color-brand)] hover:text-[color:var(--color-brand)] dark:border-white/10 dark:text-zinc-200"
          @click="closeFormPanel"
        >
          Fechar
        </button>
      </div>

      <div class="flex-1 overflow-y-auto px-5 py-5">
        <form class="space-y-5" @submit.prevent="saveService">
          <label class="space-y-2">
            <span class="text-sm font-medium text-zinc-700 dark:text-zinc-200">Nome do serviço</span>
            <input
              v-model="form.name"
              type="text"
              class="w-full rounded-lg border border-black/10 bg-white px-3 py-2 text-sm text-zinc-800 outline-none transition focus:border-[color:var(--color-brand)] dark:border-white/10 dark:bg-[#1b1b1b] dark:text-white"
            />
          </label>

          <label class="space-y-2">
            <span class="text-sm font-medium text-zinc-700 dark:text-zinc-200">Categoria</span>
            <select
              v-model="form.category"
              class="w-full rounded-lg border border-black/10 bg-white px-3 py-2 text-sm text-zinc-800 outline-none transition focus:border-[color:var(--color-brand)] dark:border-white/10 dark:bg-[#1b1b1b] dark:text-white"
            >
              <option value="Salão de beleza">Salão de beleza</option>
              <option value="Barbearia">Barbearia</option>
              <option value="Estética">Estética</option>
            </select>
          </label>

          <div v-if="formError" class="rounded-lg border border-rose-200 bg-rose-50 px-3 py-3 text-sm text-rose-700 dark:border-rose-400/30 dark:bg-rose-500/10 dark:text-rose-200">
            {{ formError }}
          </div>

          <div class="sticky bottom-0 flex gap-3 border-t border-black/5 bg-[color:var(--color-surface)] pt-4 dark:border-white/10 dark:bg-[#171717]">
            <button
              type="button"
              class="flex-1 rounded-lg border border-black/10 px-4 py-3 text-sm font-medium text-zinc-700 transition hover:border-[color:var(--color-brand)] hover:text-[color:var(--color-brand)] dark:border-white/10 dark:text-zinc-200"
              @click="closeFormPanel"
            >
              Cancelar
            </button>
            <button
              type="submit"
              class="flex-1 rounded-lg bg-[color:var(--color-brand)] px-4 py-3 text-sm font-medium text-white transition hover:opacity-90 disabled:cursor-not-allowed disabled:opacity-60"
              :disabled="isSaving"
            >
              {{ isSaving ? 'Salvando...' : editingId ? 'Salvar alterações' : 'Cadastrar serviço' }}
            </button>
          </div>
        </form>
      </div>
    </aside>
  </MainLayout>
</template>

<script setup lang="ts">
import { computed, onMounted, reactive, ref } from 'vue'
import MainLayout from '@/layouts/MainLayout.vue'

type Service = {
  id: string
  name: string
  category: string
}

const apiBaseUrl = `${import.meta.env.VITE_API_BASE_URL ?? 'http://localhost:5222'}/api`

const services = ref<Service[]>([])
const search = ref('')
const isLoading = ref(false)
const isSaving = ref(false)
const isFormPanelOpen = ref(false)
const errorMessage = ref('')
const formError = ref('')
const editingId = ref<string | null>(null)

const form = reactive({
  name: '',
  category: 'Salão de beleza',
})

const filteredServices = computed(() => {
  const query = search.value.trim().toLowerCase()
  if (!query) {
    return services.value
  }

  return services.value.filter((service) =>
    service.name.toLowerCase().includes(query) ||
    service.category.toLowerCase().includes(query))
})

function resetForm() {
  editingId.value = null
  formError.value = ''
  form.name = ''
  form.category = 'Salão de beleza'
}

function closeFormPanel() {
  isFormPanelOpen.value = false
  resetForm()
}

function startCreate() {
  resetForm()
  isFormPanelOpen.value = true
}

function startEdit(service: Service) {
  editingId.value = service.id
  formError.value = ''
  form.name = service.name
  form.category = service.category
  isFormPanelOpen.value = true
}

async function fetchServices() {
  isLoading.value = true
  errorMessage.value = ''

  try {
    const response = await fetch(`${apiBaseUrl}/services`)
    if (!response.ok) {
      throw new Error('Não foi possível carregar os serviços.')
    }

    services.value = await response.json()
  } catch (error) {
    errorMessage.value = error instanceof Error ? error.message : 'Erro inesperado ao carregar serviços.'
  } finally {
    isLoading.value = false
  }
}

async function saveService() {
  formError.value = ''

  if (!form.name.trim()) {
    formError.value = 'Informe o nome do serviço.'
    return
  }

  if (!form.category.trim()) {
    formError.value = 'Informe a categoria.'
    return
  }

  isSaving.value = true

  try {
    const response = await fetch(
      editingId.value ? `${apiBaseUrl}/services/${editingId.value}` : `${apiBaseUrl}/services`,
      {
        method: editingId.value ? 'PUT' : 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({
          name: form.name,
          category: form.category,
        }),
      },
    )

    if (!response.ok) {
      const problem = await response.json().catch(() => null)
      const detail = problem?.errors?.service?.[0] ?? 'Não foi possível salvar o serviço.'
      throw new Error(detail)
    }

    closeFormPanel()
    await fetchServices()
  } catch (error) {
    formError.value = error instanceof Error ? error.message : 'Erro inesperado ao salvar serviço.'
  } finally {
    isSaving.value = false
  }
}

async function removeService(service: Service) {
  const confirmed = window.confirm(`Excluir o serviço ${service.name}?`)
  if (!confirmed) {
    return
  }

  try {
    const response = await fetch(`${apiBaseUrl}/services/${service.id}`, {
      method: 'DELETE',
    })

    if (!response.ok) {
      throw new Error('Não foi possível excluir o serviço.')
    }

    await fetchServices()
    if (editingId.value === service.id) {
      closeFormPanel()
    }
  } catch (error) {
    formError.value = error instanceof Error ? error.message : 'Erro inesperado ao excluir serviço.'
  }
}

onMounted(fetchServices)
</script>
