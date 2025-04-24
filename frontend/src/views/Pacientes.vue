<template>
    <div class="pacientes-container container py-5">
      <div class="text-center mb-5">
        <i class="fa-solid fa-user-injured fa-3x text-primary mb-3"></i>
        <h2 class="fw-bold">Gestión de Pacientes</h2>
        <p class="text-muted">Administra los pacientes del sistema.</p>
        <button class="btn btn-success mt-2" @click="mostrarModalNuevoPaciente">
          <i class="fa-solid fa-plus me-2"></i> Nuevo Paciente
        </button>
      </div>
  
      <div class="table-responsive">
        <table class="table table-hover">
          <thead>
            <tr>
              <th>ID</th>
              <th>Nombre</th>
              <th>Apellido</th>
              <th>Teléfono</th>
              <th>Dirección</th>
              <th>Correo</th>
              <th>Acciones</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="paciente in pacientes" :key="paciente.id">
              <td>{{ paciente.id }}</td>
              <td>{{ paciente.nombre }}</td>
              <td>{{ paciente.apellido || 'N/A' }}</td>
              <td>{{ paciente.telefono }}</td>
              <td>{{ paciente.direccion || 'N/A' }}</td>
              <td>{{ paciente.correo }}</td>
              <td>
                <button class="btn btn-sm btn-outline-secondary me-2" @click="editarPaciente(paciente)">
                  <i class="fa-solid fa-pen-to-square"></i>
                </button>
                <button class="btn btn-sm btn-outline-danger" @click="eliminarPaciente(paciente.id)">
                  <i class="fa-solid fa-trash"></i>
                </button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
  
      <!-- Modal creación/edición -->
      <div class="modal fade" id="modalNuevoPaciente" tabindex="-1" aria-hidden="true">
        <div class="modal-dialog modal-lg">
          <div class="modal-content">
            <div class="modal-header">
              <h5 class="modal-title">{{ modalTitle }}</h5>
              <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Cerrar"></button>
            </div>
            <div class="modal-body">
              <form @submit.prevent="guardarPaciente">
                <div class="row mb-3">
                    <div class="col-md-12">
                        <label for="correoUsuario" class="form-label">Usuario Paciente</label>
                        <template v-if="pacienteForm.id">
                        <!-- Mostrar como texto cuando está en modo edición -->
                        <input type="text" class="form-control" :value="correoUsuario" disabled>
                        </template>
                        <template v-else>
                        <!-- Mostrar select solo para creación -->
                        <select 
                            v-model="correoUsuario" 
                            class="form-select" 
                            id="correoUsuario"
                            required
                        >
                            <option value="">Seleccione un usuario paciente...</option>
                            <option 
                            v-for="usuario in usuariosPacientes" 
                            :key="usuario.id" 
                            :value="usuario.correo"
                            >
                            {{ usuario.correo }}
                            </option>
                        </select>
                        </template>
                    </div>
                </div>
                <div class="row">
                  <div class="col-md-6 mb-3">
                    <label for="nombre" class="form-label">Nombre</label>
                    <input type="text" v-model="pacienteForm.nombre" class="form-control" id="nombre" required />
                  </div>
                  <div class="col-md-6 mb-3">
                    <label for="apellido" class="form-label">Apellido</label>
                    <input type="text" v-model="pacienteForm.apellido" class="form-control" id="apellido" />
                  </div>
                </div>
                
                <div class="row">
                  <div class="col-md-6 mb-3">
                    <label for="telefono" class="form-label">Teléfono</label>
                    <input type="tel" v-model="pacienteForm.telefono" class="form-control" id="telefono" required />
                  </div>
                  <div class="col-md-6 mb-3">
                    <label for="direccion" class="form-label">Dirección</label>
                    <input type="text" v-model="pacienteForm.direccion" class="form-control" id="direccion" />
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
  import './Pacientes.css';
  import { onMounted, ref, nextTick } from 'vue';
  import { 
    getAllPacientes, 
    deletePaciente, 
    createPaciente, 
    updatePaciente 
  } from '@/services/PacienteService';
  import { getUsuariosPacientes } from '@/services/UsuarioService';
  
  // Estados
  const pacientes = ref([]);
  const usuariosPacientes = ref([]);
  const correoUsuario = ref('');
  const modalTitle = ref('Registrar Nuevo Paciente');
  const modalButtonText = ref('Registrar');
  const pacienteForm = ref({
    id: null,
    nombre: '',
    apellido: '',
    telefono: '',
    direccion: ''
  });
  
  // Carga inicial
  onMounted(async () => {
    await cargarPacientes();
    await cargarUsuariosPacientes();
  });
  
  const cargarPacientes = async () => {
    try {
      pacientes.value = await getAllPacientes();
    } catch (error) {
      console.error('Error al cargar pacientes:', error);
      alert('Error al cargar pacientes: ' + error.message);
    }
  };
  
  const cargarUsuariosPacientes = async () => {
    try {
      usuariosPacientes.value = await getUsuariosPacientes();
    } catch (error) {
      console.error('Error al cargar usuarios pacientes:', error);
      alert('Error al cargar usuarios pacientes: ' + error.message);
    }
  };
  
  // Mostrar modal de creación
  const mostrarModalNuevoPaciente = () => {
    correoUsuario.value = '';
    pacienteForm.value = { 
      id: null,
      nombre: '',
      apellido: '',
      telefono: '',
      direccion: ''
    };
    modalTitle.value = 'Registrar Nuevo Paciente';
    modalButtonText.value = 'Registrar';
    const modal = new bootstrap.Modal(document.getElementById('modalNuevoPaciente'));
    modal.show();
  };
  
  // Mostrar modal para editar
  const editarPaciente = async (paciente) => {
    // Recargar los usuarios pacientes para asegurar que tenemos los más actualizados
    await cargarUsuariosPacientes();
    
    // Buscar el usuario exacto que coincide con el correo del paciente
    const usuarioExistente = usuariosPacientes.value.find(u => u.correo === paciente.correo);
    
    // Establecer el correoUsuario con el valor exacto del option
    correoUsuario.value = usuarioExistente ? usuarioExistente.correo : paciente.correo;
    
    pacienteForm.value = { 
        id: paciente.id,
        nombre: paciente.nombre,
        apellido: paciente.apellido || '',
        telefono: paciente.telefono,
        direccion: paciente.direccion || ''
    };
    
    modalTitle.value = 'Editar Paciente';
    modalButtonText.value = 'Actualizar';
    
    // Usar nextTick para asegurar que el DOM se ha actualizado
    await nextTick();
    
    const modal = new bootstrap.Modal(document.getElementById('modalNuevoPaciente'));
    modal.show();
    };
  
  // Guardar paciente
  const guardarPaciente = async () => {
    try {
        // Validaciones básicas
        if (!pacienteForm.value.nombre?.trim()) {
        throw new Error('El nombre es requerido');
        }

        if (!pacienteForm.value.telefono?.trim()) {
        throw new Error('El teléfono es requerido');
        }

        // Preparar datos del paciente
        const pacienteData = {
        nombre: pacienteForm.value.nombre.trim(),
        apellido: pacienteForm.value.apellido?.trim() || null,
        telefono: pacienteForm.value.telefono.trim(),
        direccion: pacienteForm.value.direccion?.trim() || null
        };

        // Operación de creación o actualización
        if (pacienteForm.value.id) {
        // Para edición - enviar también el ID
        await updatePaciente(pacienteForm.value.id, {
            ...pacienteData,
            id: pacienteForm.value.id
        });
        } else {
        // Para creación - validar correo
        if (!correoUsuario.value?.trim()) {
            throw new Error('El correo del usuario es requerido');
        }
        await createPaciente(pacienteData, correoUsuario.value.trim());
        }
        
        // Recargar datos y cerrar modal
        await cargarPacientes();
        bootstrap.Modal.getInstance(document.getElementById('modalNuevoPaciente')).hide();
        
        // Mostrar feedback positivo
        
    } catch (error) {
        console.error('Error al guardar paciente:', error);
        alert(`Error: ${error.message}`);
    }
    };
  
  // Eliminar paciente
  const eliminarPaciente = async (id) => {
    if (confirm('¿Estás seguro que deseas eliminar este paciente?')) {
      try {
        await deletePaciente(id);
        await cargarPacientes();
      } catch (error) {
        console.error('Error al eliminar paciente:', error);
        alert('Error al eliminar paciente: ' + error.message);
      }
    }
  };
  </script>