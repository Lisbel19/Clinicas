import axios from 'axios';
import { API_BASE_URL } from '@/services/api';

export const login = async (credentials) => {
    try {
      const res = await axios.post(`${API_BASE_URL}/auth/login`, credentials);
      return res.data;
    } catch (error) {
      console.error('Error en login:', error.response?.data);
      throw new Error(
        error.response?.data?.message || 
        error.response?.data?.title || 
        'Error al iniciar sesión. Por favor verifica tus credenciales.'
      );
    }
  };

export const logout = () => {
  // Limpiar localStorage
  localStorage.removeItem('authToken');
  localStorage.removeItem('userRole');
  localStorage.removeItem('userData');
};

export const getCurrentUser = () => {
  const userData = localStorage.getItem('userData');
  return userData ? JSON.parse(userData) : null;
};