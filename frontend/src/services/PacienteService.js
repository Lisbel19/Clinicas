import axios from 'axios';
import { API_BASE_URL } from '@/services/api';

export const getAllPacientes = async () => {
  try {
    const res = await axios.get(`${API_BASE_URL}/pacientes`);
    return res.data;
  } catch (error) {
    console.error('Error al cargar pacientes:', error.response?.data || error.message);
    throw error;
  }
};

export const deletePaciente = async (id) => {
  try {
    await axios.delete(`${API_BASE_URL}/pacientes/${id}`);
  } catch (error) {
    console.error('Error al eliminar paciente:', error.response?.data || error.message);
    throw error;
  }
};

export const createPaciente = async (pacienteData, correoUsuario) => {
  try {
    const requestData = {
      nombre: pacienteData.nombre,
      apellido: pacienteData.apellido || null,
      telefono: pacienteData.telefono,
      direccion: pacienteData.direccion || null,
      correo: correoUsuario // Asegúrate que el backend espera esto
    };

    const response = await axios.post(
      `${API_BASE_URL}/pacientes`,
      requestData,
      {
        params: {
          correoUsuario: correoUsuario
        }
      }
    );
    return response.data;
  } catch (error) {
    console.error('Error al crear paciente:', {
      request: {
        data: {
          nombre: pacienteData.nombre,
          apellido: pacienteData.apellido,
          telefono: pacienteData.telefono,
          direccion: pacienteData.direccion,
          correo: correoUsuario
        },
        params: { correoUsuario }
      },
      response: error.response?.data
    });
    throw new Error(error.response?.data?.message || 'Error al crear paciente');
  }
};

export const updatePaciente = async (id, pacienteData) => {
    try {
      const payload = {
        id: id, // Asegurar que el ID coincide
        nombre: pacienteData.nombre,
        apellido: pacienteData.apellido || null,
        telefono: pacienteData.telefono,
        direccion: pacienteData.direccion || null
      };
  
      const response = await axios.put(
        `${API_BASE_URL}/pacientes/${id}`,
        payload
      );
      return response.data;
    } catch (error) {
      console.error('Error al actualizar paciente:', error.response?.data || error.message);
      throw error;
    }
  };