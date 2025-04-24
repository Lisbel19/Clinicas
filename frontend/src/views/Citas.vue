<template>
  <div class="citas-container container py-5">
    <div class="text-center mb-5">
      <i class="fa-solid fa-calendar-check fa-3x text-primary mb-3"></i>
      <h2 class="fw-bold">Gestión de Citas Médicas</h2>
      <p class="text-muted">Administra las citas del sistema.</p>
      <button class="btn btn-success mt-2" @click="mostrarModalNuevaCita">
        <i class="fa-solid fa-plus me-2"></i> Nueva Cita
      </button>
    </div>

    <div class="table-responsive">
      <table class="table table-hover">
        <thead>
          <tr>
            <th>ID</th>
            <th>Fecha</th>
            <th>Paciente</th>
            <th>Médico</th>
            <th>Clínica</th>
            <th>Acciones</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="cita in citas" :key="cita.id">
            <td>{{ cita.id }}</td>
            <td>{{ formatFecha(cita.fecha) }}</td>
            <td>{{ cita.paciente?.nombre || 'N/A' }}</td>
            <td>{{ cita.medico?.nombre || 'N/A' }}</td>
            <td>{{ cita.clinica?.nombre || 'N/A' }}</td>
            <td>
              <button class="btn btn-sm btn-outline-danger" @click="eliminarCita(cita.id)">
                <i class="fa-solid fa-trash"></i>
              </button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>

    <!-- Modal creación -->
    <div class="modal fade" id="modalNuevaCita" tabindex="-1" aria-hidden="true">
      <div class="modal-dialog modal-lg">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title">Registrar Nueva Cita</h5>
            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Cerrar"></button>
          </div>
          <div class="modal-body">
            <form @submit.prevent="guardarCita">
              <div class="row mb-3">
                <div class="col-md-6">
                  <label for="clinicaId" class="form-label">Clínica</label>
                  <select 
                    v-model="citaForm.clinicaId" 
                    class="form-select" 
                    id="clinicaId"
                    required
                    @change="cargarPacientesYEspecialidades"
                  >
                    <option value="">Seleccione una clínica...</option>
                    <option 
                      v-for="clinica in clinicas" 
                      :key="clinica.id" 
                      :value="clinica.id"
                    >
                      {{ clinica.nombre }}
                    </option>
                  </select>
                </div>
              </div>

              <div class="row mb-3">
                <div class="col-md-6">
                  <label for="pacienteId" class="form-label">Paciente</label>
                  <select 
                    v-model="citaForm.pacienteId" 
                    class="form-select" 
                    id="pacienteId"
                    required
                    :disabled="!citaForm.clinicaId"
                  >
                    <option value="">Seleccione un paciente...</option>
                    <option 
                      v-for="paciente in pacientesFiltrados" 
                      :key="paciente.id" 
                      :value="paciente.id"
                    >
                      {{ paciente.nombre }} {{ paciente.apellido || '' }}
                    </option>
                  </select>
                </div>
                
                <div class="col-md-6">
                  <label for="especialidadId" class="form-label">Especialidad</label>
                  <select 
                    v-model="especialidadSeleccionada" 
                    class="form-select" 
                    id="especialidadId"
                    required
                    :disabled="!citaForm.clinicaId"
                    @change="cargarMedicos"
                  >
                    <option value="">Seleccione una especialidad...</option>
                    <option 
                      v-for="especialidad in especialidadesFiltradas" 
                      :key="especialidad.id" 
                      :value="especialidad.id"
                    >
                      {{ especialidad.nombre }}
                    </option>
                  </select>
                </div>
              </div>

              <div class="row mb-3">
                <div class="col-md-6">
                  <label for="medicoId" class="form-label">Médico</label>
                  <select 
                    v-model="citaForm.medicoId" 
                    class="form-select" 
                    id="medicoId"
                    required
                    :disabled="!especialidadSeleccionada"
                  >
                    <option value="">Seleccione un médico...</option>
                    <option 
                      v-for="medico in medicosFiltrados" 
                      :key="medico.id" 
                      :value="medico.id"
                    >
                      {{ medico.nombre }} {{ medico.apellido || '' }} - {{ medico.especialidad?.nombre }}
                    </option>
                  </select>
                </div>
                
                <div class="col-md-6">
                  <label for="fecha" class="form-label">Fecha y Hora</label>
                  <input 
                    type="datetime-local" 
                    v-model="citaForm.fecha" 
                    class="form-control" 
                    id="fecha" 
                    required
                    :disabled="!citaForm.medicoId"
                  />
                </div>
              </div>
              
              <button type="submit" class="btn btn-primary">Registrar</button>
            </form>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import { 
  getAllCitas, 
  deleteCita, 
  createCita,
  getClinicasParaCitas,
  getPacientesByClinica,
  getEspecialidadesByClinica,
  getMedicosByClinicaAndEspecialidad
} from '@/services/CitaService';

// Estados
const citas = ref([]);
const clinicas = ref([]);
const pacientesFiltrados = ref([]);
const especialidadesFiltradas = ref([]);
const medicosFiltrados = ref([]);
const especialidadSeleccionada = ref('');
const citaForm = ref({
  fecha: '',
  pacienteId: '',
  medicoId: '',
  clinicaId: ''
});

// Carga inicial
onMounted(async () => {
  await cargarCitas();
  await cargarClinicas();
});

const cargarCitas = async () => {
  try {
    citas.value = await getAllCitas();
  } catch (error) {
    console.error('Error al cargar citas:', error);
    alert('Error al cargar citas: ' + error.message);
  }
};

const cargarClinicas = async () => {
  try {
    clinicas.value = await getClinicasParaCitas();
  } catch (error) {
    console.error('Error al cargar clínicas:', error);
    alert('Error al cargar clínicas: ' + error.message);
  }
};

const cargarPacientesYEspecialidades = async () => {
  if (!citaForm.value.clinicaId) return;
  
  try {
    const [pacientes, especialidades] = await Promise.all([
      getPacientesByClinica(citaForm.value.clinicaId),
      getEspecialidadesByClinica(citaForm.value.clinicaId)
    ]);
    
    pacientesFiltrados.value = pacientes;
    especialidadesFiltradas.value = especialidades;
    
    // Resetear selecciones dependientes
    citaForm.value.pacienteId = '';
    citaForm.value.medicoId = '';
    especialidadSeleccionada.value = '';
    medicosFiltrados.value = [];
  } catch (error) {
    console.error('Error al cargar datos:', error);
    alert('Error al cargar pacientes y especialidades: ' + error.message);
  }
};

const cargarMedicos = async () => {
  if (!citaForm.value.clinicaId || !especialidadSeleccionada.value) return;
  
  try {
    medicosFiltrados.value = await getMedicosByClinicaAndEspecialidad(
      citaForm.value.clinicaId, 
      especialidadSeleccionada.value
    );
    citaForm.value.medicoId = '';
  } catch (error) {
    console.error('Error al cargar médicos:', error);
    alert('Error al cargar médicos: ' + error.message);
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

// Mostrar modal de creación
const mostrarModalNuevaCita = () => {
  citaForm.value = { 
    fecha: '',
    pacienteId: '',
    medicoId: '',
    clinicaId: ''
  };
  especialidadSeleccionada.value = '';
  pacientesFiltrados.value = [];
  especialidadesFiltradas.value = [];
  medicosFiltrados.value = [];
  
  const modal = new bootstrap.Modal(document.getElementById('modalNuevaCita'));
  modal.show();
};

// Guardar cita
const guardarCita = async () => {
  try {
    // Validaciones básicas
    if (!citaForm.value.fecha) {
      throw new Error('La fecha es requerida');
    }
    
    if (!citaForm.value.pacienteId) {
      throw new Error('El paciente es requerido');
    }
    
    if (!citaForm.value.medicoId) {
      throw new Error('El médico es requerido');
    }
    
    if (!citaForm.value.clinicaId) {
      throw new Error('La clínica es requerida');
    }

    // Crear nueva cita
    await createCita({
      fecha: citaForm.value.fecha,
      pacienteId: citaForm.value.pacienteId,
      medicoId: citaForm.value.medicoId,
      clinicaId: citaForm.value.clinicaId
    });
    
    // Recargar datos y cerrar modal
    await cargarCitas();
    bootstrap.Modal.getInstance(document.getElementById('modalNuevaCita')).hide();
    
  } catch (error) {
    console.error('Error al guardar cita:', error);
    alert(`Error: ${error.message}`);
  }
};

// Eliminar cita
const eliminarCita = async (id) => {
  if (confirm('¿Estás seguro que deseas eliminar esta cita?')) {
    try {
      await deleteCita(id);
      await cargarCitas();
    } catch (error) {
      console.error('Error al eliminar cita:', error);
      alert('Error al eliminar cita: ' + error.message);
    }
  }
};
</script>