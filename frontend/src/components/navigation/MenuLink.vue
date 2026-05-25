<template>
  <RouterLink
    :to="to"
    class="group flex items-center gap-3 rounded-lg px-3 py-3 text-sm font-medium text-white transition hover:bg-white/20"
    active-class="border border-white/30 bg-white/30 text-white shadow-sm"
  >
    <span class="inline-flex h-5 w-5 items-center justify-center">
      <slot name="icon" />
    </span>
    <span v-if="!collapsed">{{ label }}</span>
  </RouterLink>
</template>

<script setup lang="ts">
defineProps<{
  to: string
  label: string
  collapsed: boolean
}>()
</script>
