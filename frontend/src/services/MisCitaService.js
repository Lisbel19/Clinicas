import axios from 'axios';
import { API_BASE_URL } from '@/services/api';

export const getCitasByPacienteEmail = async (email) => {
  try {
    const res = await axios.get(`${API_BASE_URL}/citas/por-correo-paciente/${email}`);
    return res.data;
  } catch (error) {
    console.error('Error al cargar citas del paciente:', error);
    throw error;
  }
};

export const cancelarCita = async (id) => {
  try {
    await axios.delete(`${API_BASE_URL}/citas/${id}`);
  } catch (error) {
    console.error('Error al cancelar cita:', error);
    throw error;
  }
};