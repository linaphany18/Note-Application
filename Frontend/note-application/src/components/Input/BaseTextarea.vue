<script setup>
import { ref, computed, defineEmits, defineProps, watch } from "vue";

const props = defineProps({
  modelValue: String,
  label: String,
  placeholder: String,
  type: { type: String, default: "text" },
  required: { type: Boolean, default: false },
  disabled: { type: Boolean, default: false },
  isSubmit: { type: Boolean, default: false },
});

const innerValue = ref("");
const emit = defineEmits(["update:modelValue"]);

watch(
  () => props.modelValue,
  (newValue) => {
    innerValue.value = newValue;
  }
);

watch(innerValue, (newValue) => {
  emit("update:modelValue", newValue);
});

const error = computed(() => {
  if (props.required && props.isSubmitted && !innerValue.value) {
    return `${props.label || "This field"} is required`;
  }

  return "";
});
</script>

<template>
  <div>
    <label
      v-if="label"
      class="block text-left text-sm font-medium mb-1 text-gray-700"
    >
      <div class="flex items-center space-x-1">
        <div>{{ label }}</div>
        <p class="text-red-500" v-if="required">*</p>
      </div>
    </label>

    <textarea
      type="type"
      v-model="innerValue"
      v-bind:placeholder="placeholder"
      class="w-full border rounded-lg p-2 focus:outline-none focus:ring-2"
      v-bind:class="[
        (error
          ? 'border-red-500 focus:ring-red-300'
          : 'border-gray-300 focus:ring-blue-300') +
          (props.disabled ? 'border-0' : ''),
      ]"
      v-bind:disabled="disabled"
    />

    <p v-if="error" class="text-left text-red-500 text-sm mt-1">{{ error }}</p>
  </div>
</template>
