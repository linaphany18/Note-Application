<script setup>
import { useNoteStore } from "../../stores/noteStore";
import { formatDateTime } from "../../helper/utils";

const noteStore = useNoteStore();

const emit = defineEmits(
  { edit: (note) => {}, delete: (note) => {} },
  { view: (note) => {} }
);

const handleView = (e, id) => {
  e.stopPropagation();
  emit("view", id);
};

const handleEdit = (e, id) => {
  e.stopPropagation();
  emit("edit", id);
};

const handleDelete = (e, id) => {
  e.stopPropagation();
  emit("delete", id);
};

const headers = ["No.", "Title", "Content", "Created At", "Action"];
</script>

<template>
  <div class="py-4">
    <div class="overflow-x-auto">
      <table class="min-w-full border border-gray-300 rounded-lg">
        <thead class="bg-gray-200">
          <tr>
            <th
              class="border px-4 py-2"
              v-for="(header, index) in headers"
              :key="index"
            >
              {{ header }}
            </th>
          </tr>
        </thead>
        <tbody>
          <tr
            v-for="(note, index) in noteStore.notes"
            :key="note.id"
            :class="index % 2 === 0 ? 'bg-white' : 'bg-gray-100'"
            class="cursor-pointer"
            @click="handleView($event, note.id)"
          >
            <td class="border px-4 py-2">{{ index + 1 }}</td>
            <td class="border px-4 py-2 text-left">
              <div class="w-60">
                <div class="truncate">{{ note.title }}</div>
              </div>
            </td>
            <td class="border px-4 py-2 text-left">
              <div class="w-60">
                <div class="truncate">{{ note.content }}</div>
              </div>
            </td>
            <td class="border px-4 py-2 text-left">
              {{ formatDateTime(note.createdAt) }}
            </td>
            <td class="border px-4 py-2">
              <div class="flex justify-center gap-2">
                <p
                  class="cursor-pointer text-blue-500 hover:text-blue-800"
                  @click="handleEdit($event, note.id)"
                >
                  Edit
                </p>
                |
                <p
                  class="cursor-pointer text-red-500 hover:text-red-800"
                  @click="handleDelete($event, note.id)"
                >
                  Delete
                </p>
              </div>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>
