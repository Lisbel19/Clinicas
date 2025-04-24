<template>
    <div class="lobby-container" :class="userRole">
      <!-- Header con efecto parallax -->
      <div class="parallax-header">
        <div class="overlay"></div>
        <div class="header-content">
          <h1 class="welcome-title">¡Bienvenido!</h1>
          <p class="welcome-subtitle">{{ welcomeMessage }}</p>
          <div class="user-badge">
            <i :class="roleIcon"></i>
            <span>{{ roleLabel }}</span>
          </div>
        </div>
      </div>
  
      <!-- Contenido principal -->
      <div class="main-content container">
        <!-- Acciones rápidas -->
        <div class="quick-actions">
          <h2 class="section-title">Acciones rápidas</h2>
          <div class="actions-grid" :class="{'few-items': quickActions.length <= 2}">
            <router-link 
              v-for="action in quickActions" 
              :key="action.path" 
              :to="action.path"
              class="action-card"
              :class="action.color"
            >
              <i :class="action.icon"></i>
              <h3>{{ action.title }}</h3>
              <p>{{ action.description }}</p>
            </router-link>
          </div>
        </div>
  
        <!-- Estadísticas -->
        <div class="stats-section">
          <h2 class="section-title">Tus estadísticas</h2>
          <div class="stats-grid">
            <div class="stat-card" v-for="stat in userStats" :key="stat.title">
              <div class="stat-icon" :class="stat.color">
                <i :class="stat.icon"></i>
              </div>
              <div class="stat-info">
                <h3>{{ stat.value }}</h3>
                <p>{{ stat.title }}</p>
              </div>
            </div>
          </div>
        </div>
  
        <!-- Mensaje motivacional -->
        <div class="motivational-card">
          <i class="fas fa-quote-left"></i>
          <p class="motivational-text">{{ motivationalMessage }}</p>
          <i class="fas fa-quote-right"></i>
        </div>
      </div>
  
      <!-- Footer -->
      <footer class="lobby-footer">
        <p>MG Clinic Management - {{ currentYear }} | Sistema v1.0.0</p>
      </footer>
    </div>
  </template>
  
  <script setup>
import { ref, computed, onMounted } from 'vue';
import { useRouter } from 'vue-router';
import { getCitasByPacienteEmail } from '@/services/MisCitaService';
import { getCitasByMedicoEmail } from '@/services/CitaMedicoService';

const router = useRouter();

// Obtener datos del usuario desde localStorage
const userData = ref({
  role: localStorage.getItem('userRole') || 'paciente',
  email: JSON.parse(localStorage.getItem('userData'))?.correo || ''
});

// Estado para las citas del paciente y medicos
const pacienteCitas = ref([]);
const medicoCitas = ref([]);

// Computed property para la próxima cita
const proximaCita = computed(() => {
  if (userData.value.role !== 'paciente' || !pacienteCitas.value.length) {
    return null;
  }

  const ahora = new Date();
  const citasFuturas = pacienteCitas.value.filter(cita => new Date(cita.fecha) > ahora);
  
  if (!citasFuturas.length) {
    return null;
  }

  // Ordenar por fecha más cercana
  citasFuturas.sort((a, b) => new Date(a.fecha) - new Date(b.fecha));
  
  return citasFuturas[0];
});

// Obtener citas del paciente al cargar el componente
onMounted(async () => {
  if (userData.value.role === 'paciente' && userData.value.email) {
    try {
      const response = await getCitasByPacienteEmail(userData.value.email);
      pacienteCitas.value = response.citas || [];
    } catch (error) {
      console.error('Error al cargar citas del paciente:', error);
    }
  } else if (userData.value.role === 'medico' && userData.value.email) {
    try {
      const response = await getCitasByMedicoEmail(userData.value.email);
      medicoCitas.value = response.citas || [];
    } catch (error) {
      console.error('Error al cargar citas del médico:', error);
    }
  }
});

// Computed properties (se mantienen igual)
const userRole = computed(() => userData.value.role);
const currentYear = computed(() => new Date().getFullYear());

const roleData = {
  administrador: {
    icon: 'fas fa-user-shield',
    label: 'Administrador',
    welcome: 'Tienes control total sobre el sistema. Gestiona clínicas, usuarios y toda la información.',
    color: 'admin'
  },
  medico: {
    icon: 'fas fa-user-md',
    label: 'Médico',
    welcome: 'Tu dedicación salva vidas. Aquí puedes gestionar tus citas y pacientes.',
    color: 'doctor'
  },
  paciente: {
    icon: 'fas fa-user',
    label: 'Paciente',
    welcome: 'Tu salud es nuestra prioridad. Gestiona tus citas y consulta tu historial.',
    color: 'patient'
  }
};

const roleIcon = computed(() => roleData[userRole.value].icon);
const roleLabel = computed(() => roleData[userRole.value].label);
const welcomeMessage = computed(() => roleData[userRole.value].welcome);

// Mensajes motivacionales por rol
const motivationalMessages = {
  administrador: [
    "La excelencia en la gestión se logra con dedicación y atención al detalle.",
    "Tu trabajo hace posible que todo el sistema funcione sin problemas.",
    "Cada decisión que tomas impacta positivamente en la salud de muchos."
  ],
  medico: [
    "Tus manos curan, tu corazón consuela y tu conocimiento salva vidas.",
    "Detrás de cada paciente hay una historia, y tú eres parte importante de ella.",
    "La medicina no es solo una ciencia, es un arte que tú dominas."
  ],
  paciente: [
    "Tu salud es tu mayor riqueza, y estamos aquí para cuidarla contigo.",
    "Cada paso que das hacia el cuidado de tu salud es una victoria.",
    "Juntos podemos lograr que te sientas mejor y vivas plenamente."
  ]
};

const motivationalMessage = computed(() => {
  const messages = motivationalMessages[userRole.value];
  return messages[Math.floor(Math.random() * messages.length)];
});

// Acciones rápidas según rol
const quickActions = computed(() => {
  const baseActions = {
    administrador: [
      { title: 'Administrar Clínicas', icon: 'fas fa-hospital', path: '/clinicas', description: 'Controla las clínicas disponibles', color: 'action-blue' },
      { title: 'Gestionar Usuarios', icon: 'fas fa-users', path: '/usuarios', description: 'Administra todos los usuarios del sistema', color: 'action-purple' },
    ],
    medico: [
      { title: 'Mis Citas', icon: 'fas fa-calendar-check', path: '/citas-medicos', description: 'Revisa tu agenda de citas', color: 'action-blue' },
      { title: 'Pacientes', icon: 'fas fa-procedures', path: '/pacientes', description: 'Administra tus pacientes', color: 'action-green' },
    ],
    paciente: [
      { title: 'Mis Citas', icon: 'fas fa-calendar-alt', path: '/mis-citas', description: 'Revisa tus citas programadas', color: 'action-blue' },
    ]
  };
  
  return baseActions[userRole.value];
});

// Estadísticas según rol (modificada solo para pacientes)
const userStats = computed(() => {
  const baseStats = {
    administrador: [
      { title: 'Usuarios Activos', icon: 'fas fa-users', value: '128', color: 'stat-purple' },
      { title: 'Clínicas', icon: 'fas fa-hospital', value: '7', color: 'stat-blue' },
      { title: 'Citas Hoy', icon: 'fas fa-calendar-day', value: '56', color: 'stat-green' }
    ],
    medico: [],
    paciente: []
  };

  // Estadísticas para pacientes
  if (userRole.value === 'paciente') {
    if (proximaCita.value) {
      const fechaCita = new Date(proximaCita.value.fecha);
      const opcionesFecha = { day: 'numeric', month: 'short' };
      const fechaFormateada = fechaCita.toLocaleDateString('es-ES', opcionesFecha);
      
      baseStats.paciente.push({
        title: 'Próxima Cita', 
        icon: 'fas fa-calendar-check', 
        value: fechaFormateada, 
        color: 'stat-blue',
        extraInfo: `Con ${proximaCita.value.medico?.nombre || 'el médico'}`
      });
    } else {
      baseStats.paciente.push({
        title: 'Próximas Citas', 
        icon: 'fas fa-calendar-check', 
        value: 'No programadas', 
        color: 'stat-gray'
      });
    }

    baseStats.paciente.push({
      title: 'Citas Totales',
      icon: 'fas fa-calendar-week',
      value: pacienteCitas.value.length,
      color: 'stat-green'
    });
  }
  
  // Estadísticas para médicos
  if (userRole.value === 'medico') {
    // Citas programadas para hoy
    const citasHoy = medicoCitas.value.filter(cita => {
      const fechaCita = new Date(cita.fecha);
      const hoy = new Date();
      return fechaCita.toDateString() === hoy.toDateString();
    }).length;

    // Citas pendientes (futuras)
    const citasPendientes = medicoCitas.value.filter(cita => {
      return new Date(cita.fecha) > new Date();
    }).length;

    // Total de citas
    const totalCitas = medicoCitas.value.length;

    baseStats.medico.push(
      {
        title: 'Citas Hoy',
        icon: 'fas fa-calendar-day',
        value: citasHoy,
        color: 'stat-blue'
      },
      {
        title: 'Citas Pendientes',
        icon: 'fas fa-calendar-check',
        value: citasPendientes,
        color: 'stat-green'
      },
      {
        title: 'Total Citas',
        icon: 'fas fa-calendar-week',
        value: totalCitas,
        color: 'stat-purple'
      }
    );
  }

  return baseStats[userRole.value];
});

</script>
  
  <style scoped>
  @import './Lobby.css';
  </style>