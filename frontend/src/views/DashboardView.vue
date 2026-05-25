<template>
  <MainLayout>
    <section class="grid gap-4 xl:grid-cols-[1.8fr_1fr]">
      <div class="grid gap-4 md:grid-cols-3">
        <article
          v-for="card in metricCards"
          :key="card.title"
          class="rounded-xl border border-black/5 bg-white/80 p-5 shadow-sm backdrop-blur dark:border-white/10 dark:bg-[#212121]/90"
        >
          <p class="text-sm text-zinc-500 dark:text-zinc-400">{{ card.title }}</p>
          <p class="mt-4 text-3xl font-semibold text-zinc-900 dark:text-white">{{ card.value }}</p>
          <p class="mt-2 text-sm" :class="card.trendColor">{{ card.trend }}</p>
        </article>
      </div>

      <article class="rounded-xl border border-black/5 bg-white/80 p-5 shadow-sm backdrop-blur dark:border-white/10 dark:bg-[#212121]/90">
        <p class="text-sm font-semibold text-zinc-800 dark:text-zinc-100">Resumo do dia</p>
        <div class="mt-5 space-y-4">
          <div class="rounded-lg bg-[color:var(--color-brand-soft)]/70 p-4 dark:bg-white/5">
            <p class="text-sm text-zinc-600 dark:text-zinc-300">Serviço mais agendado</p>
            <p class="mt-2 text-lg font-semibold text-[color:var(--color-brand)] dark:text-[color:var(--color-brand-soft)]">Corte + barba</p>
          </div>
          <div class="rounded-lg bg-zinc-900 p-4 text-white dark:bg-black">
            <p class="text-sm text-white/70">Profissional em destaque</p>
            <p class="mt-2 text-lg font-semibold">Marcos Ferreira</p>
          </div>
        </div>
      </article>
    </section>

    <section class="mt-4 grid gap-4 lg:grid-cols-[1.4fr_1fr]">
      <article class="rounded-xl border border-black/5 bg-white/80 p-6 shadow-sm backdrop-blur dark:border-white/10 dark:bg-[#212121]/90">
        <div class="flex items-center justify-between">
          <div>
            <p class="text-sm font-semibold text-zinc-800 dark:text-zinc-100">Hub de relatórios</p>
            <p class="mt-1 text-sm text-zinc-500 dark:text-zinc-400">
              Faturamento, agenda, vendas e performance em um fluxo central.
            </p>
          </div>
          <button class="rounded-lg bg-[color:var(--color-brand)] px-4 py-2 text-sm font-medium text-white transition hover:opacity-90">
            Abrir relatórios
          </button>
        </div>

        <div class="mt-6 grid gap-4 md:grid-cols-3">
          <div
            v-for="report in reportCards"
            :key="report.title"
            class="rounded-lg border border-black/5 p-4 dark:border-white/10"
          >
            <p class="text-sm font-medium text-zinc-800 dark:text-zinc-100">{{ report.title }}</p>
            <p class="mt-3 text-sm text-zinc-500 dark:text-zinc-400">{{ report.description }}</p>
          </div>
        </div>
      </article>

      <article class="rounded-xl border border-black/5 bg-white/80 p-6 shadow-sm backdrop-blur dark:border-white/10 dark:bg-[#212121]/90">
        <p class="text-sm font-semibold text-zinc-800 dark:text-zinc-100">Cadastros prontos para detalhar</p>
        <ul class="mt-5 space-y-3">
          <li
            v-for="item in registerItems"
            :key="item"
            class="flex items-center justify-between rounded-lg bg-zinc-50 px-4 py-3 text-sm text-zinc-700 dark:bg-white/5 dark:text-zinc-200"
          >
            <span>{{ item }}</span>
            <span class="rounded-full bg-[color:var(--color-accent)]/15 px-3 py-1 text-xs font-semibold text-[color:var(--color-accent)]">
              Base criada
            </span>
          </li>
        </ul>
      </article>
    </section>
  </MainLayout>
</template>

<script setup lang="ts">
import MainLayout from '@/layouts/MainLayout.vue'

const metricCards = [
  { title: 'Faturamento mensal', value: 'R$ 24.800', trend: '+12% vs. mês anterior', trendColor: 'text-emerald-600 dark:text-emerald-400' },
  { title: 'Agendamentos hoje', value: '38', trend: '6 ainda em andamento', trendColor: 'text-cyan-700 dark:text-cyan-300' },
  { title: 'Ticket médio', value: 'R$ 87', trend: 'Meta de R$ 90', trendColor: 'text-amber-600 dark:text-amber-400' },
]

const reportCards = [
  { title: 'Faturamento', description: 'Compare períodos, serviços e vendas de produtos em poucos cliques.' },
  { title: 'Serviços mais agendados', description: 'Entenda a demanda e priorize o portfólio que gera mais retorno.' },
  { title: 'Desempenho de profissionais', description: 'Visualize volume, receita e constância por profissional.' },
]

const registerItems = ['Serviços', 'Produtos', 'Profissionais', 'Clientes', 'Usuários']
</script>
