<script setup>
import { ref, computed, watch } from "vue";

const props = defineProps({
  total: { type: Number, required: true },
  perPage: { type: Number, default: 10 },
  modelValue: { type: Number, default: 1 }
});

const emit = defineEmits(["update:modelValue", "change"]);

const currentPage = ref(props.modelValue);

const totalPages = computed(() => Math.ceil(props.total / props.perPage));

function goToPage(page) {
  if (page < 1 || page > totalPages.value) return;
  currentPage.value = page;
  emit("update:modelValue", page);
  emit("change", page);
}

// Watch for external modelValue changes from parent
watch(() => props.modelValue, (val) => {
  currentPage.value = val;
});
</script>

<template>
  <div v-if="totalPages > 1" class="flex justify-center space-x-2 mt-4">
    <button
      :disabled="currentPage === 1"
      @click="goToPage(currentPage - 1)"
      class="px-2 py-1 border rounded"
    >
      Prev
    </button>

    <button
      v-for="page in totalPages"
      :key="page"
      :class="['px-2 py-1 border rounded', { 'font-bold bg-cyan-600 text-white': page === currentPage }]"
      @click="goToPage(page)"
    >
      {{ page }}
    </button>

    <button
      :disabled="currentPage === totalPages"
      @click="goToPage(currentPage + 1)"
      class="px-2 py-1 border rounded"
    >
      Next
    </button>
  </div>
</template>