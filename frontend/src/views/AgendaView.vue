<template>
  <MainLayout>
    <section class="space-y-4">
      <article class="rounded-xl border border-black/5 bg-white/80 p-5 shadow-sm backdrop-blur dark:border-white/10 dark:bg-[#212121]/90">
        <p class="text-sm font-semibold text-zinc-900 dark:text-white">Agenda</p>
        <h1 class="mt-2 text-3xl font-semibold text-zinc-900 dark:text-white">
          Agenda do salão
        </h1>
        <p class="mt-2 text-sm text-zinc-500 dark:text-zinc-400">
          Esta área concentra horários, atendimentos e encaixes.
        </p>
      </article>

      <article class="rounded-xl border border-black/5 bg-white/80 p-5 shadow-sm backdrop-blur dark:border-white/10 dark:bg-[#212121]/90">
        <div class="flex flex-col gap-4 md:flex-row md:items-center md:justify-between">
          <div>
            <p class="text-sm font-semibold text-zinc-900 dark:text-white">Calendário</p>
            <h2 class="mt-2 text-2xl font-semibold text-zinc-900 dark:text-white">Visualização de agenda</h2>
            <p class="mt-2 text-sm text-zinc-500 dark:text-zinc-400">
              Navegue pela agenda em Dia, Semana ou Mês.
            </p>
          </div>

          <div class="flex flex-col gap-3 md:flex-row md:items-center md:justify-between">
          <div class="flex-1 min-w-0">
            <label class="mb-2 block text-sm font-semibold text-zinc-900 dark:text-white">Filtrar por profissional</label>
            <select
              v-model="selectedProfessional"
              class="min-w-0 rounded-lg border border-black/10 bg-white px-4 py-2 text-sm text-zinc-700 outline-none transition focus:border-[color:var(--color-brand)] focus:ring-[color:var(--color-brand)]/10 dark:border-white/10 dark:bg-[#161616] dark:text-zinc-100"
            >
              <option value="all">Todos</option>
              <option v-for="professional in professionalOptions" :key="professional" :value="professional">
                {{ professional }}
              </option>
            </select>
          </div>

          <div class="flex flex-wrap gap-2">
            <button
              v-for="option in viewOptions"
              :key="option.id"
              type="button"
              @click="selectedView = option.id"
              :class="[
                'rounded-full px-4 py-2 text-sm font-semibold transition',
                selectedView === option.id
                  ? 'bg-[color:var(--color-brand)] text-white shadow-sm'
                  : 'border border-black/10 bg-white text-zinc-700 hover:border-[color:var(--color-brand)] hover:text-[color:var(--color-brand)] dark:border-white/10 dark:bg-[#161616] dark:text-zinc-200'
              ]"
            >
              {{ option.label }}
            </button>
          </div>
        </div>
      </div>

        <div class="mt-6 space-y-4">
          <div class="flex flex-col gap-3 rounded-2xl border border-black/5 bg-zinc-50 p-4 text-zinc-700 dark:border-white/10 dark:bg-white/5 dark:text-zinc-100 sm:flex-row sm:items-center sm:justify-between">
            <div>
              <p class="text-xs uppercase tracking-[0.18em] text-zinc-500 dark:text-zinc-400">{{ selectedViewLabel }}</p>
              <p class="mt-2 text-xl font-semibold text-zinc-900 dark:text-white">{{ formattedRange }}</p>
            </div>
            <div class="flex items-center gap-2">
              <button
                type="button"
                class="rounded-lg border border-black/10 bg-white px-3 py-2 text-sm font-medium text-zinc-700 transition hover:border-[color:var(--color-brand)] hover:text-[color:var(--color-brand)] dark:border-white/10 dark:bg-[#161616] dark:text-zinc-200"
                @click="previousPeriod"
              >
                Anterior
              </button>
              <button
                type="button"
                class="rounded-lg border border-black/10 bg-white px-3 py-2 text-sm font-medium text-zinc-700 transition hover:border-[color:var(--color-brand)] hover:text-[color:var(--color-brand)] dark:border-white/10 dark:bg-[#161616] dark:text-zinc-200"
                @click="nextPeriod"
              >
                Próximo
              </button>
            </div>
          </div>

          <div>
            <div v-if="selectedView === 'month'" class="overflow-hidden rounded-3xl border border-black/5 bg-white shadow-sm dark:border-white/10 dark:bg-[#111111]">
              <div class="grid grid-cols-7 gap-px bg-black/5 text-center text-xs uppercase tracking-[0.2em] text-zinc-500 dark:bg-white/5 dark:text-zinc-400">
                <span class="bg-zinc-50 py-3 dark:bg-[#161616]">Seg</span>
                <span class="bg-zinc-50 py-3 dark:bg-[#161616]">Ter</span>
                <span class="bg-zinc-50 py-3 dark:bg-[#161616]">Qua</span>
                <span class="bg-zinc-50 py-3 dark:bg-[#161616]">Qui</span>
                <span class="bg-zinc-50 py-3 dark:bg-[#161616]">Sex</span>
                <span class="bg-zinc-50 py-3 dark:bg-[#161616]">Sáb</span>
                <span class="bg-zinc-50 py-3 dark:bg-[#161616]">Dom</span>
              </div>
              <div class="grid grid-cols-7 gap-px bg-black/5">
                <div
                  v-for="day in monthDays"
                  :key="day.date.toISOString()"
                  :class="[
                    'min-h-[120px] overflow-hidden border border-black/5 bg-white/95 p-3 text-sm text-zinc-700 dark:border-white/10 dark:bg-[#111111] dark:text-zinc-100',
                    !day.isCurrentMonth ? 'bg-zinc-50 text-zinc-400 dark:bg-[#121212] dark:text-zinc-500' : '',
                    day.isToday ? 'border-[color:var(--color-brand)] ring-1 ring-[color:var(--color-brand)]/10' : ''
                  ]"
                >
                  <div class="flex items-center justify-between">
                    <span class="font-semibold">{{ day.date.getDate() }}</span>
                    <span v-if="day.isToday" class="rounded-full bg-[color:var(--color-brand)] px-2 py-0.5 text-[10px] font-semibold text-white">Hoje</span>
                  </div>
                  <div class="mt-3 space-y-2">
                    <p v-if="day.event" class="rounded-2xl bg-[color:var(--color-brand)]/10 px-2 py-1 text-[13px] font-medium text-[color:var(--color-brand)]">{{ day.event }}</p>
                    <p v-else class="text-xs text-zinc-500 dark:text-zinc-500">Sem compromissos</p>
                  </div>
                </div>
              </div>
            </div>

            <div v-else-if="selectedView === 'week'" class="overflow-hidden rounded-3xl border border-black/5 bg-white shadow-sm dark:border-white/10 dark:bg-[#111111]">
              <div class="grid grid-cols-7 bg-zinc-50 text-center text-xs uppercase tracking-[0.18em] text-zinc-500 dark:bg-[#161616] dark:text-zinc-400">
                <div v-for="day in weekDays" :key="day.date.toISOString()" class="border-r border-black/5 py-3 last:border-r-0 dark:border-white/10">
                  <p class="font-semibold text-zinc-900 dark:text-white">{{ day.shortLabel }}</p>
                  <p class="mt-1 text-sm text-zinc-500 dark:text-zinc-400">{{ day.day }}</p>
                </div>
              </div>
              <div class="grid grid-cols-7 gap-px bg-black/5">
                <div
                  v-for="day in weekDays"
                  :key="day.date.toISOString()"
                  class="min-h-[220px] overflow-hidden border border-black/5 bg-white/95 p-3 text-sm text-zinc-700 dark:border-white/10 dark:bg-[#111111] dark:text-zinc-100"
                >
                  <div class="space-y-3">
                    <div v-for="event in day.events" :key="event.title" class="rounded-2xl bg-[color:var(--color-brand)]/10 p-3 text-[13px] text-[color:var(--color-brand)]">
                      <p class="font-semibold">{{ event.title }}</p>
                      <p class="text-xs text-zinc-600 dark:text-zinc-300">{{ event.time }}</p>
                    </div>
                    <p v-if="day.events.length === 0" class="text-xs text-zinc-500 dark:text-zinc-500">Nenhum agendamento</p>
                  </div>
                </div>
              </div>
            </div>

            <div v-else class="overflow-hidden rounded-3xl border border-black/5 bg-white shadow-sm dark:border-white/10 dark:bg-[#111111]">
              <div class="grid grid-cols-4 bg-zinc-50 text-left text-xs uppercase tracking-[0.18em] text-zinc-500 dark:bg-[#161616] dark:text-zinc-400">
                <span class="border-r border-black/5 px-3 py-3 dark:border-white/10">Hora</span>
                <span class="border-r border-black/5 px-3 py-3 dark:border-white/10">Serviço</span>
                <span class="border-r border-black/5 px-3 py-3 dark:border-white/10">Profissional</span>
                <span class="px-3 py-3">Cliente</span>
              </div>
              <div class="divide-y divide-black/5 bg-white/95 dark:divide-white/10 dark:bg-[#111111]">
                <div v-for="slot in daySlots" :key="slot.time" class="grid grid-cols-4 gap-2 px-3 py-4 text-sm text-zinc-700 dark:text-zinc-100">
                  <span class="font-medium">{{ slot.time }}</span>
                  <span>{{ slot.service }}</span>
                  <span>{{ slot.professional }}</span>
                  <span>{{ slot.client }}</span>
                </div>
              </div>
            </div>
          </div>
        </div>
      </article>
    </section>
  </MainLayout>
</template>

<script setup lang="ts">
import { computed, ref } from 'vue'
import MainLayout from '@/layouts/MainLayout.vue'

type ViewMode = 'day' | 'week' | 'month'

const viewOptions = [
  { id: 'day' as ViewMode, label: 'Dia' },
  { id: 'week' as ViewMode, label: 'Semana' },
  { id: 'month' as ViewMode, label: 'Mês' },
]

const selectedView = ref<ViewMode>('week')
const currentDate = ref(new Date())

const appointments = [
  { date: new Date(), time: '10:00', service: 'Corte + Barba', professional: 'Mariana', client: 'Ana' },
  { date: new Date(new Date().setDate(new Date().getDate() + 1)), time: '14:30', service: 'Manicure', professional: 'Paula', client: 'Carla' },
  { date: new Date(new Date().setDate(new Date().getDate() + 2)), time: '09:00', service: 'Coloração', professional: 'Lucas', client: 'João' },
]

const selectedProfessional = ref('all')
const professionalOptions = computed(() => {
  return Array.from(new Set(appointments.map((item) => item.professional))).sort()
})

const filteredAppointments = computed(() => {
  if (selectedProfessional.value === 'all') {
    return appointments
  }
  return appointments.filter((item) => item.professional === selectedProfessional.value)
})

const selectedViewLabel = computed(() => {
  switch (selectedView.value) {
    case 'day':
      return 'Visão do dia'
    case 'week':
      return 'Visão da semana'
    default:
      return 'Visão do mês'
  }
})

const formattedRange = computed(() => {
  const formatter = new Intl.DateTimeFormat('pt-BR', { month: 'long', year: 'numeric' })
  if (selectedView.value === 'month') {
    return formatter.format(currentDate.value)
  }

  if (selectedView.value === 'week') {
    const startOfWeek = new Date(currentDate.value)
    const endOfWeek = new Date(currentDate.value)
    const diff = (currentDate.value.getDay() + 6) % 7
    startOfWeek.setDate(currentDate.value.getDate() - diff)
    endOfWeek.setDate(startOfWeek.getDate() + 6)
    const startDay = startOfWeek.toLocaleDateString('pt-BR', { day: '2-digit', month: 'short' })
    const endDay = endOfWeek.toLocaleDateString('pt-BR', { day: '2-digit', month: 'short' })
    return `${startDay} — ${endDay}`
  }

  return currentDate.value.toLocaleDateString('pt-BR', { weekday: 'long', day: '2-digit', month: 'long' })
})

const monthDays = computed(() => {
  const year = currentDate.value.getFullYear()
  const month = currentDate.value.getMonth()
  const firstOfMonth = new Date(year, month, 1)
  const lastOfMonth = new Date(year, month + 1, 0)

  const firstDayIndex = (firstOfMonth.getDay() + 6) % 7
  const totalCells = Math.ceil((firstDayIndex + lastOfMonth.getDate()) / 7) * 7
  const startDate = new Date(firstOfMonth)
  startDate.setDate(firstOfMonth.getDate() - firstDayIndex)

  return Array.from({ length: totalCells }, (_, index) => {
    const date = new Date(startDate)
    date.setDate(startDate.getDate() + index)
    const isToday = date.toDateString() === new Date().toDateString()
    const event = filteredAppointments.value.find((item) => item.date.toDateString() === date.toDateString())?.service
    return {
      date,
      isCurrentMonth: date.getMonth() === month,
      isToday,
      event,
    }
  })
})

const weekDays = computed(() => {
  const date = new Date(currentDate.value)
  const startOfWeek = new Date(date)
  startOfWeek.setDate(date.getDate() - ((date.getDay() + 6) % 7))

  return Array.from({ length: 7 }, (_, index) => {
    const dayDate = new Date(startOfWeek)
    dayDate.setDate(startOfWeek.getDate() + index)
    const events = filteredAppointments.value
      .filter((item) => item.date.toDateString() === dayDate.toDateString())
      .map((item) => ({ title: item.service, time: item.time }))

    return {
      date: dayDate,
      day: dayDate.toLocaleDateString('pt-BR', { day: '2-digit', month: 'short' }),
      shortLabel: dayDate.toLocaleDateString('pt-BR', { weekday: 'short' }),
      events,
    }
  })
})

const daySlots = computed(() => {
  const sameDayAppointments = filteredAppointments.value.filter(
    (item) => item.date.toDateString() === currentDate.value.toDateString(),
  )

  if (sameDayAppointments.length) {
    return sameDayAppointments.map((item) => ({
      time: item.time,
      service: item.service,
      professional: item.professional,
      client: item.client,
    }))
  }

  return Array.from({ length: 6 }, (_, index) => ({
    time: `${9 + index}:00`,
    service: '—',
    professional: 'Disponível',
    client: 'Livre',
  }))
})

function previousPeriod() {
  const date = new Date(currentDate.value)
  if (selectedView.value === 'day') {
    date.setDate(date.getDate() - 1)
  } else if (selectedView.value === 'week') {
    date.setDate(date.getDate() - 7)
  } else {
    date.setMonth(date.getMonth() - 1)
  }
  currentDate.value = new Date(date)
}

function nextPeriod() {
  const date = new Date(currentDate.value)
  if (selectedView.value === 'day') {
    date.setDate(date.getDate() + 1)
  } else if (selectedView.value === 'week') {
    date.setDate(date.getDate() + 7)
  } else {
    date.setMonth(date.getMonth() + 1)
  }
  currentDate.value = new Date(date)
}
</script>
