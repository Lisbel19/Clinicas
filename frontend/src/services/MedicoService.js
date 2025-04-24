import axios from 'axios';
import { API_BASE_URL } from '@/services/api';

export const getAllMedicos = async () => {
  try {
    const res = await axios.get(`${API_BASE_URL}/medicos`);
    return res.data;
  } catch (error) {
    console.error('Error al cargar médicos:', error.response?.data || error.message);
    throw error;
  }
};

export const deleteMedico = async (id) => {
  try {
    await axios.delete(`${API_BASE_URL}/medicos/${id}`);
  } catch (error) {
    console.error('Error al eliminar médico:', error.response?.data || error.message);
    throw error;
  }
};

export const createMedico = async (medico, correoUsuario) => {
    try {
      const payload = {
        nombre: medico.nombre,
        apellido: medico.apellido,
        telefono: medico.telefono,
        horario_consulta: medico.horario_consulta,
        especialidad_id: medico.especialidad_id,
        correo_usuario: correoUsuario
      };
  
      const response = await axios.post(`${API_BASE_URL}/medicos`, payload, {
        params: {
          correoUsuario: correoUsuario
        }
      });
      return response.data;
    } catch (error) {
      console.error('Detalles del error al crear médico:', {
        requestData: payload,
        errorResponse: error.response?.data,
        status: error.response?.status
      });
      throw error;
    }
  };

export const updateMedico = async (medico) => {
  try {
    const response = await axios.put(
      `${API_BASE_URL}/medicos/${medico.id}`,
      medico
    );
    return response.data;
  } catch (error) {
    console.error('Error al actualizar médico:', error.response?.data || error.message);
    throw error;
  }
};