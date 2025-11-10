<script setup>
import { ref } from "vue";
import BaseInput from "./Input/BaseInput.vue";
import BaseButton from "./Button/BaseButton.vue";
import { useAuthStore } from "../stores/authStore";
import router from "../router";

const user = useAuthStore();

const username = ref("");
const password = ref("");
const isDisabled = ref(false);
const isSubmitted = ref(false);
const error = ref("");

const handleSubmit = async (e) => {
  e.preventDefault();

  isSubmitted.value = true;
  isDisabled.value = true;

  if (username.value.trim() !== "" && password.value.trim() !== "") {
    isSubmitted.value = false;

    const res = await user.login(username.value, password.value);

    if (res.data.success) {
      resetForm();
      isDisabled.value = false;
      isSubmitted.value = false;

      router.push({ name: "Main" });
    }

    error.value = res.data.message;
    isDisabled.value = false;
  } else {
    isDisabled.value = false;
  }
};

const resetForm = () => {
  username.value = "";
  password.value = "";
};
</script>

<template>
  <div class="p-10">
    <div class="flex justify-center">
      <div
        class="bg-white rounded-2xl shadow-lg p-10 w-full max-w-sm sm:max-w-md"
        style="
          box-shadow: 0 -4px 10px rgba(0, 0, 0, 0.2),
            0 4px 6px rgba(0, 0, 0, 0.1);
        "
      >
        <h2 class="text-xl font-semibold mb-4">Login</h2>

        <div>
          <form>
            <div class="mb-4">
              <BaseInput
                v-model="username"
                label="Username"
                placeholder="Username"
                v-bind:required="true"
                v-bind:isSubmitted="isSubmitted"
              />
            </div>

            <div class="mb-4">
              <BaseInput
                v-model="password"
                type="password"
                label="Password"
                placeholder="Password"
                v-bind:required="true"
                v-bind:isSubmitted="isSubmitted"
              />
            </div>

            <BaseButton
              v-bind:name="'Login'"
              @click="handleSubmit"
              v-bind:disable="isDisabled"
            ></BaseButton>

            <p v-if="error" class="text-left text-red-500 text-sm mt-1">
              {{ error }}
            </p>
          </form>
        </div>
      </div>
    </div>
  </div>
</template>
