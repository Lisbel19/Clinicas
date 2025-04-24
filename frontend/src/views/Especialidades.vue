<template>
  <div class="especialidades-container container py-5">
    <div class="text-center mb-5">
      <i class="fa-solid fa-user-doctor fa-3x text-primary mb-3"></i>
      <h2 class="fw-bold">Gestión de Especialidades</h2>
      <p class="text-muted">Administra las especialidades médicas disponibles en el sistema.</p>
      <button class="btn btn-success mt-2" @click="mostrarModalNuevaEspecialidad">
        <i class="fa-solid fa-plus me-2"></i> Nueva Especialidad
      </button>
    </div>

    <div class="table-responsive">
      <table class="table table-hover">
        <thead>
          <tr>
            <th>ID</th>
            <th>Nombre</th>
            <th>Acciones</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="especialidad in especialidades" :key="especialidad.id">
            <td>{{ especialidad.id }}</td>
            <td>{{ especialidad.nombre }}</td>
            <td>
              <button class="btn btn-sm btn-outline-secondary me-2" @click="editarEspecialidad(especialidad)">
                <i class="fa-solid fa-pen-to-square"></i>
              </button>
              <button class="btn btn-sm btn-outline-danger" @click="eliminarEspecialidad(especialidad.id)">
                <i class="fa-solid fa-trash"></i>
              </button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>

    <!-- Modal creación/edición -->
    <div class="modal fade" id="modalNuevaEspecialidad" tabindex="-1" aria-hidden="true">
      <div class="modal-dialog">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title">{{ modalTitle }}</h5>
            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Cerrar"></button>
          </div>
          <div class="modal-body">
            <form @submit.prevent="guardarEspecialidad">
              <div class="mb-3">
                <label for="nombre" class="form-label">Nombre</label>
                <input type="text" v-model="especialidadForm.nombre" class="form-control" id="nombre" required />
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
import './Especialidades.css';
import { onMounted, ref } from 'vue';
import { 
  getAllEspecialidades, 
  deleteEspecialidad, 
  createEspecialidad, 
  updateEspecialidad 
} from '@/services/EspecialidadService';

// Estados
const especialidades = ref([]);
const modalTitle = ref('Registrar Nueva Especialidad');
const modalButtonText = ref('Registrar');
const especialidadForm = ref({
  id: null,
  nombre: ''
});

// Carga inicial
onMounted(async () => {
  await cargarEspecialidades();
});

const cargarEspecialidades = async () => {
  try {
    especialidades.value = await getAllEspecialidades();
  } catch (error) {
    console.error('Error al cargar especialidades:', error);
    alert('Error al cargar las especialidades');
  }
};

// Mostrar modal de creación
const mostrarModalNuevaEspecialidad = () => {
  especialidadForm.value = { id: null, nombre: '' };
  modalTitle.value = 'Registrar Nueva Especialidad';
  modalButtonText.value = 'Registrar';
  const modal = new bootstrap.Modal(document.getElementById('modalNuevaEspecialidad'));
  modal.show();
};

// Mostrar modal para editar
const editarEspecialidad = (especialidad) => {
  especialidadForm.value = { 
    id: especialidad.id,
    nombre: especialidad.nombre
  };
  modalTitle.value = 'Editar Especialidad';
  modalButtonText.value = 'Actualizar';
  const modal = new bootstrap.Modal(document.getElementById('modalNuevaEspecialidad'));
  modal.show();
};

// Guardar especialidad
const guardarEspecialidad = async () => {
  try {
    // Validaciones
    if (!especialidadForm.value.nombre?.trim()) {
      throw new Error('El nombre de la especialidad es requerido');
    }

    if (especialidadForm.value.id) {
      // Actualización
      await updateEspecialidad({
        id: especialidadForm.value.id,
        nombre: especialidadForm.value.nombre.trim()
      });
    } else {
      // Creación
      await createEspecialidad({
        nombre: especialidadForm.value.nombre.trim()
      });
    }
    
    await cargarEspecialidades();
    bootstrap.Modal.getInstance(document.getElementById('modalNuevaEspecialidad')).hide();
    
  } catch (error) {
    console.error('Error en guardarEspecialidad:', {
      error: error,
      formData: especialidadForm.value,
      response: error.response?.data
    });
    
    const errorMessage = error.response?.data?.title || 
                        error.response?.data?.message || 
                        error.message || 
                        'Error al guardar la especialidad';
    
    alert(`Error: ${errorMessage}`);
  }
};

// Eliminar especialidad
const eliminarEspecialidad = async (id) => {
  if (confirm('¿Estás seguro que deseas eliminar esta especialidad?')) {
    try {
      await deleteEspecialidad(id);
      await cargarEspecialidades();
    } catch (error) {
      console.error('Error al eliminar especialidad:', error);
      alert(`Error: ${error.response?.data?.message || error.message}`);
    }
  }
};
</script>