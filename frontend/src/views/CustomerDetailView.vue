<template>
  <MainLayout>
    <section class="space-y-4">
      <button
        class="mb-2 rounded-lg border border-black/10 px-4 py-2 text-sm font-medium text-zinc-700 transition hover:border-[color:var(--color-brand)] hover:text-[color:var(--color-brand)] dark:border-white/10 dark:text-zinc-200"
        @click="goBack"
      >
        Voltar
      </button>

      <article class="rounded-xl border border-black/5 bg-white/80 p-5 shadow-sm backdrop-blur dark:border-white/10 dark:bg-[#212121]/90">
        <div v-if="isLoading" class="py-8 text-center text-zinc-500 dark:text-zinc-400">
          Carregando detalhes do cliente...
        </div>

        <div v-else-if="errorMessage" class="rounded-lg border border-rose-200 bg-rose-50 px-4 py-3 text-rose-700 dark:border-rose-400/30 dark:bg-rose-500/10 dark:text-rose-200">
          {{ errorMessage }}
        </div>

        <div v-else-if="customer" class="space-y-6">
          <div class="flex flex-col gap-4 lg:flex-row lg:items-start lg:justify-between">
            <div>
              <p class="text-xs font-semibold uppercase tracking-[0.28em] text-zinc-500 dark:text-zinc-400">
                Perfil do cliente
              </p>
              <h1 class="mt-2 text-3xl font-semibold text-zinc-900 dark:text-white">
                {{ customer.name }}
              </h1>
              <p class="mt-2 text-sm text-zinc-500 dark:text-zinc-400">
                ID: {{ customer.id }}
              </p>
            </div>

            <div class="border-b border-black/10 dark:border-white/10">
              <div class="-mb-px flex gap-6" role="tablist" aria-label="Abas do cliente">
                <button
                  v-for="tab in tabs"
                  :key="tab.id"
                  type="button"
                  role="tab"
                  :aria-selected="activeTab === tab.id"
                  class="group relative pb-4 text-sm font-semibold transition"
                  :class="activeTab === tab.id
                    ? 'text-[color:var(--color-brand)]'
                    : 'text-zinc-500 hover:text-zinc-900 dark:text-zinc-400 dark:hover:text-white'"
                  @click="activeTab = tab.id"
                >
                  <span>{{ tab.label }}</span>
                  <span
                    class="absolute inset-x-0 -bottom-px h-0.5 rounded-full transition"
                    :class="activeTab === tab.id
                      ? 'bg-[color:var(--color-brand)]'
                      : 'bg-transparent group-hover:bg-black/20 dark:group-hover:bg-white/20'"
                  />
                </button>
              </div>
            </div>
          </div>

          <div v-if="activeTab === 'dados'" class="grid gap-4 md:grid-cols-2">
            <div class="rounded-xl border border-black/5 p-4 dark:border-white/10">
              <p class="text-xs font-semibold uppercase tracking-[0.2em] text-zinc-500 dark:text-zinc-400">
                Telefone
              </p>
              <p class="mt-2 text-lg font-medium text-zinc-900 dark:text-white">
                {{ customer.phone }}
              </p>
            </div>

            <div class="rounded-xl border border-black/5 p-4 dark:border-white/10">
              <p class="text-xs font-semibold uppercase tracking-[0.2em] text-zinc-500 dark:text-zinc-400">
                Email
              </p>
              <p class="mt-2 text-lg font-medium text-zinc-900 dark:text-white">
                {{ customer.email }}
              </p>
            </div>
          </div>

          <div v-else class="space-y-4 pt-1">
            <div class="flex flex-col gap-1">
              <p class="text-sm font-semibold text-zinc-900 dark:text-white">
                Agendamentos
              </p>
              <p class="text-sm text-zinc-500 dark:text-zinc-400">
                Aqui vamos listar os agendamentos desse cliente.
              </p>
            </div>

            <div class="rounded-xl border border-dashed border-black/10 bg-white/40 p-6 text-sm text-zinc-500 dark:border-white/10 dark:bg-white/5 dark:text-zinc-400">
              Nenhum agendamento carregado ainda.
            </div>
          </div>
        </div>
      </article>
    </section>
  </MainLayout>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import MainLayout from '@/layouts/MainLayout.vue'

interface Customer {
  id: string
  name: string
  phone: string
  email: string
}

const tabs = [
  { id: 'dados', label: 'Dados Gerais' },
  { id: 'agendamentos', label: 'Agendamentos' },
] as const

const apiBaseUrl = `${import.meta.env.VITE_API_BASE_URL ?? 'http://localhost:5222'}/api`
const router = useRouter()
const route = useRoute()

const activeTab = ref<(typeof tabs)[number]['id']>('dados')
const customer = ref<Customer | null>(null)
const isLoading = ref(false)
const errorMessage = ref('')

const customerId = route.params.id as string

async function loadCustomer() {
  isLoading.value = true
  errorMessage.value = ''

  try {
    const response = await fetch(`${apiBaseUrl}/customers/${customerId}`)
    if (!response.ok) {
      throw new Error('Erro ao carregar cliente.')
    }

    customer.value = await response.json()
  } catch (error) {
    errorMessage.value = error instanceof Error ? error.message : 'Erro ao carregar detalhes do cliente.'
    console.error(error)
  } finally {
    isLoading.value = false
  }
}

function goBack() {
  router.push({ name: 'customers' })
}

onMounted(loadCustomer)
</script>
