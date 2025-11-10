<script setup>
import { ref } from "vue";
import BaseModal from "../Modal/BaseModal.vue";
import BaseInput from "../Input/BaseInput.vue";
import BaseTextArea from "../Input/BaseTextArea.vue";
import BaseButton from "../Button/BaseButton.vue";
import { toast } from "vue3-toastify";
import { useNoteStore } from "../../stores/noteStore";

const noteStore = useNoteStore();

defineProps({
  isShowModal: { type: Boolean, default: true },
});

const isSubmitted = ref(false);
const isDisabled = ref(false);
const title = ref("");
const content = ref("");
const emit = defineEmits(["update:show"]);

const handleSubmit = async (e) => {
  e.preventDefault();

  isSubmitted.value = true;
  isDisabled.value = true;

  if (title.value.trim() !== "") {
    isSubmitted.value = false;

    const res = await noteStore.createNote({
      title: title.value,
      content: content.value,
    });

    if (res?.success) {
      toast.success("Note created successfully!");
      resetForm();
      emit("update:show", false);
      isDisabled.value = false;
    }
  } else {
    isDisabled.value = false;
  }
};

const resetForm = () => {
  title.value = "";
  content.value = "";
  isDisabled.value = false;
  isSubmitted.value = false;
};

const closeModal = () => {
  emit("update:show", false);
  resetForm();
};
</script>

<template>
  <BaseModal v-bind:show="isShowModal" title="Create Note" @close="closeModal">
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

      <BaseButton
        v-bind:name="'Save'"
        @click="handleSubmit"
        v-bind:disable="isDisabled"
      ></BaseButton>
    </form>
  </BaseModal>
</template>
