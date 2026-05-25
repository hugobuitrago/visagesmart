import { createRouter, createWebHistory } from 'vue-router'
import DashboardView from '@/views/DashboardView.vue'
import ProfessionalDetailView from '@/views/ProfessionalDetailView.vue'
import ProfessionalsView from '@/views/ProfessionalsView.vue'
import ProductsView from '@/views/ProductsView.vue'
import ServicesView from '@/views/ServicesView.vue'
import CustomersView from '@/views/CustomersView.vue'
import CustomerDetailView from '@/views/CustomerDetailView.vue'
import AgendaView from '@/views/AgendaView.vue'
import LoyaltyProgramView from '@/views/LoyaltyProgramView.vue'

const router = createRouter({
  history: createWebHistory(),
  routes: [
    {
      path: '/',
      name: 'dashboard',
      component: DashboardView,
    },
    {
      path: '/services',
      name: 'services',
      component: ServicesView,
    },
    {
      path: '/products',
      name: 'products',
      component: ProductsView,
    },
    {
      path: '/professionals',
      name: 'professionals',
      component: ProfessionalsView,
    },
    {
      path: '/professionals/:id',
      name: 'professional-detail',
      component: ProfessionalDetailView,
    },
    {
      path: '/customers',
      name: 'customers',
      component: CustomersView,
    },
    {
      path: '/customers/:id',
      name: 'customer-detail',
      component: CustomerDetailView,
    },
    {
      path: '/agenda',
      name: 'agenda',
      component: AgendaView,
    },
    {
      path: '/fidelidade',
      name: 'loyalty-program',
      component: LoyaltyProgramView,
    },
  ],
})

export default router
