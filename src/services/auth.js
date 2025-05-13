import api from './api';

// Authentication
export const login = async (data) => {
  const response = await api.post('/User/login', data);
  return response.data;
};

export const register = async (data) => {
  const response = await api.post('/User', data, {
    headers: {
      'Content-Type': 'multipart/form-data',
    }
  });
  return response.data;
};

// User Management
export const getAllUsers = async () => {
  try {
    const response = await api.get('/User');
    return response.data;
  } catch (error) {
    console.error('Error fetching users:', error);
    throw error;
  }
};

export const getUserById = async (id) => {
  try {
    const response = await api.get(`/User/${id}`);
    return response.data;
  } catch (error) {
    console.error(`Error fetching user ${id}:`, error);
    throw error;
  }
};

export const updateUser = async (id, userData) => {
  try {
    const response = await api.put(`/User/${id}`, userData);
    return response.data;
  } catch (error) {
    console.error(`Error updating user ${id}:`, error);
    throw error;
  }
};

export const deleteUser = async (id) => {
  try {
    const response = await api.delete(`/User/${id}`);
    return response.data;
  } catch (error) {
    console.error(`Error deleting user ${id}:`, error);
    throw error;
  }
};

// Add more user-related API calls as needed