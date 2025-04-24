import axios from 'axios';
import { API_BASE_URL } from '@/services/api';

export const getAllCitas = async () => {
  try {
    const res = await axios.get(`${API_BASE_URL}/citas`);
    return res.data;
  } catch (error) {
    console.error('Error al cargar citas:', error);
    throw error;
  }
};

export const getCitaById = async (id) => {
  try {
    const res = await axios.get(`${API_BASE_URL}/citas/${id}`);
    return res.data;
  } catch (error) {
    console.error('Error al cargar cita:', error);
    throw error;
  }
};

export const createCita = async (citaData) => {
  try {
    const response = await axios.post(`${API_BASE_URL}/citas`, {
      fecha: citaData.fecha,
      paciente_id: citaData.pacienteId,
      medico_id: citaData.medicoId,
      clinica_id: citaData.clinicaId
    });
    return response.data;
  } catch (error) {
    console.error('Error al crear cita:', error);
    throw error;
  }
};

export const deleteCita = async (id) => {
  try {
    await axios.delete(`${API_BASE_URL}/citas/${id}`);
  } catch (error) {
    console.error('Error al eliminar cita:', error);
    throw error;
  }
};

// Métodos para el flujo de selección
export const getClinicasParaCitas = async () => {
  try {
    const res = await axios.get(`${API_BASE_URL}/citas/clinicas`);
    return res.data;
  } catch (error) {
    console.error('Error al cargar clínicas:', error);
    throw error;
  }
};

export const getPacientesByClinica = async (clinicaId) => {
  try {
    const res = await axios.get(`${API_BASE_URL}/citas/pacientes-por-clinica/${clinicaId}`);
    return res.data;
  } catch (error) {
    console.error('Error al cargar pacientes por clínica:', error);
    throw error;
  }
};

export const getEspecialidadesByClinica = async (clinicaId) => {
  try {
    const res = await axios.get(`${API_BASE_URL}/citas/especialidades-por-clinica/${clinicaId}`);
    return res.data;
  } catch (error) {
    console.error('Error al cargar especialidades por clínica:', error);
    throw error;
  }
};

export const getMedicosByClinicaAndEspecialidad = async (clinicaId, especialidadId) => {
  try {
    const res = await axios.get(`${API_BASE_URL}/citas/medicos-por-clinica-especialidad/${clinicaId}/${especialidadId}`);
    return res.data;
  } catch (error) {
    console.error('Error al cargar médicos por clínica y especialidad:', error);
    throw error;
  }
};