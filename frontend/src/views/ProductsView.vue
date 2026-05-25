<template>
  <MainLayout>
    <section class="space-y-4">
      <article class="rounded-xl border border-black/5 bg-white/80 p-5 shadow-sm backdrop-blur dark:border-white/10 dark:bg-[#212121]/90">
        <div class="flex flex-col gap-4 lg:flex-row lg:items-center lg:justify-between">
          <div>
            <p class="text-sm font-semibold text-zinc-900 dark:text-white">Produtos</p>
            <p class="mt-1 text-sm text-zinc-500 dark:text-zinc-400">
              Gerencie o inventário de produtos com estoque, preços de compra e venda.
            </p>
          </div>

          <div class="flex flex-col gap-3 sm:flex-row">
            <input
              v-model="search"
              type="search"
              placeholder="Buscar por nome"
              class="min-w-0 rounded-lg border border-black/10 bg-white px-3 py-2 text-sm text-zinc-800 outline-none transition focus:border-[color:var(--color-brand)] dark:border-white/10 dark:bg-[#1b1b1b] dark:text-white"
              @keydown.enter="applySearch"
            />
            <button
              class="rounded-lg bg-[color:var(--color-brand)] px-4 py-2 text-sm font-medium text-white transition hover:opacity-90"
              @click="startCreate"
            >
              Novo produto
            </button>
          </div>
        </div>

        <div class="mt-5 overflow-hidden rounded-xl border border-black/5 dark:border-white/10">
          <table class="min-w-full divide-y divide-black/5 text-sm dark:divide-white/10">
            <thead class="bg-zinc-50/90 dark:bg-white/5">
              <tr class="text-left text-zinc-500 dark:text-zinc-300">
                <th class="px-4 py-3 font-medium">Nome</th>
                <th class="px-4 py-3 font-medium text-right">Estoque</th>
                <th class="px-4 py-3 font-medium text-right">Preço de Compra</th>
                <th class="px-4 py-3 font-medium text-right">Preço de Venda</th>
                <th class="px-4 py-3 font-medium text-right">Ações</th>
              </tr>
            </thead>
            <tbody class="divide-y divide-black/5 bg-white/80 dark:divide-white/10 dark:bg-transparent">
              <tr v-if="isLoading">
                <td colspan="5" class="px-4 py-8 text-center text-zinc-500 dark:text-zinc-400">
                  Carregando produtos...
                </td>
              </tr>
              <tr v-else-if="errorMessage">
                <td colspan="5" class="px-4 py-8 text-center text-rose-600 dark:text-rose-300">
                  {{ errorMessage }}
                </td>
              </tr>
              <tr v-else-if="filteredProducts.length === 0">
                <td colspan="5" class="px-4 py-8 text-center text-zinc-500 dark:text-zinc-400">
                  Nenhum produto encontrado.
                </td>
              </tr>
              <tr v-for="product in filteredProducts" :key="product.id" class="bg-white/40 transition hover:bg-zinc-100/40 dark:bg-white/5 dark:hover:bg-white/10">
                <td class="px-4 py-4">
                  <p class="font-semibold text-zinc-900 dark:text-white">{{ product.name }}</p>
                </td>
                <td class="px-4 py-4 text-right">
                  <p class="font-medium text-zinc-900 dark:text-white">{{ product.quantityInStock }}</p>
                </td>
                <td class="px-4 py-4 text-right">
                  <p class="font-medium text-zinc-900 dark:text-white">R$ {{ formatPrice(product.purchasePrice) }}</p>
                </td>
                <td class="px-4 py-4 text-right">
                  <p class="font-medium text-zinc-900 dark:text-white">R$ {{ formatPrice(product.sellingPrice) }}</p>
                </td>
                <td class="px-4 py-4 text-right">
                  <div class="flex justify-end gap-2">
                    <button
                      class="rounded-lg border border-blue-200 bg-gradient-to-r from-blue-50 to-blue-50/50 px-3 py-2 text-xs font-medium text-blue-700 transition hover:from-blue-100 dark:border-blue-900/30 dark:from-blue-950/30 dark:to-blue-950/20 dark:text-blue-300"
                      @click="editProduct(product)"
                    >
                      Editar
                    </button>
                    <button
                      class="rounded-lg border border-rose-200 bg-gradient-to-r from-rose-50 to-rose-50/50 px-3 py-2 text-xs font-medium text-rose-600 transition hover:from-rose-100 dark:border-rose-900/30 dark:from-rose-950/30 dark:to-rose-950/20 dark:text-rose-300"
                      @click="deleteProduct(product.id)"
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

      <!-- Modal de Criação/Edição -->
      <article v-if="showModal" class="rounded-xl border border-black/5 bg-white/80 p-5 shadow-sm backdrop-blur dark:border-white/10 dark:bg-[#212121]/90">
        <div class="flex items-center justify-between mb-4">
          <h2 class="text-lg font-semibold text-zinc-900 dark:text-white">
            {{ isEditing ? 'Editar Produto' : 'Novo Produto' }}
          </h2>
          <button
            class="text-zinc-500 transition hover:text-zinc-900 dark:hover:text-white"
            @click="closeModal"
          >
            ✕
          </button>
        </div>

        <div class="space-y-4">
          <label class="space-y-2">
            <span class="text-sm font-medium text-zinc-700 dark:text-zinc-200">Nome do Produto *</span>
            <input
              v-model="form.name"
              type="text"
              class="w-full rounded-lg border border-black/10 bg-white px-3 py-2 text-sm text-zinc-800 outline-none transition focus:border-[color:var(--color-brand)] dark:border-white/10 dark:bg-[#1b1b1b] dark:text-white"
              placeholder="Nome do produto"
            />
          </label>

          <label class="space-y-2">
            <span class="text-sm font-medium text-zinc-700 dark:text-zinc-200">Quantidade em Estoque *</span>
            <input
              v-model.number="form.quantityInStock"
              type="number"
              min="0"
              class="w-full rounded-lg border border-black/10 bg-white px-3 py-2 text-sm text-zinc-800 outline-none transition focus:border-[color:var(--color-brand)] dark:border-white/10 dark:bg-[#1b1b1b] dark:text-white"
              placeholder="0"
            />
          </label>

          <div class="grid gap-4 md:grid-cols-2">
            <label class="space-y-2">
              <span class="text-sm font-medium text-zinc-700 dark:text-zinc-200">Preço de Compra *</span>
              <input
                v-model.number="form.purchasePrice"
                type="number"
                min="0"
                step="0.01"
                class="w-full rounded-lg border border-black/10 bg-white px-3 py-2 text-sm text-zinc-800 outline-none transition focus:border-[color:var(--color-brand)] dark:border-white/10 dark:bg-[#1b1b1b] dark:text-white"
                placeholder="0.00"
              />
            </label>

            <label class="space-y-2">
              <span class="text-sm font-medium text-zinc-700 dark:text-zinc-200">Preço de Venda *</span>
              <input
                v-model.number="form.sellingPrice"
                type="number"
                min="0"
                step="0.01"
                class="w-full rounded-lg border border-black/10 bg-white px-3 py-2 text-sm text-zinc-800 outline-none transition focus:border-[color:var(--color-brand)] dark:border-white/10 dark:bg-[#1b1b1b] dark:text-white"
                placeholder="0.00"
              />
            </label>
          </div>

          <div v-if="formError" class="rounded-lg border border-rose-200 bg-rose-50 px-3 py-3 text-sm text-rose-700 dark:border-rose-400/30 dark:bg-rose-500/10 dark:text-rose-200">
            {{ formError }}
          </div>

          <div class="flex justify-end gap-3">
            <button
              class="rounded-lg border border-black/10 px-4 py-2 text-sm font-medium text-zinc-700 transition hover:border-[color:var(--color-brand)] hover:text-[color:var(--color-brand)] dark:border-white/10 dark:text-zinc-200"
              @click="closeModal"
              :disabled="isSaving"
            >
              Cancelar
            </button>
            <button
              class="rounded-lg bg-[color:var(--color-brand)] px-4 py-2 text-sm font-medium text-white transition hover:opacity-90 disabled:opacity-60"
              @click="saveProduct"
              :disabled="isSaving"
            >
              {{ isSaving ? 'Salvando...' : 'Salvar' }}
            </button>
          </div>
        </div>
      </article>
    </section>
  </MainLayout>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import MainLayout from '@/layouts/MainLayout.vue'

interface Product {
  id: string
  name: string
  quantityInStock: number
  purchasePrice: number
  sellingPrice: number
}

interface ProductForm {
  name: string
  quantityInStock: number
  purchasePrice: number
  sellingPrice: number
}

const products = ref<Product[]>([])
const search = ref('')
const isLoading = ref(false)
const errorMessage = ref('')
const showModal = ref(false)
const isEditing = ref(false)
const isSaving = ref(false)
const formError = ref('')
const editingProductId = ref<string | null>(null)

const form = ref<ProductForm>({
  name: '',
  quantityInStock: 0,
  purchasePrice: 0,
  sellingPrice: 0,
})

const filteredProducts = computed(() => {
  if (!search.value) return products.value
  const query = search.value.toLowerCase()
  return products.value.filter(p => p.name.toLowerCase().includes(query))
})

const formatPrice = (price: number) => {
  return price.toFixed(2).replace('.', ',')
}

const loadProducts = async () => {
  isLoading.value = true
  errorMessage.value = ''
  try {
    const response = await fetch('/api/products')
    if (!response.ok) throw new Error('Erro ao carregar produtos')
    products.value = await response.json()
  } catch (error) {
    errorMessage.value = 'Erro ao carregar produtos'
    console.error(error)
  } finally {
    isLoading.value = false
  }
}

const applySearch = () => {
  // A busca é feita via computed property filteredProducts
}

const startCreate = () => {
  isEditing.value = false
  editingProductId.value = null
  form.value = {
    name: '',
    quantityInStock: 0,
    purchasePrice: 0,
    sellingPrice: 0,
  }
  formError.value = ''
  showModal.value = true
}

const editProduct = (product: Product) => {
  isEditing.value = true
  editingProductId.value = product.id
  form.value = {
    name: product.name,
    quantityInStock: product.quantityInStock,
    purchasePrice: product.purchasePrice,
    sellingPrice: product.sellingPrice,
  }
  formError.value = ''
  showModal.value = true
}

const closeModal = () => {
  showModal.value = false
  formError.value = ''
}

const validateForm = (): boolean => {
  if (!form.value.name.trim()) {
    formError.value = 'Nome do produto é obrigatório'
    return false
  }
  if (form.value.quantityInStock < 0) {
    formError.value = 'Quantidade não pode ser negativa'
    return false
  }
  if (form.value.purchasePrice < 0) {
    formError.value = 'Preço de compra não pode ser negativo'
    return false
  }
  if (form.value.sellingPrice < 0) {
    formError.value = 'Preço de venda não pode ser negativo'
    return false
  }
  return true
}

const saveProduct = async () => {
  if (!validateForm()) return

  isSaving.value = true
  formError.value = ''
  try {
    const method = isEditing.value ? 'PUT' : 'POST'
    const url = isEditing.value ? `/api/products/${editingProductId.value}` : '/api/products'

    const response = await fetch(url, {
      method,
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(form.value),
    })

    if (!response.ok) throw new Error('Erro ao salvar produto')

    await loadProducts()
    closeModal()
  } catch (error) {
    formError.value = 'Erro ao salvar produto'
    console.error(error)
  } finally {
    isSaving.value = false
  }
}

const deleteProduct = async (id: string) => {
  if (!confirm('Tem certeza que deseja excluir este produto?')) return

  try {
    const response = await fetch(`/api/products/${id}`, { method: 'DELETE' })
    if (!response.ok) throw new Error('Erro ao excluir produto')
    await loadProducts()
  } catch (error) {
    errorMessage.value = 'Erro ao excluir produto'
    console.error(error)
  }
}

onMounted(() => {
  loadProducts()
})
</script>
