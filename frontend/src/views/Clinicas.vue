<template>
    <div class="clinicas-container container py-5">
      <div class="text-center mb-5">
        <i class="fa-solid fa-stethoscope fa-3x text-primary mb-3"></i>
        <h2 class="fw-bold">Gestión de Clínicas</h2>
        <p class="text-muted">Consulta, administra y organiza todas las clínicas registradas en el sistema.</p>
        <button class="btn btn-success mt-2" @click="mostrarModalNuevaClinica">
          <i class="fa-solid fa-plus me-2"></i> Nueva Clínica
        </button>
      </div>
  
      <div class="row">
        <div v-for="clinica in clinicas" :key="clinica.id" class="col-md-4 mb-4">
          <div class="card h-100 shadow-sm">
            <div class="card-body">
              <h5 class="card-title text-primary">{{ clinica.nombre }}</h5>
              <p class="card-text"><strong>RNC:</strong> {{ clinica.rnc }}</p>
              <p class="card-text"><strong>Dirección:</strong> {{ clinica.direccion }}</p>
              <p class="card-text"><strong>Teléfono:</strong> {{ clinica.telefono }}</p>
              <p class="card-text"><strong>Correo:</strong> {{ clinica.correo }}</p>
              <p class="card-text"><strong>Horario:</strong> {{ clinica.horario }}</p>
              <div class="d-flex justify-content-end gap-2">
                <button class="btn btn-sm btn-outline-secondary" @click="editarClinica(clinica)">
                  <i class="fa-solid fa-pen-to-square"></i>
                </button>
                <button class="btn btn-sm btn-outline-danger" @click="eliminarClinica(clinica.id)">
                  <i class="fa-solid fa-trash"></i>
                </button>
              </div>
            </div>
          </div>
        </div>
      </div>
  
      <!-- Modal creación/edición -->
      <div class="modal fade" id="modalNuevaClinica" tabindex="-1" aria-labelledby="modalNuevaClinicaLabel" aria-hidden="true">
        <div class="modal-dialog">
          <div class="modal-content">
            <div class="modal-header">
              <h5 class="modal-title" id="modalNuevaClinicaLabel">{{ modalTitle }}</h5>
              <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Cerrar"></button>
            </div>
            <div class="modal-body">
              <form @submit.prevent="guardarClinica">
                <div class="mb-3">
                  <label for="nombre" class="form-label">Nombre</label>
                  <input type="text" v-model="clinicaForm.nombre" class="form-control" id="nombre" required />
                </div>
                <div class="mb-3">
                  <label for="rnc" class="form-label">RNC</label>
                  <input type="text" v-model="clinicaForm.rnc" class="form-control" id="rnc" required />
                </div>
                <div class="mb-3">
                  <label for="telefono" class="form-label">Teléfono</label>
                  <input type="text" v-model="clinicaForm.telefono" class="form-control" id="telefono" required />
                </div>
                <div class="mb-3">
                  <label for="direccion" class="form-label">Dirección</label>
                  <input type="text" v-model="clinicaForm.direccion" class="form-control" id="direccion" required />
                </div>
                <div class="mb-3">
                  <label for="correo" class="form-label">Correo</label>
                  <input type="email" v-model="clinicaForm.correo" class="form-control" id="correo" required />
                </div>
                <div class="mb-3">
                  <label for="horario" class="form-label">Horario</label>
                  <input type="text" v-model="clinicaForm.horario" class="form-control" id="horario" required />
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
  import './Clinicas.css';
  import { onMounted, ref } from 'vue';
  import { getAllClinicas, deleteClinica, createClinica, updateClinica } from '@/services/ClinicaService';
  
  // Estados
  const clinicas = ref([]);
  const modalTitle = ref('Registrar Nueva Clínica');
  const modalButtonText = ref('Registrar');
  const clinicaForm = ref({
    id: null,
    rnc: '',
    nombre: '',
    telefono: '',
    direccion: '',
    correo: '',
    horario: ''
  });
  
  // Carga inicial de clínicas
  onMounted(async () => {
    await cargarClinicas();
  });
  
  const cargarClinicas = async () => {
    try {
      clinicas.value = await getAllClinicas();
    } catch (error) {
      console.error('Error al cargar clínicas:', error);
    }
  };
  
  // Mostrar modal de creación
  const mostrarModalNuevaClinica = () => {
    clinicaForm.value = { id: null, rnc: '', nombre: '', telefono: '', direccion: '', correo: '', horario: '' }; // Limpiar el formulario
    modalTitle.value = 'Registrar Nueva Clínica';
    modalButtonText.value = 'Registrar';
    const modal = new bootstrap.Modal(document.getElementById('modalNuevaClinica'));
    modal.show();
  };
  
  // Mostrar modal para editar clínica
  const editarClinica = (clinica) => {
    clinicaForm.value = { ...clinica }; // Rellenar el formulario con los datos de la clínica
    modalTitle.value = 'Editar Clínica';
    modalButtonText.value = 'Actualizar';
    const modal = new bootstrap.Modal(document.getElementById('modalNuevaClinica'));
    modal.show();
  };
  
  // Guardar clínica (Crear o actualizar)
    const guardarClinica = async () => {
        try {
            const payload = {
            rnc: clinicaForm.value.rnc.toString(),
            nombre: clinicaForm.value.nombre,
            telefono: clinicaForm.value.telefono,
            direccion: clinicaForm.value.direccion,
            correo: clinicaForm.value.correo,
            horario: clinicaForm.value.horario
            };

            if (clinicaForm.value.id) {
            await updateClinica({ ...payload, id: clinicaForm.value.id });
            } else {
            await createClinica(payload);
            }
            
            await cargarClinicas();
            bootstrap.Modal.getInstance(document.getElementById('modalNuevaClinica')).hide();
        } catch (error) {
            alert(`Error: ${error.message}`);
            console.error('Detalles del error:', error);
        }
    };
  
  // Eliminar clínica
  const eliminarClinica = async (id) => {
    if (confirm('¿Estás seguro que deseas eliminar esta clínica?')) {
      try {
        await deleteClinica(id);
        await cargarClinicas();
      } catch (error) {
        console.error('Error al eliminar clínica:', error);
      }
    }
  };
  </script>
  