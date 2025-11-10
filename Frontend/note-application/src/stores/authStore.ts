import { defineStore } from "pinia";
import api from "../helper/api";

interface ApiResponse<T> {
  success: boolean;
  message: string;
  data: T;
  token?: string | null;
}

interface User {
  id?: number;
  username?: string;
  role?: string;
}

interface UserState {
  data: User | null;
  token: string | null;
  loading: boolean;
  error: string | null;
}

export const useAuthStore = defineStore("auth", {
  state: (): UserState => ({
    token: localStorage.getItem("token") || "",
    data: null,
    loading: false,
    error: null,
  }),
  getters: {
    isAuthenticated: (state) => !!state.token,
  },
  actions: {
    async login(username: string, password: string) {
      try {
        const res = await api.post<ApiResponse<User>>("/api/v1/login", {
          username,
          password,
        });

        const token = res.data.token || null;
        this.token = token;
        this.data = res.data.data;

        if (this.token) {
          localStorage.setItem("token", this.token);
        }

        return res;
      } catch (err: any) {
        return err?.response;
      }
    },

    logout() {
      this.token = "";
      this.data = null;
      localStorage.removeItem("token");
    },
  },
});
