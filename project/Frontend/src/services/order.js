import api from './api';

export const createOrder = async (orderItems) => {
  const response = await api.post('/orders', {
    items: orderItems,
  });
  return response.data;
};

export const getMyOrders = async (orderItems) => {
  const response = await api.get('/Orders/my-orders');
  return response.data;
};

export const getOrderedBooks = async () => {
  try {
    const response = await api.get('/Orders/order-books');  
    return response;
  
  } catch (error) {
    console.error('Error fetching ordered books:', error);
    throw error;
  }
};