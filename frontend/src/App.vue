<template>
  <div class="app-container">
    <NavBar v-if="showNavbar" @logout="handleLogout" :userRole="userRole" />
    <router-view />
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue';
import { useRouter, useRoute } from 'vue-router';
import NavBar from '@/components/NavBar.vue';

const router = useRouter();
const route = useRoute();
const userRole = ref(null);

// Verificar autenticación al cargar
onMounted(() => {
  checkAuth();
});

const checkAuth = () => {
  const userData = localStorage.getItem('userData');
  userRole.value = userData ? JSON.parse(userData).tipo_usuario : null;
};

// Computed para mostrar navbar
const showNavbar = computed(() => {
  return route.path !== '/' && userRole.value;
});

const handleLogout = () => {
  localStorage.removeItem('authToken');
  localStorage.removeItem('userData');
  localStorage.removeItem('userRole');
  userRole.value = null;
  router.push('/');
};

// Observar cambios de ruta
router.afterEach(() => {
  checkAuth();
});
</script>

<style>
@import '@/assets/css/App.css';
</style>