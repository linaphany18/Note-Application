<script setup>
import { defineProps } from "vue";

const props = defineProps({
  name: {
    type: String,
    default: "Button"
  },
  disable: {
    type: Boolean,
    default: false
  }
});

const handleSubmit = () => {};
</script>

<template>
  <button
    v-if="name === 'Cancel' || name === 'No'"
    @click="handleSubmit"
    class="bg-slate-500 hover:bg-slate-600 text-white px-4 py-2 rounded-lg min-w-24"
  >
    {{ name }}
  </button>

  <button
    v-else
    @click="handleSubmit"
    v-bind:disabled="props.disable"
    v-bind:class="['bg-cyan-600 hover:bg-cyan-700 text-white px-4 py-2 rounded-lg min-w-24', { 'opacity-50 cursor-not-allowed': props.disable }]"
  >
    {{ name }}
  </button>
</template>
