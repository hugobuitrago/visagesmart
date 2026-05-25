import { computed, ref, watch } from 'vue'
import { defineStore } from 'pinia'

type ThemeMode = 'light' | 'dark'

export const useThemeStore = defineStore('theme', () => {
  const mode = ref<ThemeMode>('light')
  const isDark = computed(() => mode.value === 'dark')

  function applyTheme(value: ThemeMode) {
    mode.value = value
    document.documentElement.classList.toggle('dark', value === 'dark')
    localStorage.setItem('visagesmart-theme', value)
  }

  function toggleTheme() {
    applyTheme(isDark.value ? 'light' : 'dark')
  }

  function initializeTheme() {
    const saved = localStorage.getItem('visagesmart-theme') as ThemeMode | null
    const prefersDark = window.matchMedia('(prefers-color-scheme: dark)').matches
    applyTheme(saved ?? (prefersDark ? 'dark' : 'light'))
  }

  watch(mode, (value) => {
    document.documentElement.dataset.theme = value
  }, { immediate: true })

  return { mode, isDark, initializeTheme, toggleTheme }
})
