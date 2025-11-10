<script setup>
import { defineEmits, defineProps, ref, watch } from "vue";
import BaseModal from "../Modal/BaseModal.vue";
import { useNoteStore } from "../../stores/noteStore";
import BaseInput from "../Input/BaseInput.vue";
import BaseTextarea from "../Input/BaseTextarea.vue";
import { formatDateTime } from "../../helper/utils";

defineProps({
  isShowModal: { type: Boolean, default: false },
});

const noteStore = useNoteStore();
const isDisabled = ref(true);
const title = ref("");
const content = ref("");
const createdAt = ref("");
const updatedAt = ref("");

const resetForm = () => {
  title.value = "";
  content.value = "";
  createdAt.value = "";
  updatedAt.value = "";
};

watch(
  () => noteStore.note,
  (newNote) => {
    resetForm();
    if (newNote) {
      title.value = newNote.title;
      content.value = newNote.content;
      createdAt.value = formatDateTime(newNote.createdAt);
      updatedAt.value = formatDateTime(newNote.updatedAt);
    }
  },
  { immediate: true }
);

const emit = defineEmits(["update:show"]);

const closeModal = () => {
  emit("update:show", false);
  resetForm();
};
</script>

<template>
  <BaseModal v-bind:show="isShowModal" title="Note Details" @close="closeModal">
    <form>
      <div class="mb-4">
        <BaseInput
          v-model="title"
          label="Title"
          placeholder="Title"
          :disabled="isDisabled"
        />
      </div>

      <div class="mb-4">
        <BaseTextarea
          v-model="content"
          label="Content"
          placeholder="Content"
          :disabled="isDisabled"
        />
      </div>

      <div class="mb-4">
        <BaseInput
          v-model="createdAt"
          label="Created At"
          placeholder="Created At"
          :disabled="isDisabled"
        />
      </div>

      <div class="mb-4">
        <BaseInput
          v-model="updatedAt"
          label="Updated At"
          placeholder="Updated At"
          :disabled="isDisabled"
        />
      </div>
    </form>
  </BaseModal>
</template>
