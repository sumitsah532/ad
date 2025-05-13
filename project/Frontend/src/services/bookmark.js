import api from './api';

// Bookmark Management

// Create a new bookmark
export const createBookmark = async (formData) => {
  try {
    console.log(formData)
     const response = await api.post('/Bookmark', formData, {
      headers: {
        'Content-Type': 'multipart/form-data'
      }
    });
    return response.data;
  } catch (error) {
    console.error('Error creating bookmark:', error);
    throw error;
  }
};

// Get all bookmarks for a specific user
export const getBookmarksByUserId = async (userId,token) => {
  try {
    const response = await api.get(`/Bookmark/user/${userId}`);
    return response.data;
  } catch (error) {
    console.error(`Error fetching bookmarks for user ${userId}:`, error);
    throw error;
  }
};

// Delete a bookmark by its ID
export const deleteBookmark = async (bookmarkId,token) => {
  try {
    const response = await api.delete(`/Bookmark/by-id/${bookmarkId}`);
    return response.data;
  } catch (error) {
    console.error(`Error deleting bookmark ${bookmarkId}:`, error);
    throw error;
  }
};

// Check if a bookmark exists for a user and a book
export const checkBookmarkExists = async (userId, bookId) => {
  try {
    const response = await api.get(`/Bookmark/exists?userId=${userId}&bookId=${bookId}`);
    return response.data;
  } catch (error) {
    console.error(`Error checking if bookmark exists for user ${userId} and book ${bookId}:`, error);
    throw error;
  }
};
