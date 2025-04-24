<template>
    <nav class="navbar navbar-expand-lg navbar-light bg-light">
      <div class="container-fluid">
        <router-link to="/" class="navbar-brand">
          <i class="fas fa-hospital me-2 text-primary"></i>MG Clinic
        </router-link>
        
        <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNav">
          <span class="navbar-toggler-icon"></span>
        </button>
        
        <div class="collapse navbar-collapse" id="navbarNav">
          <ul class="navbar-nav me-auto">
            <!-- Menú para Administrador (completo) -->
            <template v-if="userRole === 'administrador'">
              <li class="nav-item">
                <router-link to="/lobby" class="nav-link">
                  <i class="fas fa-home me-1"></i> Lobby
                </router-link>
              </li>
              <li class="nav-item">
                <router-link to="/clinicas" class="nav-link">
                  <i class="fas fa-clinic-medical me-1"></i> Clínicas
                </router-link>
              </li>
              <li class="nav-item">
                <router-link to="/especialidades" class="nav-link">
                  <i class="fas fa-stethoscope me-1"></i>Especialidades
                </router-link>
              </li>
              <li class="nav-item">
                <router-link to="/usuarios" class="nav-link">
                  <i class="fas fa-users-cog me-1"></i> Usuarios
                </router-link>
              </li>
              <li class="nav-item">
                <router-link to="/medicos" class="nav-link">
                  <i class="fas fa-user-md me-1"></i> Médicos
                </router-link>
              </li>
              <li class="nav-item">
                <router-link to="/pacientes" class="nav-link">
                  <i class="fas fa-procedures me-1"></i> Pacientes
                </router-link>
              </li>
              <li class="nav-item">
                <router-link to="/citas" class="nav-link">
                  <i class="fas fa-calendar-check me-1"></i> Citas
                </router-link>
              </li>
            </template>
            
            <!-- Menú para Médico -->
            <template v-if="userRole === 'medico'">
              <li class="nav-item">
                <router-link to="/lobby" class="nav-link">
                  <i class="fas fa-home me-1"></i> Lobby
                </router-link>
              </li>
              <li class="nav-item">
                <router-link to="/citas-medicos" class="nav-link">
                  <i class="fas fa-calendar-alt me-1"></i> Ver Citas Asignadas
                </router-link>
              </li>
            </template>
            
            <!-- Menú para Paciente -->
            <template v-if="userRole === 'paciente'">
              <li class="nav-item">
                <router-link to="/lobby" class="nav-link">
                  <i class="fas fa-home me-1"></i> Lobby
                </router-link>
              </li>
              <li class="nav-item">
                <router-link to="/mis-citas" class="nav-link">
                  <i class="fas fa-calendar-alt me-1"></i> Mis Citas
                </router-link>
              </li>
            </template>
          </ul>
          
          <div class="d-flex">
            <button class="btn btn-outline-danger" @click="$emit('logout')">
              <i class="fas fa-sign-out-alt me-2"></i> Cerrar Sesión
            </button>
          </div>
        </div>
      </div>
    </nav>
  </template>
  
  <script setup>
  defineProps({
    userRole: String
  });
  </script>
  
  <style scoped>
  .navbar {
    box-shadow: 0 2px 4px rgba(0,0,0,0.1);
    margin-bottom: 20px;
  }
  
  .nav-link {
    font-weight: 500;
    margin-right: 10px;
    display: flex;
    align-items: center;
    transition: all 0.2s ease;
  }
  
  .nav-link:hover {
    transform: translateY(-2px);
  }
  
  .nav-link.router-link-exact-active {
    color: var(--bs-primary);
    font-weight: bold;
    border-bottom: 2px solid var(--bs-primary);
  }
  
  .nav-link i {
    width: 20px;
    text-align: center;
  }
  
  .btn-outline-danger {
    transition: all 0.3s ease;
    display: flex;
    align-items: center;
  }
  
  /* Animación para el icono de logout */
  .btn-outline-danger:hover i {
    animation: shake 0.5s;
  }
  
  @keyframes shake {
    0% { transform: translateX(0); }
    25% { transform: translateX(-3px); }
    50% { transform: translateX(3px); }
    75% { transform: translateX(-3px); }
    100% { transform: translateX(0); }
  }
  
  /* Efecto hover para items del menú */
  .nav-item:hover .nav-link:not(.router-link-exact-active) {
    color: var(--bs-primary);
  }
  </style>