<script setup>
import { defineProps, defineEmits } from "vue";
import BaseModal from "./BaseModal.vue";
import BaseButton from "../Button/BaseButton.vue";

const props = defineProps({
  isShowModal: { type: Boolean, default: true },
});

const emit = defineEmits(["update:show", "delete"]);

const handleDelete = (e) => {
  e.stopPropagation();
  console.log("Note id in btn delete", e.target.id);

  emit("delete", e.target.id);
  emit("update:show", false);
};

</script>

<template>
  <BaseModal
    v-bind:show="isShowModal"
    title="Delete Note"
    @close="emit('update:show', false)"
  >
    <form>
        <p class="!p-5">Are you sure you want to delete ?</p>

      <div class="flex flex-wrap justify-center gap-5 items-center my-5">
        <BaseButton v-bind:name="'Yes'" @click="handleDelete($event)"></BaseButton>
        <BaseButton v-bind:name="'No'" @click="emit('update:show', false)"></BaseButton>
      </div>
    </form>
  </BaseModal>
</template>
