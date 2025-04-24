<template>
    <div class="medicos-container container py-5">
      <div class="text-center mb-5">
        <i class="fa-solid fa-user-doctor fa-3x text-primary mb-3"></i>
        <h2 class="fw-bold">Gestión de Médicos</h2>
        <p class="text-muted">Administra los médicos del sistema.</p>
        <button class="btn btn-success mt-2" @click="mostrarModalNuevoMedico">
          <i class="fa-solid fa-plus me-2"></i> Nuevo Médico
        </button>
      </div>
  
      <div class="table-responsive">
        <table class="table table-hover">
          <thead>
            <tr>
              <th>ID</th>
              <th>Nombre</th>
              <th>Apellido</th>
              <th>Especialidad</th>
              <th>Teléfono</th>
              <th>Correo</th>
              <th>Horario Consulta</th>
              <th>Acciones</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="medico in medicos" :key="medico.id">
              <td>{{ medico.id }}</td>
              <td>{{ medico.nombre }}</td>
              <td>{{ medico.apellido || 'N/A' }}</td>
              <td>{{ medico.especialidad?.nombre || 'N/A' }}</td>
              <td>{{ medico.telefono }}</td>
              <td>{{ medico.correo }}</td>
              <td>{{ JSON.stringify(medico.horario_consulta) || 'No especificado' }}</td>
              <td>
                <button class="btn btn-sm btn-outline-secondary me-2" @click="editarMedico(medico)">
                  <i class="fa-solid fa-pen-to-square"></i>
                </button>
                <button class="btn btn-sm btn-outline-danger" @click="eliminarMedico(medico.id)">
                  <i class="fa-solid fa-trash"></i>
                </button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
  
      <!-- Modal creación/edición -->
      <div class="modal fade" id="modalNuevoMedico" tabindex="-1" aria-hidden="true">
        <div class="modal-dialog modal-lg">
          <div class="modal-content">
            <div class="modal-header">
              <h5 class="modal-title">{{ modalTitle }}</h5>
              <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Cerrar"></button>
            </div>
            <div class="modal-body">
              <form @submit.prevent="guardarMedico">
                <div class="row mb-3">
                    <div class="col-md-12">
                        <label for="correoUsuario" class="form-label">Usuario Médico</label>
                        <select 
                        v-model="correoUsuario" 
                        class="form-select" 
                        id="correoUsuario"
                        :disabled="!!medicoForm.id" 
                        required
                        >
                        <option value="">Seleccione un usuario médico...</option>
                        <option 
                            v-for="usuario in usuariosMedicos" 
                            :key="usuario.id" 
                            :value="usuario.correo"
                        >
                            {{ usuario.correo }}
                        </option>
                        </select>
                    </div>
                </div>
                <div class="row">
                  <div class="col-md-6 mb-3">
                    <label for="nombre" class="form-label">Nombre</label>
                    <input type="text" v-model="medicoForm.nombre" class="form-control" id="nombre" required />
                  </div>
                  <div class="col-md-6 mb-3">
                    <label for="apellido" class="form-label">Apellido</label>
                    <input type="text" v-model="medicoForm.apellido" class="form-control" id="apellido" />
                  </div>
                </div>
                
                <div class="row">
                  <div class="col-md-6 mb-3">
                    <label for="especialidad" class="form-label">Especialidad</label>
                    <select v-model="medicoForm.especialidad_id" class="form-select" id="especialidad" required>
                      <option value="">Seleccione una especialidad...</option>
                      <option v-for="especialidad in especialidades" 
                              :key="especialidad.id" 
                              :value="especialidad.id">
                        {{ especialidad.nombre }}
                      </option>
                    </select>
                  </div>
                  <div class="col-md-6 mb-3">
                    <label for="horario" class="form-label">Horario Consulta</label>
                    <input type="text" v-model="medicoForm.horario_consulta" class="form-control" id="horario" />
                  </div>
                </div>
                
                <div class="row">
                  <div class="col-md-6 mb-3">
                    <label for="telefono" class="form-label">Teléfono</label>
                    <input type="tel" v-model="medicoForm.telefono" class="form-control" id="telefono" required />
                  </div>
                </div>
                
                <button type="submit" class="btn btn-primary">{{ modalButtonText }}</button>
              </form>
            </div>
          </div>
        </div>
      </div>
    </div>
</template>

<script setup>
import './Medicos.css';
import { onMounted, ref } from 'vue';
import { 
  getAllMedicos, 
  deleteMedico, 
  createMedico, 
  updateMedico 
} from '@/services/MedicoService';
import { getAllEspecialidades } from '@/services/EspecialidadService';
import { getUsuariosMedicos } from '@/services/UsuarioService';

// Estados
const medicos = ref([]);
const especialidades = ref([]);
const usuariosMedicos = ref([]);
const correoUsuario = ref('');
const modalTitle = ref('Registrar Nuevo Médico');
const modalButtonText = ref('Registrar');
const medicoForm = ref({
  id: null,
  nombre: '',
  apellido: '',
  telefono: '',
  horario_consulta: '',
  especialidad_id: null
});

// Carga inicial
onMounted(async () => {
  await cargarMedicos();
  await cargarEspecialidades();
  await cargarUsuariosMedicos();
});

const cargarMedicos = async () => {
  try {
    medicos.value = await getAllMedicos();
  } catch (error) {
    console.error('Error al cargar médicos:', error);
    alert('Error al cargar médicos: ' + error.message);
  }
};

const cargarEspecialidades = async () => {
  try {
    especialidades.value = await getAllEspecialidades();
  } catch (error) {
    console.error('Error al cargar especialidades:', error);
    alert('Error al cargar especialidades: ' + error.message);
  }
};

const cargarUsuariosMedicos = async () => {
  try {
    usuariosMedicos.value = await getUsuariosMedicos();
  } catch (error) {
    console.error('Error al cargar usuarios médicos:', error);
    alert('Error al cargar usuarios médicos: ' + error.message);
  }
};

// Mostrar modal de creación
const mostrarModalNuevoMedico = () => {
  correoUsuario.value = '';
  medicoForm.value = { 
    id: null,
    nombre: '',
    apellido: '',
    telefono: '',
    horario_consulta: '',
    especialidad_id: null
  };
  modalTitle.value = 'Registrar Nuevo Médico';
  modalButtonText.value = 'Registrar';
  const modal = new bootstrap.Modal(document.getElementById('modalNuevoMedico'));
  modal.show();
};

// Mostrar modal para editar
const editarMedico = (medico) => {
  correoUsuario.value = medico.correo;
  medicoForm.value = { 
    id: medico.id,
    nombre: medico.nombre,
    apellido: medico.apellido || '',
    telefono: medico.telefono,
    horario_consulta: medico.horario_consulta || '',
    especialidad_id: medico.especialidad?.id || null
  };
  modalTitle.value = 'Editar Médico';
  modalButtonText.value = 'Actualizar';
  const modal = new bootstrap.Modal(document.getElementById('modalNuevoMedico'));
  modal.show();
};

// Guardar médico
const guardarMedico = async () => {
  try {
    // Validaciones básicas
    if (!medicoForm.value.nombre?.trim()) {
      throw new Error('El nombre es requerido');
    }

    if (!medicoForm.value.telefono?.trim()) {
      throw new Error('El teléfono es requerido');
    }

    if (!medicoForm.value.especialidad_id) {
      throw new Error('La especialidad es requerida');
    }

    if (!medicoForm.value.id && !correoUsuario.value) {
      throw new Error('El correo del usuario es requerido');
    }

    // Operación de creación o actualización
    if (medicoForm.value.id) {
      await updateMedico(medicoForm.value);
    } else {
      await createMedico(medicoForm.value, correoUsuario.value);
    }
    
    await cargarMedicos();
    bootstrap.Modal.getInstance(document.getElementById('modalNuevoMedico')).hide();
    
  } catch (error) {
    console.error('Error al guardar médico:', error);
    alert('Error: ' + error.message);
  }
};

// Eliminar médico
const eliminarMedico = async (id) => {
  if (confirm('¿Estás seguro que deseas eliminar este médico?')) {
    try {
      await deleteMedico(id);
      await cargarMedicos();
    } catch (error) {
      console.error('Error al eliminar médico:', error);
      alert('Error al eliminar médico: ' + error.message);
    }
  }
};
</script>