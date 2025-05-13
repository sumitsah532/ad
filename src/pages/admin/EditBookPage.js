import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { FaSave } from 'react-icons/fa';
import { getAllBooks, getBookDetails, updateBookAdmin } from '../../services/books';

const EditBookPage = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const [loading, setLoading] = useState(true);
  const [updating, setUpdating] = useState(false);

  const [book, setBook] = useState({
    title: '',
    author: '',
    isbn: '',
    price: '',
    stockQuantity: '',
    isOnSale: false,
    saleStartDate: '',
    saleEndDate: '',
    salePrice: '',
    category: '',
    bookImageUrl: '',
    tags: ''
  });

  const [imageFile, setImageFile] = useState(null);
  const [previewUrl, setPreviewUrl] = useState('');

  useEffect(() => {
    const fetchBook = async () => {
      try {
        const data = await getBookDetails(id);
        setBook({
          title: data.title || '',
          author: data.author || '',
          isbn: data.isbn || '',
          price: data.price || '',
          stockQuantity: data.stockQuantity || '',
          isOnSale: data.isOnSale || false,
          saleStartDate: data.saleStartDate ? data.saleStartDate.slice(0, 10) : '',
          saleEndDate: data.saleEndDate ? data.saleEndDate.slice(0, 10) : '',
          salePrice: data.salePrice || '',
          category: data.category || '',
          bookImageUrl: data.bookImageUrl || '',
          tags: data.tags ? data.tags.join(', ') : ''
        });
        setPreviewUrl(data.bookImageUrl || '');
        setLoading(false);
      } catch (error) {
        console.error('Error fetching book details:', error);
      }
    };

    fetchBook();
  }, [id]);

  const handleChange = (e) => {
    const { name, value, type, checked } = e.target;
    setBook(prev => ({
      ...prev,
      [name]: type === 'checkbox' ? checked : value
    }));
  };

  const handleImageChange = (e) => {
    const file = e.target.files[0];
    setImageFile(file);

    if (file) {
      const reader = new FileReader();
      reader.onloadend = () => {
        setPreviewUrl(reader.result);
      };
      reader.readAsDataURL(file);
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setUpdating(true);
  
    // Create FormData to handle both regular fields and file upload
    const formData = new FormData();
  
    // Append all book data fields
    formData.append('title', book.title);
    formData.append('author', book.author);
    formData.append('isbn', book.isbn);
    formData.append('price', book.price);
    formData.append('stockQuantity', book.stockQuantity);
    formData.append('isOnSale', book.isOnSale);
    formData.append('category', book.category);
    formData.append('tags', book.tags.split(',').map(tag => tag.trim()).join(','));
  
    // Handle sale information if on sale
    if (book.isOnSale) {
      formData.append('salePrice', book.salePrice);
      formData.append('saleStartDate', book.saleStartDate);
      formData.append('saleEndDate', book.saleEndDate);
    }
  
    // Handle image - either append the new file or the existing URL
    if (imageFile) {
      formData.append('BookImage', imageFile);
    } else {
      formData.append('BookImageUrl', previewUrl || book.bookImageUrl);
    }
  
    try {
      const response = await updateBookAdmin(id, formData)
      // const response = await axios.put(`/api/book/${id}`, formData, {
      //   headers: {
      //     'Authorization': `Bearer ${localStorage.getItem('token')}`,
      //     'Content-Type': 'multipart/form-data'
      //   }
      // });
  
      if (response.status === 200) {
        alert('Book updated successfully!');
        // Refresh book list after successful update
        const updatedBooks = await getAllBooks();
        // You might want to update your state with updatedBooks here
        navigate('/admin/manage-books');
      } else {
        throw new Error(response.data.message || 'Failed to update book');
      }
    } catch (error) {
      console.error('Error updating book:', error);
      alert(error.response?.data?.message || 'Failed to update book. Please try again.');
    } finally {
      setUpdating(false);
    }
  };
  
  if (loading) return <div className="text-center p-4">Loading book info...</div>;

  return (
    <div className="max-w-3xl mx-auto p-6 bg-white shadow rounded-lg">
      <h2 className="text-2xl font-bold mb-6">Edit Book</h2>
      <form onSubmit={handleSubmit} className="grid grid-cols-1 gap-4">
        <input name="title" value={book.title} onChange={handleChange} className="border p-2 rounded" placeholder="Title" required />
        <input name="author" value={book.author} onChange={handleChange} className="border p-2 rounded" placeholder="Author" required />
        <input name="isbn" value={book.isbn} onChange={handleChange} className="border p-2 rounded" placeholder="ISBN" required />
        <input name="price" type="number" value={book.price} onChange={handleChange} className="border p-2 rounded" placeholder="Price" required />
        <input name="stockQuantity" type="number" value={book.stockQuantity} onChange={handleChange} className="border p-2 rounded" placeholder="Stock Quantity" required />

        <div className="flex items-center space-x-2">
          <input type="checkbox" name="isOnSale" checked={book.isOnSale} onChange={handleChange} />
          <label>Is On Sale?</label>
        </div>

        {book.isOnSale && (
          <>
            <input type="date" name="saleStartDate" value={book.saleStartDate} onChange={handleChange} className="border p-2 rounded" />
            <input type="date" name="saleEndDate" value={book.saleEndDate} onChange={handleChange} className="border p-2 rounded" />
            <input type="number" name="salePrice" value={book.salePrice} onChange={handleChange} className="border p-2 rounded" placeholder="Sale Price" />
          </>
        )}

        <input name="category" value={book.category} onChange={handleChange} className="border p-2 rounded" placeholder="Category" />

        <div>
          <label className="block font-semibold mb-1">Book Image</label>
          {previewUrl && (
            <img
              src={previewUrl}
              alt="Book Preview"
              className="w-32 h-32 object-cover rounded mb-2 border"
            />
          )}
          <input type="file" accept="image/*" onChange={handleImageChange} className="border p-2 rounded w-full" />
        </div>

        <input name="tags" value={book.tags} onChange={handleChange} className="border p-2 rounded" placeholder="Tags (comma-separated)" />

        <button type="submit" disabled={updating} className="bg-blue-600 text-white py-2 px-4 rounded hover:bg-blue-700 flex items-center">
          <FaSave className="mr-2" />
          {updating ? 'Updating...' : 'Update Book'}
        </button>
      </form>
    </div>
  );
};

export default EditBookPage;
