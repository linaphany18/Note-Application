import { defineStore } from "pinia";
import api from "../helper/api";

interface ApiResponse<T> {
  success: boolean;
  message: string;
  data: T;
}

interface NoteList<T> {
  data: T;
  total: number;
  page: number;
  limit: number;
}

interface Note {
  id: number;
  title: string;
  content: string;
  createdAt: string;
  updatedAt: string;
}

interface NoteState {
  notes: Note[];
  note: Note | null;
  loading: boolean;
  error: string | null;
  total: number;
}

export const useNoteStore = defineStore("noteStore", {
  state: (): NoteState => ({
    notes: [],
    note: null,
    loading: false,
    error: null,
    total: 0,
  }),
  actions: {
    async getNoteList(
      search: string = "",
      offset: number = 0,
      limit: number = 10
    ) {
      this.loading = true;
      this.error = null;

      try {
        const res = await api.get<ApiResponse<NoteList<Note[]>>>("api/v1/note/notes", {
          params: { search, offset, limit },
        });

        if (res.data.success) {
          this.notes = res.data.data.data;
          this.total = res.data.data.total;
        }
      } catch (err: any) {
        this.error = err.message || "Failed to get notes";
      } finally {
        this.loading = false;
      }
    },

    async createNote(note: Note) {
      try {
        this.loading = true;

        const res = await api.post<ApiResponse<Note>>("api/v1/note/create", note);

        if (res.data.success) {
          await this.getNoteList();
          return res.data;
        }
      } catch (err: any) {
        this.error = err.message || "Failed to create note";
      } finally {
        this.loading = false;
      }
    },

    async deleteNote(id: number) {
      try {
        this.loading = true;

        const res = await api.delete<ApiResponse<null>>(`api/v1/note/delete/${id}`);
        if (res.data.success) {
          await this.getNoteList();
          return res.data;
        }
      } catch (err: any) {
        this.error = err.message || "Failed to delete note";
      } finally {
        this.loading = false;
      }
    },

    async getNoteDetail(id: number) {
      try {
        this.loading = true;

        const res = await api.get<ApiResponse<Note>>(`api/v1/note/${id}`);

        if (res.data.success) {
          this.note = res.data.data;
        }
      } catch (err: any) {
        this.error = err.message || "Failed to get note detail";
      } finally {
        this.loading = false;
      }
    },

    async editNote(id: number, note: Note) {
      try {
        this.loading = true;

        const res = await api.put<ApiResponse<Note>>(`api/v1/note/edit/${id}`, note);

        if (res.data.success) {
          await this.getNoteList();
          return res.data;
        }
      } catch (err: any) {
        this.error = err.message || "Failed to update note";
      } finally {
        this.loading = false;
      }
    },
  },
});
