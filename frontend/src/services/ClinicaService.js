import axios from 'axios';
import { API_BASE_URL } from '@/services/api';

export const getAllClinicas = async () => {
  try {
    const res = await axios.get(`${API_BASE_URL}/clinicas`);
    return res.data;
  } catch (error) {
    console.error('Error al cargar clínicas:', error);
    throw error;
  }
};

export const deleteClinica = async (id) => {
  try {
    await axios.delete(`${API_BASE_URL}/clinicas/${id}`);
  } catch (error) {
    console.error('Error al eliminar clínica:', error);
    throw error;
  }
};

export const createClinica = async (clinica) => {
    try {
      const response = await axios.post(`${API_BASE_URL}/clinicas`, clinica);
      return response.data;
    } catch (error) {
      console.error('Error completo:', {
        request: error.config?.data,
        response: error.response?.data
      });
      throw new Error(error.response?.data?.title || 'Error al crear clínica');
    }
  };

export const updateClinica = async (clinica) => {
  try {
    await axios.put(`${API_BASE_URL}/clinicas/${clinica.id}`, clinica);
  } catch (error) {
    console.error('Error al actualizar clínica:', error);
    throw error;
  }
};
