<script setup>
import { onMounted, ref, watch } from "vue";
import BaseModal from "../Modal/BaseModal.vue";
import BaseInput from "../Input/BaseInput.vue";
import BaseTextArea from "../Input/BaseTextArea.vue";
import BaseButton from "../Button/BaseButton.vue";
import { useNoteStore } from "../../stores/noteStore";
import { toast } from "vue3-toastify";

const noteStore = useNoteStore();

const props = defineProps({
  isShowModal: { type: Boolean, default: true },
});

const title = ref("");
const content = ref("");
const isSubmitted = ref(false);
const isDisabled = ref(false);

const resetForm = () => {
  title.value = "";
  content.value = "";
  isDisabled.value = false;
  isSubmitted.value = false;
};

watch(
  () => noteStore.note,
  (newNote) => {
    resetForm();
    if (newNote) {
      title.value = newNote.title;
      content.value = newNote.content;
    }
  },
  { immediate: true }
);

const emit = defineEmits(["update:show"]);

const handleSubmit = async (e) => {
  e.preventDefault();

  isSubmitted.value = true;
  isDisabled.value = true;

  if (title.value.trim() !== "") {
    isSubmitted.value = false;

    const res = await noteStore.editNote(noteStore.note.id, {
      title: title.value,
      content: content.value,
    });

    if (res?.success) {
      toast.success("Note updated successfully!");
      resetForm();
      emit("update:show", false);
      isDisabled.value = false;
    }
  } else {
    isDisabled.value = false;
  }
};

const handleCancel = (e) => {
  e.preventDefault();
  emit("update:show", false);
  resetForm();
};
</script>

<template>
  <BaseModal
    v-bind:show="props.isShowModal"
    title="Edit Note"
    @close="emit('update:show', false)"
  >
    <form>
      <div class="mb-4">
        <BaseInput
          v-model="title"
          label="Title"
          placeholder="Title"
          v-bind:required="true"
          v-bind:isSubmitted="isSubmitted"
        />
      </div>

      <div class="mb-4">
        <BaseTextArea
          v-model="content"
          label="Content"
          placeholder="Content"
          v-bind:required="false"
          v-bind:isSubmitted="isSubmitted"
        />
      </div>
      <div class="flex flex-wrap justify-center gap-5 items-center my-5">
        <BaseButton :name="'Save'" @click="handleSubmit"></BaseButton>

        <BaseButton :name="'Cancel'" @click="handleCancel"></BaseButton>
      </div>
    </form>
  </BaseModal>
</template>
