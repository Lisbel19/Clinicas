<template>
  <div class="mis-citas-container container py-5">
    <div class="text-center mb-5">
      <i class="fa-solid fa-calendar-days fa-3x text-primary mb-3"></i>
      <h2 class="fw-bold">Mis Citas Médicas</h2>
    </div>

    <div v-if="citas.length === 0" class="alert alert-info">
      No tienes citas programadas.
    </div>

    <div v-else class="table-responsive">
      <table class="table table-hover">
        <thead>
          <tr>
            <th>Fecha</th>
            <th>Médico</th>
            <th>Especialidad</th>
            <th>Clínica</th>
            <th>Estado</th>
            <th>Acciones</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="cita in citas" :key="cita.id">
            <td>{{ formatFecha(cita.fecha) }}</td>
            <td>{{ cita.medico?.nombre || 'N/A' }}</td>
            <td>{{ cita.medico?.especialidad?.nombre || 'N/A' }}</td>
            <td>{{ cita.clinica?.nombre || 'N/A' }}</td>
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
import { getCitasByPacienteEmail, cancelarCita } from '@/services/MisCitaService';

// Estados
const citas = ref([]);
const nombrePaciente = ref('');

// Obtener email del usuario logueado desde localStorage
const userEmail = localStorage.getItem('userEmail');

// Carga inicial
onMounted(async () => {
  await cargarCitasPaciente();
});

const cargarCitasPaciente = async () => {
  try {
    // Obtener el correo del usuario logueado desde localStorage
    const userData = JSON.parse(localStorage.getItem('userData'));
    const email = userData?.correo;
    
    if (!email) {
      throw new Error('No se pudo obtener el email del usuario');
    }
    
    // Hacer la llamada al endpoint
    const response = await getCitasByPacienteEmail(email);
    
    citas.value = response.citas;
    
    // Obtener el nombre del paciente (puedes ajustar esto según lo que devuelva tu backend)
    nombrePaciente.value = response.nombrePaciente || 
                          userData?.nombre || 
                          'Paciente';
  } catch (error) {
    console.error('Error al cargar citas del paciente:', error);
    alert('Error al cargar tus citas: ' + error.message);
  }
};

// Formatear fecha para mostrar
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
  
  if (fechaCita < ahora) return 'Completada';
  return 'Pendiente';
};

const getEstadoCitaClass = (fecha) => {
  const estado = getEstadoCita(fecha);
  return estado === 'Pendiente' ? 'badge bg-warning' : 'badge bg-success';
};

// Verificar si la cita puede cancelarse (solo citas futuras)
const puedeCancelar = (fecha) => {
  const fechaCita = new Date(fecha);
  const ahora = new Date();
  return fechaCita > ahora;
};

// Confirmar cancelación de cita
const confirmarCancelacion = (id) => {
  if (confirm('¿Estás seguro que deseas cancelar esta cita?')) {
    cancelarCita(id);
    // Recargar las citas después de cancelar
    cargarCitasPaciente();
  }
};
</script>

<style scoped src="./MisCitas.css"></style>