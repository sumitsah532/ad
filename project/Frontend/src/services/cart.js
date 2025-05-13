import api from './api';

// ✅ Get the active cart
export const getCart = async () => {
  const response = await api.get('/Cart');
  return response.data;
};

// ✅ Add item to cart
export const addCartItem = async (cartItem) => {
  const response = await api.post('/Cart/items', cartItem); // send as JSON
  return response.data;
};

export const updateCartItem = async (cartItemId, updatedItem) => {
    console.log(cartItemId);
    console.log(updatedItem);
    
    const response = await api.put(`/Cart/items/${cartItemId}`, updatedItem); // send as JSON object
    console.log(response)
    return response.data;
  };
  

// ✅ Remove a specific cart item
export const removeCartItem = async (cartItemId) => {
  const response = await api.delete(`/Cart/items/${cartItemId}`);
  return response.data;
};

// ✅ Clear entire cart
export const clearCart = async () => {
  const response = await api.delete('/Cart/clear');
  return response.data;
};
