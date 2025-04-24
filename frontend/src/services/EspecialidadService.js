import axios from 'axios';
import { API_BASE_URL } from '@/services/api';

export const getAllEspecialidades = async () => {
  try {
    const res = await axios.get(`${API_BASE_URL}/especialidades`);
    return res.data;
  } catch (error) {
    console.error('Error al cargar especialidades:', error);
    throw error;
  }
};

export const deleteEspecialidad = async (id) => {
  try {
    await axios.delete(`${API_BASE_URL}/especialidades/${id}`);
  } catch (error) {
    console.error('Error al eliminar especialidad:', error);
    throw error;
  }
};

export const createEspecialidad = async (especialidad) => {
    try {
      const payload = { nombre: especialidad.nombre }; // Solo envía el nombre
      const response = await axios.post(`${API_BASE_URL}/especialidades`, payload);
      return response.data;
    } catch (error) {
      console.error('Detalles del error:', {
        requestData: payload,
        errorResponse: error.response?.data
      });
      throw error;
    }
  };

export const updateEspecialidad = async (especialidad) => {
  try {
    await axios.put(`${API_BASE_URL}/especialidades/${especialidad.id}`, especialidad);
  } catch (error) {
    console.error('Error al actualizar especialidad:', error);
    throw error;
  }
};