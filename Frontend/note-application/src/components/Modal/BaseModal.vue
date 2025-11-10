<script setup>
defineProps({
  show: Boolean,
  title: {
    type: String,
    default: 'Modal Title'
  }
})

const emit = defineEmits(['close'])

</script>

<template>
  <div 
    v-if="show"
    class="fixed inset-0 flex items-center justify-center bg-black bg-opacity-50 z-50"
  >
    <div class="bg-white rounded-2xl shadow-lg p-6 w-96 relative">
      <button 
        @click="emit('close')"
        class="absolute top-2 right-2 text-gray-500 hover:text-gray-800"
      >
        ✕
      </button>
      
      <!-- Modal Title -->
      <h2 class="text-xl font-semibold mb-4">{{ title }}</h2>
      
      <!-- Slot for content -->
      <div>
        <slot></slot>
      </div>
    </div>
  </div>
</template>