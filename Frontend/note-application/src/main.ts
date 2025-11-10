import { createApp } from 'vue'
import './style.css'
import App from './App.vue'
import { createPinia } from 'pinia'
import Vue3Toastify, { type ToastContainerOptions } from 'vue3-toastify';
import 'vue3-toastify/dist/index.css';
import { VueAwesomePaginate } from 'vue-awesome-paginate'
import 'vue-awesome-paginate/dist/style.css'
import router from './router';

const app = createApp(App);

app.use(createPinia());
app.use(Vue3Toastify, {
  autoClose: 2000,
  position: 'bottom-right',
} as ToastContainerOptions);
app.component('VueAwesomePaginate', VueAwesomePaginate);
app.use(router)
app.mount('#app');