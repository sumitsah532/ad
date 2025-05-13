// services/review.js
import api from './api';
export const getReviewsByBookId = async (bookId) => {
  const response = await api.get(`/reviews/book/${bookId}`);
  return response.data;
};

export const createReview = async (reviewData) => {
  const response = await api.post('/reviews', reviewData);
  return response.data;
};

export const hasUserPurchasedBook = async (userId, bookId) => {
  const response = await api.get(`/orders/has-purchased?userId=${userId}&bookId=${bookId}`);
  return response.data;
};