<template>
  <div class="home-container d-flex flex-column align-items-center justify-content-center text-center">
    <!-- Branding -->
    <div class="branding mb-5">
      <i class="fa-solid fa-hospital fa-4x text-primary mb-3"></i>
      <h1 class="fw-bold">MG Clinic Management</h1>
      <p class="lead">Solución integral y moderna para la gestión de clínicas.</p>
      <button class="btn btn-primary mt-3" data-bs-toggle="modal" data-bs-target="#loginModal">
        <i class="fa-solid fa-right-to-bracket me-2"></i> Iniciar Sesión
      </button>
    </div>

    <!-- Características -->
    <div class="features container text-start">
      <h2 class="text-center text-primary mb-4">¿Qué puedes hacer con MG Clinic?</h2>
      <div class="row">
        <div class="col-md-4 mb-4">
          <div class="feature-box p-4 h-100 shadow-sm rounded bg-white">
            <i class="fa-solid fa-user-doctor fa-2x text-success mb-2"></i>
            <h5>Médicos</h5>
            <p>Gestionan diagnósticos, citas, y seguimiento de pacientes desde un solo lugar.</p>
          </div>
        </div>
        <div class="col-md-4 mb-4">
          <div class="feature-box p-4 h-100 shadow-sm rounded bg-white">
            <i class="fa-solid fa-user fa-2x text-info mb-2"></i>
            <h5>Pacientes</h5>
            <p>Pueden consultar sus citas médicas, diagnósticos y comunicarse con la clínica fácilmente.</p>
          </div>
        </div>
        <div class="col-md-4 mb-4">
          <div class="feature-box p-4 h-100 shadow-sm rounded bg-white">
            <i class="fa-solid fa-user-shield fa-2x text-warning mb-2"></i>
            <h5>Administradores</h5>
            <p>Controlan clínicas, usuarios, médicos, citas y toda la información del sistema.</p>
          </div>
        </div>
      </div>
    </div>

    <!-- Llamado a la acción -->
    <div class="cta text-center mt-5">
      <h3 class="fw-bold text-dark">¿Listo para digitalizar tu clínica?</h3>
      <p>Moderniza tu gestión con MG Clinic Management. Fácil, rápido y seguro.</p>
      <a href="#loginModal" class="btn btn-success btn-lg" data-bs-toggle="modal">
        <i class="fa-solid fa-paper-plane me-2"></i> Solicita una Demo
      </a>
    </div>
  </div>

    <!-- Modal Login -->
  <div class="modal fade" id="loginModal" tabindex="-1" aria-labelledby="loginModalLabel" aria-hidden="true">
    <div class="modal-dialog">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title" id="loginModalLabel">Iniciar Sesión</h5>
          <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Cerrar"></button>
        </div>
        <div class="modal-body">
          <form @submit.prevent="handleLogin">
            <div class="mb-3">
              <label for="correo" class="form-label">Correo electrónico</label>
              <input v-model="correo" type="email" class="form-control" id="correo" required :disabled="isAnimating">
            </div>
            <div class="mb-3">
              <label for="contrasena" class="form-label">Contraseña</label>
              <input v-model="contrasena" type="password" class="form-control" id="contrasena" required :disabled="isAnimating">
            </div>
            <div v-if="errorMessage" class="alert alert-danger">
              {{ errorMessage }}
            </div>
            
            <!-- Contenedor de animación -->
            <div v-if="isAnimating" class="loading-animation text-center py-4">
              <div class="spinner-border text-primary" style="width: 3rem; height: 3rem;" role="status">
                <span class="visually-hidden">Cargando...</span>
              </div>
              <h5 class="mt-3 text-primary">Iniciando sesión...</h5>
              <div class="progress mt-3">
                <div class="progress-bar progress-bar-striped progress-bar-animated" 
                     role="progressbar" 
                     :style="{ width: progress + '%' }"></div>
              </div>
            </div>
            
            <button v-else type="submit" class="btn btn-success w-100" :disabled="loading">
              <span v-if="loading" class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>
              {{ loading ? 'Validando...' : 'Ingresar' }}
            </button>
          </form>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import './Home.css';
import { ref, onMounted, onBeforeUnmount } from 'vue';
import { useRouter } from 'vue-router';
import { login } from '@/services/AuthService';

const router = useRouter();
const correo = ref('');
const contrasena = ref('');
const errorMessage = ref('');
const loading = ref(false);
const isAnimating = ref(false);
const progress = ref(0);
let loginModal = null;
let animationInterval = null;

// Función para limpiar completamente el modal
const cleanUpModal = () => {
  const modalElement = document.getElementById('loginModal');
  if (modalElement) {
    // Forzar el cierre del modal
    modalElement.classList.remove('show');
    modalElement.style.display = 'none';
    modalElement.setAttribute('aria-hidden', 'true');
    modalElement.removeAttribute('aria-modal');
    modalElement.removeAttribute('role');
    
    // Eliminar el backdrop
    const backdrops = document.querySelectorAll('.modal-backdrop');
    backdrops.forEach(backdrop => backdrop.remove());
    
    // Restaurar el scroll del body
    document.body.style.overflow = '';
    document.body.style.paddingRight = '';
    document.body.classList.remove('modal-open');
    
    // Resetear el modal de Bootstrap
    const modalInstance = bootstrap.Modal.getInstance(modalElement);
    if (modalInstance) {
      modalInstance.dispose();
    }
  }
};

onMounted(() => {
  const modalElement = document.getElementById('loginModal');
  if (modalElement) {
    modalElement.addEventListener('hidden.bs.modal', cleanUpModal);
    loginModal = new bootstrap.Modal(modalElement, {
      backdrop: 'static',
      keyboard: false
    });
  }
});

onBeforeUnmount(() => {
  if (loginModal) {
    loginModal.hide();
  }
  cleanUpModal();
  if (animationInterval) clearInterval(animationInterval);
});

const startAnimation = () => {
  isAnimating.value = true;
  progress.value = 0;
  
  // Animación de la barra de progreso durante 2 segundos
  animationInterval = setInterval(() => {
    progress.value += 2;
    if (progress.value >= 100) {
      clearInterval(animationInterval);
    }
  }, 40); // 40ms * 50 incrementos = 2000ms (2 segundos)
};

const handleLogin = async () => {
  errorMessage.value = '';
  loading.value = true;

  try {
    const credentials = {
      correo: correo.value,
      contrasena: contrasena.value
    };

    const authData = await login(credentials);
    
    // Guardar datos de autenticación
    localStorage.setItem('authToken', authData.token);
    localStorage.setItem('userData', JSON.stringify(authData.user));
    localStorage.setItem('userRole', authData.user.tipo_usuario);

    // Iniciar animación
    startAnimation();
    
    // Esperar 2 segundos para la animación
    await new Promise(resolve => setTimeout(resolve, 2000));

    // Cerrar modal manualmente antes de redireccionar
    cleanUpModal();

    // Forzar un reflow para asegurar que los cambios se apliquen
    document.body.getBoundingClientRect();

    // Redirección por rol
    const redirectPath = {
      'administrador': '/lobby',
      'medico': '/lobby',
      'paciente': '/lobby'
    }[authData.user.tipo_usuario] || '/';

    router.push(redirectPath);

  } catch (error) {
    isAnimating.value = false;
    if (animationInterval) clearInterval(animationInterval);
    errorMessage.value = error.message || 'Error al iniciar sesión';
    console.error('Login error:', error);
  } finally {
    loading.value = false;
  }
};
</script>

<style>
/* Agrega esto a tu Home.css */
.loading-animation {
  animation: fadeIn 0.3s ease-in-out;
}

.progress {
  height: 0.5rem;
  background-color: #e9ecef;
  border-radius: 0.25rem;
}

.progress-bar {
  transition: width 0.1s linear;
}

@keyframes fadeIn {
  from { opacity: 0; transform: translateY(10px); }
  to { opacity: 1; transform: translateY(0); }
}
</style>