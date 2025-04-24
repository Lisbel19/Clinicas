import axios from 'axios';
import { API_BASE_URL } from '@/services/api';

export const getAllUsuarios = async () => {
  try {
    const res = await axios.get(`${API_BASE_URL}/usuarios`);
    return res.data;
  } catch (error) {
    console.error('Detalles del error al cargar usuarios:', {
      errorResponse: error.response?.data
    });
    throw error;
  }
};

export const getUsuariosMedicos = async () => {
  try {
    const res = await axios.get(`${API_BASE_URL}/medicos/usuarios-medicos`);
    return res.data;
  } catch (error) {
    console.error('Error al cargar usuarios médicos:', {
      status: error.response?.status,
      data: error.response?.data
    });
    throw new Error('No se pudieron cargar los usuarios médicos');
  }
};

export const getUsuariosPacientes = async () => {
  try {
    const res = await axios.get(`${API_BASE_URL}/pacientes/usuarios-pacientes`);
    return res.data;
  } catch (error) {
    console.error('Error al cargar usuarios pacientes:', {
      status: error.response?.status,
      data: error.response?.data
    });
    throw new Error('No se pudieron cargar los usuarios pacientes');
  }
};

export const deleteUsuario = async (id) => {
  try {
    await axios.delete(`${API_BASE_URL}/usuarios/${id}`);
  } catch (error) {
    console.error('Detalles del error al eliminar usuario:', {
      id: id,
      errorResponse: error.response?.data
    });
    throw error;
  }
};

export const createUsuario = async (usuario) => {
  try {
    const payload = {
      correo: usuario.correo,
      contrasena: usuario.contrasena,
      tipo_usuario: usuario.tipo_usuario,
      clinica_id: usuario.clinica_id
    };
    
    const response = await axios.post(`${API_BASE_URL}/usuarios`, payload);
    return response.data;
  } catch (error) {
    console.error('Detalles del error al crear usuario:', {
      requestData: {
        correo: usuario.correo,
        tipo_usuario: usuario.tipo_usuario,
        clinica_id: usuario.clinica_id
      },
      errorResponse: error.response?.data
    });
    throw new Error(error.response?.data?.title || 'Error al crear usuario');
  }
};

export const updateUsuario = async (usuarioData) => {
    try {
      // Validación crítica del ID
      if (!usuarioData.id || isNaN(usuarioData.id)) {
        throw new Error('ID de usuario no válido');
      }
  
      const payload = {
        correo: usuarioData.correo,
        tipo_usuario: usuarioData.tipo_usuario,
        clinica_id: usuarioData.clinica_id || null
      };
  
      if (usuarioData.contrasena?.trim()) {
        payload.contrasena = usuarioData.contrasena;
      }
  
      const response = await axios.put(
        `${API_BASE_URL}/usuarios/${usuarioData.id}`,
        payload,
        {
          headers: {
            'Content-Type': 'application/json'
          }
        }
      );
      
      return response.data;
    } catch (error) {
      console.error('Error en updateUsuario:', {
        inputData: usuarioData,
        errorDetails: error.response?.data || error.message
      });
      throw error;
    }
  };

export const getUsuarioByCorreo = async (correo) => {
  try {
    const response = await axios.get(`${API_BASE_URL}/usuarios/correo/${correo}`);
    return response.data;
  } catch (error) {
    console.error('Detalles del error al buscar usuario por correo:', {
      correo: correo,
      errorResponse: error.response?.data
    });
    throw error;
  }
};