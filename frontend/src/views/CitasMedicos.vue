<template>
    <div class="citas-medicos-container container py-5">
      <div class="text-center mb-5">
        <i class="fa-solid fa-calendar-check fa-3x text-primary mb-3"></i>
        <h2 class="fw-bold">Mis Citas Médicas</h2>
        <p v-if="nombreMedico" class="text-muted">Dr. {{ nombreMedico }}</p>
      </div>
  
      <div v-if="loading" class="text-center">
        <div class="spinner-border text-primary" role="status">
          <span class="visually-hidden">Cargando...</span>
        </div>
        <p>Cargando citas...</p>
      </div>
  
      <div v-else-if="error" class="alert alert-danger">
        {{ error }}
      </div>
  
      <div v-else-if="citas.length === 0" class="alert alert-info">
        No tienes citas programadas.
      </div>
  
      <div v-else class="table-responsive">
        <table class="table table-hover">
          <thead>
            <tr>
              <th>Fecha</th>
              <th>Paciente</th>
              <th>Especialidad</th>
              <th>Clínica</th>
              <th>Estado</th>
              <th>Acciones</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="cita in citas" :key="cita.id">
              <td>{{ formatFecha(cita.fecha) }}</td>
              <td>{{ cita.paciente?.nombre }} {{ cita.paciente?.apellido }}</td>
              <td>{{ cita.medico?.especialidad?.nombre || 'N/A' }}</td>
              <td>{{ cita.clinica?.nombre }}</td>
              <td>
                <span :class="getEstadoCitaClass(cita.fecha)">
                  {{ getEstadoCita(cita.fecha) }}
                </span>
              </td>
              <td>
                <button 
                  class="btn btn-sm btn-outline-danger" 
                  @click="confirmarCancelacion(cita.id)"
                  :disabled="!puedeCancelar(cita.fecha)"
                >
                  <i class="fa-solid fa-ban"></i> Cancelar
                </button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </template>
  
  <script setup>
  import { ref, onMounted } from 'vue';
  import axios from 'axios';
  import { API_BASE_URL } from '@/services/api';
  import { useRouter } from 'vue-router';
  
  const router = useRouter();
  const citas = ref([]);
  const nombreMedico = ref('');
  const loading = ref(true);
  const error = ref(null);
  
  // Carga inicial
  onMounted(async () => {
    await cargarCitasMedico();
  });
  
  const cargarCitasMedico = async () => {
    try {
      loading.value = true;
      error.value = null;
      
      // Verificar autenticación
      const userData = JSON.parse(localStorage.getItem('userData'));
      if (!userData) {
        throw new Error('No se encontraron datos de usuario. Por favor inicie sesión.');
      }
  
      // Verificar que el usuario es médico
      if (userData.tipo_usuario !== 'medico') {
        router.push('/unauthorized');
        return;
      }
  
      const email = userData.correo;
      if (!email) {
        throw new Error('No se pudo obtener el email del médico');
      }
  
      // Hacer la llamada al endpoint
      const response = await axios.get(`${API_BASE_URL}/citas/por-correo-medico/${encodeURIComponent(email)}`, {
        headers: {
          'Authorization': `Bearer ${localStorage.getItem('authToken')}`
        }
      });
  
      // Verificar estructura de respuesta
      if (!response.data || !Array.isArray(response.data.citas)) {
        throw new Error('Formato de respuesta inesperado');
      }
  
      citas.value = response.data.citas;
      nombreMedico.value = response.data.nombre_medico || `${userData.nombre || ''} ${userData.apellido || ''}`.trim() || 'Médico';
      
    } catch (err) {
      console.error('Error al cargar citas:', err);
      error.value = err.response?.data?.message || err.message || 'Error al cargar las citas';
      citas.value = [];
    } finally {
      loading.value = false;
    }
  };
  
  // Formatear fecha
  const formatFecha = (fechaString) => {
    const fecha = new Date(fechaString);
    return fecha.toLocaleString('es-ES', {
      day: '2-digit',
      month: '2-digit',
      year: 'numeric',
      hour: '2-digit',
      minute: '2-digit'
    });
  };
  
  // Determinar estado de la cita
  const getEstadoCita = (fecha) => {
    const ahora = new Date();
    const fechaCita = new Date(fecha);
    return fechaCita < ahora ? 'Completada' : 'Pendiente';
  };
  
  const getEstadoCitaClass = (fecha) => {
    return getEstadoCita(fecha) === 'Pendiente' ? 'badge bg-warning' : 'badge bg-success';
  };
  
  // Verificar si la cita puede cancelarse
  const puedeCancelar = (fecha) => {
    const fechaCita = new Date(fecha);
    const ahora = new Date();
    return fechaCita > ahora;
  };
  
  // Confirmar cancelación de cita
  const confirmarCancelacion = async (id) => {
    if (confirm('¿Estás seguro que deseas cancelar esta cita?')) {
      try {
        await axios.delete(`${API_BASE_URL}/citas/medico/${id}`, {
          headers: {
            'Authorization': `Bearer ${localStorage.getItem('authToken')}`
          }
        });
        await cargarCitasMedico();
      } catch (err) {
        console.error('Error al cancelar cita:', err);
        error.value = err.response?.data?.message || err.message || 'Error al cancelar la cita';
      }
    }
  };
  </script>
  
  <style scoped src="./CitasMedicos.css"></style>