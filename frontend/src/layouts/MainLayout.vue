<template>
  <div class="min-h-screen text-zinc-900 transition-colors duration-300 dark:text-zinc-100">
    <div class="flex min-h-screen">
      <aside
        :class="[
          'border-r border-black/5 bg-[color:var(--color-brand)] backdrop-blur-xl transition-all duration-300',
          isSidebarCollapsed ? 'w-24' : 'w-76'
        ]"
      >
        <div class="flex h-full flex-col px-4 py-5">
          <div class="flex items-center justify-between pb-6">
            <div v-if="!isSidebarCollapsed">
              <p class="text-xs font-semibold uppercase tracking-[0.32em] text-white/80">
                VisageSmart
              </p>
              <h1 class="mt-2 text-2xl font-semibold text-white">
                Manager
              </h1>
            </div>
            <button
              class="rounded-lg border border-white/20 p-2 text-white transition hover:border-white/40 hover:bg-white/10"
              @click="isSidebarCollapsed = !isSidebarCollapsed"
            >
              <span class="sr-only">Alternar menu</span>
              <svg viewBox="0 0 24 24" class="h-5 w-5 fill-none stroke-current stroke-2">
                <path d="M4 6h16M4 12h16M4 18h16" />
              </svg>
            </button>
          </div>

          <nav class="space-y-6">
            <div class="space-y-2">
              <MenuLink :collapsed="isSidebarCollapsed" to="/" label="Dashboard">
                <template #icon>
                  <CircleIcon />
                </template>
              </MenuLink>
            </div>

            <div class="space-y-2">
              <button
                class="flex w-full items-center justify-between rounded-lg px-3 py-3 text-left text-sm font-medium text-white transition hover:bg-white/20"
                @click="isCatalogOpen = !isCatalogOpen"
              >
                <span class="flex items-center gap-3">
                  <FolderIcon />
                  <span v-if="!isSidebarCollapsed">Cadastros</span>
                </span>
                <svg
                  v-if="!isSidebarCollapsed"
                  viewBox="0 0 24 24"
                  class="h-4 w-4 fill-none stroke-current stroke-2 transition"
                  :class="isCatalogOpen ? 'rotate-180' : ''"
                >
                  <path d="m6 9 6 6 6-6" />
                </svg>
              </button>

              <div v-if="isCatalogOpen && !isSidebarCollapsed" class="space-y-1 pl-4">
                <MenuLink v-for="item in registerLinks" :key="item.label" :collapsed="false" :to="item.to" :label="item.label">
                  <template #icon>
                    <SmallDot />
                  </template>
                </MenuLink>
              </div>
            </div>

            <div class="space-y-2">
              <MenuLink
                v-for="item in operationLinks"
                :key="item.label"
                :collapsed="isSidebarCollapsed"
                :to="item.to"
                :label="item.label"
              >
                <template #icon>
                  <component :is="item.icon" />
                </template>
              </MenuLink>
            </div>

            <div class="space-y-2">
              <MenuLink :collapsed="isSidebarCollapsed" to="/reports" label="Relatórios e Gráficos">
                <template #icon>
                  <ChartIcon />
                </template>
              </MenuLink>
            </div>
          </nav>

          <div class="mt-auto rounded-xl bg-white/20 p-4 text-white shadow-lg">
            <p v-if="!isSidebarCollapsed" class="text-sm font-semibold">
              Gestão com visão completa
            </p>
            <p v-if="!isSidebarCollapsed" class="mt-2 text-sm text-white/80">
              Faturamento, agenda e desempenho em um único painel.
            </p>
          </div>
        </div>
      </aside>

      <div class="flex min-w-0 flex-1 flex-col">
        <header class="border-b border-black/5 bg-[color:var(--color-brand)] px-4 py-4 shadow-lg sm:px-6">
          <div class="flex flex-col gap-4 lg:flex-row lg:items-center lg:justify-between">
            <div>
              <p class="text-xs font-semibold uppercase tracking-[0.28em] text-white/80">
                Operação diária
              </p>
              <h2 class="mt-1 text-2xl font-semibold text-white">
                Dashboard Principal
              </h2>
            </div>

            <div class="flex items-center gap-3 self-start lg:self-auto">
              <button class="rounded-lg border border-white/20 bg-white/20 px-4 py-2 text-sm font-medium text-white transition hover:border-white/40 hover:bg-white/30">
                3 notificações
              </button>

              <div class="flex items-center gap-3 rounded-lg border border-white/20 bg-white/20 px-3 py-2">
                <div class="flex h-10 w-10 items-center justify-center rounded-lg bg-white/30 text-sm font-semibold text-white">
                  AD
                </div>
                <div class="hidden sm:block">
                  <p class="text-sm font-semibold text-white">Admin</p>
                  <p class="text-xs text-white/80">Gerente operacional</p>
                </div>
              </div>
            </div>
          </div>
        </header>

        <main class="flex-1 p-4 sm:p-6">
          <slot />
        </main>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue'
import MenuLink from '@/components/navigation/MenuLink.vue'
import { CalendarIcon, ChartIcon, CircleIcon, FolderIcon, GiftIcon, MoonIcon, SmallDot, SunIcon } from '@/components/navigation/icons'
import { useThemeStore } from '@/stores/theme'

const theme = useThemeStore()
const isSidebarCollapsed = ref(false)
const isCatalogOpen = ref(true)

const registerLinks = [
  { label: 'Serviços', to: '/services' },
  { label: 'Produtos', to: '/products' },
  { label: 'Profissionais', to: '/professionals' },
  { label: 'Clientes', to: '/customers' },
  { label: 'Usuários', to: '/users' },
]

const operationLinks = [
  { label: 'Agenda', to: '/agenda', icon: CalendarIcon },
  { label: 'Programa de fidelidade', to: '/fidelidade', icon: GiftIcon },
]

onMounted(() => {
  theme.initializeTheme()
})
</script>
