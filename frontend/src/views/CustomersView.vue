<template>
  <MainLayout>
    <section class="space-y-4">
      <article class="rounded-xl border border-black/5 bg-white/80 p-5 shadow-sm backdrop-blur dark:border-white/10 dark:bg-[#212121]/90">
        <div class="flex flex-col gap-4 lg:flex-row lg:items-center lg:justify-between">
          <div>
            <p class="text-sm font-semibold text-zinc-900 dark:text-white">Clientes</p>
            <p class="mt-1 text-sm text-zinc-500 dark:text-zinc-400">
              Consulte a lista de clientes e abra os detalhes quando precisar.
            </p>
          </div>

          <div class="flex flex-col gap-3 sm:flex-row">
            <input
              v-model="search"
              type="search"
              placeholder="Buscar por nome, telefone ou e-mail"
              class="min-w-0 rounded-lg border border-black/10 bg-white px-3 py-2 text-sm text-zinc-800 outline-none transition focus:border-[color:var(--color-brand)] dark:border-white/10 dark:bg-[#1b1b1b] dark:text-white"
            />
            <button
              class="rounded-lg border border-black/10 px-4 py-2 text-sm font-medium text-zinc-700 transition hover:border-[color:var(--color-brand)] hover:text-[color:var(--color-brand)] dark:border-white/10 dark:text-zinc-200"
              @click="startCreate"
            >
              Novo cliente
            </button>
          </div>
        </div>

        <div class="mt-5 overflow-hidden rounded-xl border border-black/5 dark:border-white/10">
          <table class="min-w-full divide-y divide-black/5 text-sm dark:divide-white/10">
            <thead class="bg-zinc-50/90 dark:bg-white/5">
              <tr class="text-left text-zinc-500 dark:text-zinc-300">
                <th class="px-4 py-3 font-medium">Nome</th>
                <th class="px-4 py-3 font-medium">Telefone</th>
                <th class="px-4 py-3 font-medium">Email</th>
                <th class="px-4 py-3 font-medium text-right">Ações</th>
              </tr>
            </thead>
            <tbody class="divide-y divide-black/5 bg-white/80 dark:divide-white/10 dark:bg-transparent">
              <tr v-if="isLoading">
                <td colspan="4" class="px-4 py-8 text-center text-zinc-500 dark:text-zinc-400">
                  Carregando clientes...
                </td>
              </tr>
              <tr v-else-if="errorMessage">
                <td colspan="4" class="px-4 py-8 text-center text-rose-600 dark:text-rose-300">
                  {{ errorMessage }}
                </td>
              </tr>
              <tr v-else-if="filteredCustomers.length === 0">
                <td colspan="4" class="px-4 py-8 text-center text-zinc-500 dark:text-zinc-400">
                  Nenhum cliente encontrado.
                </td>
              </tr>
              <tr
                v-for="customer in filteredCustomers"
                :key="customer.id"
                class="bg-white/40 transition hover:bg-zinc-100/40 dark:bg-white/5 dark:hover:bg-white/10"
              >
                <td class="px-4 py-4">
                  <p class="font-semibold text-zinc-900 dark:text-white">{{ customer.name }}</p>
                </td>
                <td class="px-4 py-4">
                  <p class="text-zinc-700 dark:text-zinc-300">{{ customer.phone }}</p>
                </td>
                <td class="px-4 py-4">
                  <p class="text-zinc-700 dark:text-zinc-300">{{ customer.email }}</p>
                </td>
                <td class="px-4 py-4 text-right">
                  <button
                    class="rounded-lg border border-blue-200 bg-gradient-to-r from-blue-50 to-blue-50/50 px-3 py-2 text-xs font-medium text-blue-700 transition hover:from-blue-100 dark:border-blue-900/30 dark:from-blue-950/30 dark:to-blue-950/20 dark:text-blue-300"
                    @click="viewCustomer(customer)"
                  >
                    Visualizar
                  </button>
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
          <p class="text-sm font-semibold text-zinc-900 dark:text-white">Novo cliente</p>
          <p class="mt-1 text-sm text-zinc-500 dark:text-zinc-400">
            Cadastre o cliente rapidamente para começar a usá-lo no sistema.
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
        <form class="space-y-5" @submit.prevent="saveCustomer">
          <label class="space-y-2">
            <span class="text-sm font-medium text-zinc-700 dark:text-zinc-200">Nome</span>
            <input
              v-model="form.name"
              type="text"
              class="w-full rounded-lg border border-black/10 bg-white px-3 py-2 text-sm text-zinc-800 outline-none transition focus:border-[color:var(--color-brand)] dark:border-white/10 dark:bg-[#1b1b1b] dark:text-white"
              placeholder="Nome do cliente"
            />
          </label>

          <label class="space-y-2">
            <span class="text-sm font-medium text-zinc-700 dark:text-zinc-200">Telefone</span>
            <input
              v-model="form.phone"
              type="tel"
              class="w-full rounded-lg border border-black/10 bg-white px-3 py-2 text-sm text-zinc-800 outline-none transition focus:border-[color:var(--color-brand)] dark:border-white/10 dark:bg-[#1b1b1b] dark:text-white"
              placeholder="(00) 00000-0000"
            />
          </label>

          <label class="space-y-2">
            <span class="text-sm font-medium text-zinc-700 dark:text-zinc-200">Email</span>
            <input
              v-model="form.email"
              type="email"
              class="w-full rounded-lg border border-black/10 bg-white px-3 py-2 text-sm text-zinc-800 outline-none transition focus:border-[color:var(--color-brand)] dark:border-white/10 dark:bg-[#1b1b1b] dark:text-white"
              placeholder="email@exemplo.com"
            />
          </label>

          <div
            v-if="formError"
            class="rounded-lg border border-rose-200 bg-rose-50 px-3 py-3 text-sm text-rose-700 dark:border-rose-400/30 dark:bg-rose-500/10 dark:text-rose-200"
          >
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
              {{ isSaving ? 'Salvando...' : 'Cadastrar cliente' }}
            </button>
          </div>
        </form>
      </div>
    </aside>
  </MainLayout>
</template>

<script setup lang="ts">
import { computed, onMounted, reactive, ref } from 'vue'
import { useRouter } from 'vue-router'
import MainLayout from '@/layouts/MainLayout.vue'

interface Customer {
  id: string
  name: string
  phone: string
  email: string
}

interface CustomerForm {
  name: string
  phone: string
  email: string
}

const apiBaseUrl = `${import.meta.env.VITE_API_BASE_URL ?? 'http://localhost:5222'}/api`
const router = useRouter()

const customers = ref<Customer[]>([])
const search = ref('')
const isLoading = ref(false)
const isSaving = ref(false)
const errorMessage = ref('')
const formError = ref('')
const isFormPanelOpen = ref(false)

const form = reactive<CustomerForm>({
  name: '',
  phone: '',
  email: '',
})

const filteredCustomers = computed(() => {
  const query = search.value.trim().toLowerCase()
  if (!query) {
    return customers.value
  }

  return customers.value.filter((customer) =>
    customer.name.toLowerCase().includes(query) ||
    customer.phone.toLowerCase().includes(query) ||
    customer.email.toLowerCase().includes(query),
  )
})

function resetForm() {
  form.name = ''
  form.phone = ''
  form.email = ''
  formError.value = ''
}

function startCreate() {
  resetForm()
  isFormPanelOpen.value = true
}

function closeFormPanel() {
  isFormPanelOpen.value = false
  formError.value = ''
}

async function loadCustomers() {
  isLoading.value = true
  errorMessage.value = ''

  try {
    const response = await fetch(`${apiBaseUrl}/customers`)
    if (!response.ok) {
      throw new Error('Nao foi possivel carregar os clientes.')
    }

    customers.value = await response.json()
  } catch (error) {
    errorMessage.value = error instanceof Error ? error.message : 'Erro inesperado ao carregar clientes.'
  } finally {
    isLoading.value = false
  }
}

function validateForm() {
  if (!form.name.trim()) return 'Informe o nome do cliente.'
  if (!form.phone.trim()) return 'Informe o telefone.'
  if (!form.email.trim()) return 'Informe o email.'
  return ''
}

async function saveCustomer() {
  formError.value = validateForm()
  if (formError.value) {
    return
  }

  isSaving.value = true

  try {
    const response = await fetch(`${apiBaseUrl}/customers`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({
        name: form.name,
        phone: form.phone,
        email: form.email,
      }),
    })

    if (!response.ok) {
      const problem = await response.json().catch(() => null)
      const detail = problem?.errors?.customer?.[0] ?? 'Nao foi possivel salvar o cliente.'
      throw new Error(detail)
    }

    closeFormPanel()
    await loadCustomers()
  } catch (error) {
    formError.value = error instanceof Error ? error.message : 'Erro inesperado ao salvar cliente.'
  } finally {
    isSaving.value = false
  }
}

function viewCustomer(customer: Customer) {
  router.push({ name: 'customer-detail', params: { id: customer.id } })
}

onMounted(loadCustomers)
</script>
