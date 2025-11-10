<script setup>
import router from '../router';
import { useAuthStore } from '../stores/authStore';

const user = useAuthStore();

const handleLogout = () => {
    user.logout();
    router.push("/login");
}

</script>

<template>
  <div>
    <div class="flex justify-between p-4 bg-cyan-600">
      <p class="text-white font-bold text-xl">Note Application</p>

      <p v-show="user.token" class="text-white cursor-pointer" @click="handleLogout">Logout</p>
    </div>
  </div>
</template>
