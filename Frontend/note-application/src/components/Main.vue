<script setup>
import { ref, watch } from "vue";
import BaseInput from "./Input/BaseInput.vue";
import Navbar from "./Navbar.vue";
import BaseTextArea from "./Input/BaseTextArea.vue";
import BaseButton from "./Button/BaseButton.vue";
import CreateNote from "./Create/CreateNote.vue";
import NoteList from "./List/NoteList.vue";
import DeleteModal from "./Modal/DeleteModal.vue";
import EditNote from "./Edit/EditNote.vue";
import { onMounted } from "vue";
import { useNoteStore } from "../stores/noteStore";
import { toast } from "vue3-toastify";
import debounce from "lodash/debounce";
import ViewNote from "./Detail/ViewNote.vue";
import Pagination from "./Pagination/Pagination.vue";

const noteStore = useNoteStore();

onMounted(() => {
  noteStore.getNoteList();
});

const showCreateModal = ref(false);
const showDeleteModal = ref(false);
const showEditModal = ref(false);
const showViewModal = ref(false);
const deleteNoteID = ref(null);
const editNoteID = ref(null);
const viewNoteID = ref(null);
const searchTerm = ref("");
const total = ref(0);
const limit = ref(10);

const currentPage = ref(1);
const perPage = ref(10);

const handlePageChange = async (page) => {
  currentPage.value = page;
  await noteStore.getNoteList(
    searchTerm.value,
    (page - 1) * perPage.value,
    perPage.value
  );
};

const getDeleteNoteID = (id) => {
  showDeleteModal.value = true;
  deleteNoteID.value = id;
};

const getEditNoteID = (id) => {
  getNoteDetail(id);
  editNoteID.value = id;
  showEditModal.value = true;
};

const getViewNoteID = (id) => {
  getNoteDetail(id);
  viewNoteID.value = id;
  showViewModal.value = true;
};

const getNoteDetail = async (id) => {
  noteStore.note = null;
  await noteStore.getNoteDetail(id);
};

const handleDelete = async () => {
  const res = await noteStore.deleteNote(deleteNoteID.value);

  if (res?.success) {
    toast.success("Note deleted successfully");
  } else {
    toast.error("Failed to delete note");
  }
};

const debouncedSearch = debounce((term) => {
  noteStore.getNoteList(term);
}, 300);

watch(
  () => searchTerm.value,
  (term) => debouncedSearch(term)
);
</script>

<template>
  <div class="w-full">
    <div class="p-10">
      <div
        class="bg-white rounded-2xl shadow-lg p-10 min-h-[500px]"
        style="
          box-shadow: 0 -4px 10px rgba(0, 0, 0, 0.2),
            0 4px 6px rgba(0, 0, 0, 0.1);
        "
      >
        <div class="flex flex-wrap justify-between items-center">
          <div class="flex items-center justify-center gap-4">
            <p>Total Note: ({{ noteStore.total }})</p>

            <BaseInput
              v-if="noteStore.notes.length > 0"
              placeholder="Search..."
              v-model="searchTerm"
            />
          </div>

          <BaseButton
            v-bind:name="'Create'"
            @click="showCreateModal = true"
          ></BaseButton>
        </div>

        <p v-if="noteStore.loading" class="py-10">Loading...</p>

        <NoteList
          v-show="noteStore.notes.length"
          v-else
          @edit="getEditNoteID"
          @delete="getDeleteNoteID"
          @view="getViewNoteID"
        />

        <div v-if="noteStore.total > perPage">
          <Pagination
            v-model="currentPage"
            :total="noteStore.total"
            :per-page="perPage"
            @change="handlePageChange"
          />
        </div>

        <p
          v-if="!noteStore.notes.length"
          class="text-center text-gray-500 py-10"
        >
          No data
        </p>
      </div>
    </div>

    <CreateNote v-model:show="showCreateModal" />

    <ViewNote v-model:show="showViewModal" :key="viewNoteID" />

    <EditNote v-model:show="showEditModal" :key="editNoteID" />

    <DeleteModal v-model:show="showDeleteModal" @delete="handleDelete" />
  </div>
</template>