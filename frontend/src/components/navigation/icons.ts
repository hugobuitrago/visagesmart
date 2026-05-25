import { h } from 'vue'

const baseIconClass = 'h-5 w-5 fill-none stroke-current stroke-2'

export const CircleIcon = () =>
  h('svg', { viewBox: '0 0 24 24', class: baseIconClass }, [
    h('path', { d: 'M12 3a9 9 0 1 0 0 18a9 9 0 0 0 0-18Z' }),
    h('path', { d: 'M12 8v4l3 3' }),
  ])

export const FolderIcon = () =>
  h('svg', { viewBox: '0 0 24 24', class: baseIconClass }, [
    h('path', { d: 'M3 7.5A2.5 2.5 0 0 1 5.5 5H10l2 2h6.5A2.5 2.5 0 0 1 21 9.5v7A2.5 2.5 0 0 1 18.5 19h-13A2.5 2.5 0 0 1 3 16.5v-9Z' }),
  ])

export const ChartIcon = () =>
  h('svg', { viewBox: '0 0 24 24', class: baseIconClass }, [
    h('path', { d: 'M5 19V9' }),
    h('path', { d: 'M12 19V5' }),
    h('path', { d: 'M19 19v-7' }),
  ])

export const CalendarIcon = () =>
  h('svg', { viewBox: '0 0 24 24', class: baseIconClass }, [
    h('path', { d: 'M7 3v4M17 3v4M4 9h16' }),
    h('rect', { x: '4', y: '5', width: '16', height: '16', rx: '2' }),
    h('path', { d: 'M8 13h4M8 17h4M14 13h2M14 17h2' }),
  ])

export const GiftIcon = () =>
  h('svg', { viewBox: '0 0 24 24', class: baseIconClass }, [
    h('path', { d: 'M20 12v8a1 1 0 0 1-1 1H5a1 1 0 0 1-1-1v-8' }),
    h('path', { d: 'M2 7h20v5H2z' }),
    h('path', { d: 'M12 7v14M12 7c-1.7 0-3-1.3-3-3 0-1.1.9-2 2-2 1.7 0 4 4 4 5M12 7c1.7 0 3-1.3 3-3 0-1.1-.9-2-2-2-1.7 0-4 4-4 5' }),
  ])

export const SunIcon = () =>
  h('svg', { viewBox: '0 0 24 24', class: baseIconClass }, [
    h('circle', { cx: '12', cy: '12', r: '4' }),
    h('path', { d: 'M12 2v2M12 20v2M4.93 4.93l1.41 1.41M17.66 17.66l1.41 1.41M2 12h2M20 12h2M4.93 19.07l1.41-1.41M17.66 6.34l1.41-1.41' }),
  ])

export const MoonIcon = () =>
  h('svg', { viewBox: '0 0 24 24', class: baseIconClass }, [
    h('path', { d: 'M21 12.79A9 9 0 0 1 11.21 3A7 7 0 1 0 21 12.79Z' }),
  ])

export const SmallDot = () =>
  h('span', { class: 'inline-block h-2 w-2 rounded-full bg-current' })
