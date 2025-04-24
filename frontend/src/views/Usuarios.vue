<template>
    <div class="usuarios-container container py-5">
      <div class="text-center mb-5">
        <i class="fa-solid fa-users fa-3x text-primary mb-3"></i>
        <h2 class="fw-bold">Gestión de Usuarios</h2>
        <p class="text-muted">Administra los usuarios del sistema.</p>
        <button class="btn btn-success mt-2" @click="mostrarModalNuevoUsuario">
          <i class="fa-solid fa-plus me-2"></i> Nuevo Usuario
        </button>
      </div>
  
      <div class="table-responsive">
        <table class="table table-hover">
          <thead>
            <tr>
              <th>ID</th>
              <th>Correo</th>
              <th>Tipo</th>
              <th>Clínica</th>
              <th>Acciones</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="usuario in usuarios" :key="usuario.id">
              <td>{{ usuario.id }}</td>
              <td>{{ usuario.correo }}</td>
              <td>
                <span class="badge" :class="{
                  'bg-primary': usuario.tipo_usuario === 'administrador',
                  'bg-success': usuario.tipo_usuario === 'medico',
                  'bg-info': usuario.tipo_usuario === 'paciente'
                }">
                  {{ usuario.tipo_usuario }}
                </span>
              </td>
              <td>{{ usuario.clinica ? usuario.clinica.nombre : 'N/A' }}</td>
              <td>
                <button class="btn btn-sm btn-outline-secondary me-2" @click="editarUsuario(usuario)">
                  <i class="fa-solid fa-pen-to-square"></i>
                </button>
                <button class="btn btn-sm btn-outline-danger" @click="eliminarUsuario(usuario.id)">
                  <i class="fa-solid fa-trash"></i>
                </button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
  
      <!-- Modal creación/edición -->
      <div class="modal fade" id="modalNuevoUsuario" tabindex="-1" aria-hidden="true">
        <div class="modal-dialog modal-lg">
          <div class="modal-content">
            <div class="modal-header">
              <h5 class="modal-title">{{ modalTitle }}</h5>
              <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Cerrar"></button>
            </div>
            <div class="modal-body">
              <form @submit.prevent="guardarUsuario">
                <div class="row">
                  <div class="col-md-6 mb-3">
                    <label for="correo" class="form-label">Correo</label>
                    <input type="email" v-model="usuarioForm.correo" class="form-control" id="correo" required />
                  </div>
                  <div class="col-md-6 mb-3">
                    <label for="contrasena" class="form-label">Contraseña</label>
                    <input type="password" v-model="usuarioForm.contrasena" class="form-control" id="contrasena" required />
                  </div>
                </div>
                
                <div class="row">
                  <div class="col-md-6 mb-3">
                    <label for="tipo_usuario" class="form-label">Tipo de Usuario</label>
                    <select v-model="usuarioForm.tipo_usuario" class="form-select" id="tipo_usuario" required>
                      <option value="">Seleccione...</option>
                      <option value="administrador">Administrador</option>
                      <option value="medico">Médico</option>
                      <option value="paciente">Paciente</option>
                    </select>
                  </div>
                  <div class="col-md-6 mb-3">
                    <label for="clinica_id" class="form-label">Clínica</label>
                    <select v-model="usuarioForm.clinica_id" class="form-select" id="clinica_id">
                      <option :value="null">Ninguna</option>
                      <option v-for="clinica in clinicas" :key="clinica.id" :value="clinica.id">
                        {{ clinica.nombre }}
                      </option>
                    </select>
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
  import './Usuarios.css';
  import { onMounted, ref } from 'vue';
  import { 
    getAllUsuarios, 
    deleteUsuario, 
    createUsuario, 
    updateUsuario 
  } from '@/services/UsuarioService';
  import { getAllClinicas } from '@/services/ClinicaService';
  
  // Estados
  const usuarios = ref([]);
  const clinicas = ref([]);
  const modalTitle = ref('Registrar Nuevo Usuario');
  const modalButtonText = ref('Registrar');
  const usuarioForm = ref({
    id: null,
    correo: '',
    contrasena: '',
    tipo_usuario: '',
    clinica_id: null
  });
  
  // Carga inicial
  onMounted(async () => {
    await cargarUsuarios();
    await cargarClinicas();
  });
  
  const cargarUsuarios = async () => {
    try {
      usuarios.value = await getAllUsuarios();
    } catch (error) {
      console.error('Error al cargar usuarios:', error);
    }
  };
  
  const cargarClinicas = async () => {
    try {
      clinicas.value = await getAllClinicas();
    } catch (error) {
      console.error('Error al cargar clínicas:', error);
    }
  };
  
  // Mostrar modal de creación
  const mostrarModalNuevoUsuario = () => {
    usuarioForm.value = { 
      id: null, 
      correo: '', 
      contrasena: '', 
      tipo_usuario: '', 
      clinica_id: null 
    };
    modalTitle.value = 'Registrar Nuevo Usuario';
    modalButtonText.value = 'Registrar';
    const modal = new bootstrap.Modal(document.getElementById('modalNuevoUsuario'));
    modal.show();
  };
  
  // Mostrar modal para editar
  const editarUsuario = (usuario) => {
    usuarioForm.value = { 
      id: usuario.id,
      correo: usuario.correo,
      contrasena: '', // No mostramos la contraseña actual
      tipo_usuario: usuario.tipo_usuario,
      clinica_id: usuario.clinica_id
    };
    modalTitle.value = 'Editar Usuario';
    modalButtonText.value = 'Actualizar';
    const modal = new bootstrap.Modal(document.getElementById('modalNuevoUsuario'));
    modal.show();
  };
  
  // Guardar usuario
    const guardarUsuario = async () => {
    try {
        // Validación reforzada del ID para actualización
        const userId = Number(usuarioForm.value.id);
        if (usuarioForm.value.id && (isNaN(userId) || userId <= 0)) {
        throw new Error('ID de usuario no válido');
        }

        // Validaciones básicas del formulario
        if (!usuarioForm.value.correo?.trim()) {
        throw new Error('El correo electrónico es requerido');
        }

        if (!usuarioForm.value.tipo_usuario) {
        throw new Error('El tipo de usuario es requerido');
        }

        // Validación de formato de email
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        if (!emailRegex.test(usuarioForm.value.correo)) {
        throw new Error('El correo electrónico no tiene un formato válido');
        }

        // Construcción del payload con formato snake_case
        const payload = {
        correo: usuarioForm.value.correo.trim(),
        tipo_usuario: usuarioForm.value.tipo_usuario,
        clinica_id: usuarioForm.value.clinica_id || null
        };

        // Manejo de contraseña
        if (!usuarioForm.value.id && !usuarioForm.value.contrasena?.trim()) {
        throw new Error('La contraseña es requerida para nuevos usuarios');
        }

        if (usuarioForm.value.contrasena?.trim()) {
        if (usuarioForm.value.contrasena.length < 6) {
            throw new Error('La contraseña debe tener al menos 6 caracteres');
        }
        payload.contrasena = usuarioForm.value.contrasena;
        }

        // Operación de creación o actualización
        if (usuarioForm.value.id) {
        // Para actualización - asegurar que el ID es numérico
        await updateUsuario({ 
            ...payload,
            id: userId
        });
        } else {
        // Para creación
        await createUsuario(payload);
        }
        
        // Recargar datos y cerrar modal
        await cargarUsuarios();
        bootstrap.Modal.getInstance(document.getElementById('modalNuevoUsuario')).hide();
        
    } catch (error) {
        console.error('Error en guardarUsuario:', {
        timestamp: new Date().toISOString(),
        formData: usuarioForm.value,
        errorDetails: {
            message: error.message,
            response: error.response?.data,
            stack: error.stack
        }
        });
        
        // Mensaje de error mejorado
        let errorMessage = 'Error al guardar el usuario';
        
        if (error.response?.data) {
        // Manejo de errores del backend
        errorMessage = error.response.data.title || error.response.data.message;
        
        if (error.response.data.errors) {
            const validationErrors = Object.entries(error.response.data.errors)
            .map(([field, errors]) => `${field}: ${errors.join(', ')}`)
            .join('\n');
            errorMessage += `\nErrores:\n${validationErrors}`;
        }
        } else {
        // Errores de frontend
        errorMessage = error.message;
        }
        
        alert(errorMessage);
    }
    };
  
  // Eliminar usuario
  const eliminarUsuario = async (id) => {
  if (confirm('¿Estás seguro que deseas eliminar este usuario?')) {
    try {
      await deleteUsuario(id);
      await cargarUsuarios();
    } catch (error) {
      console.error('Error completo:', error);
      alert(`Error: ${error.message || 'Ocurrió un error al eliminar el usuario'}`);
    }
  }
};
  </script>