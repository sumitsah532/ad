import api from './api';

// Get paginated books
export const getBooks = async (page = 1, pageSize = 10) => {
  const response = await api.get(`/books?page=${page}&pageSize=${pageSize}`);
  return response.data;
};

// Get all books
export const getAllBooks = async () => {
  const response = await api.get('http://localhost:5098/api/Book');
  return response.data;
};

// Get book details by ID
export const getBookDetails = async (id) => {
  const response = await api.get(`/book/${id}`);
  return response.data;
};

// Search books with filters
export const searchBooks = async (params) => {
  const response = await api.get('/books/search', { params });
  return response.data;
};

// Add new book
export const addBook = async (data) => {
  const response = await api.post('/book', data, {
    headers: {
      'Content-Type': 'multipart/form-data',
      'Authorization': `Bearer ${localStorage.getItem('token')}`
    },
  });
  return response.data;
};

// Update existing book
export const updateBookAdmin = async (id, data) => {
  const response = await api.put(`/book/${id}`, data, {
    headers: {
      'Content-Type': 'multipart/form-data',
      'Authorization': `Bearer ${localStorage.getItem('token')}`
    },
  });
  return response.data;
};

// Delete book by ID
export const deleteBook = async (id) => {
  const response = await api.delete(`/book/${id}`, {
    headers: {
      'Authorization': `Bearer ${localStorage.getItem('token')}`
    },
  });
  return response.data;
};

// Delete book by ID
export const getBookById = async (id) => {
  const response = await api.get(`/book/${id}`, {
   
  });
  return response.data;
};


export const fetchBanner = async () => {
  const response = await api.get(`/BannerAnnouncement/active`);
  return response.data;
};
