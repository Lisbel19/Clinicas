import axios from 'axios';
import { API_BASE_URL } from '@/services/api';

export const getCitasByMedicoEmail = async (email) => {
  try {
    const res = await axios.get(`${API_BASE_URL}/citas/por-correo-medico/${email}`);
    return res.data;
  } catch (error) {
    console.error('Error al cargar citas del médico:', error);
    throw error;
  }
};

export const cancelarCitaMedico = async (id) => {
  try {
    await axios.delete(`${API_BASE_URL}/citas/medico/${id}`);
  } catch (error) {
    console.error('Error al cancelar cita:', error);
    throw error;
  }
};