import React, { useRef, useState } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';

const AddBookPage = () => {
  const inputClass = "w-full border border-gray-300 rounded px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500";
  const navigate = useNavigate();

  const [formData, setFormData] = useState({
    title: '',
    author: '',
    isbn: '',
    price: '',
    stockQuantity: '',
    category: '',
    isOnSale: false,
    salePrice: '',
    saleStartDate: '',
    saleEndDate: '',
    tags: [],
    bookImage: null,
  });

  const [tagInput, setTagInput] = useState('');
  const [imagePreview, setImagePreview] = useState(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  const fileInputRef = useRef(null);

  const handleChange = (e) => {
    const { name, value, type, checked, files } = e.target;

    if (type === 'checkbox') {
      setFormData((prev) => ({ ...prev, [name]: checked }));
    } else if (type === 'file') {
      const file = files[0];
      if (file) {
        setFormData((prev) => ({ ...prev, [name]: file }));
        setImagePreview(URL.createObjectURL(file));
      }
    } else {
      setFormData((prev) => ({ ...prev, [name]: value }));
    }
  };

  const triggerFileInput = () => {
    fileInputRef.current.click();
  };

  const handleAddTag = () => {
    if (tagInput.trim() !== '') {
      setFormData((prev) => ({
        ...prev,
        tags: [...prev.tags, tagInput.trim()],
      }));
      setTagInput('');
    }
  };

  const handleRemoveTag = (tagToRemove) => {
    setFormData((prev) => ({
      ...prev,
      tags: prev.tags.filter((tag) => tag !== tagToRemove),
    }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    setError('');

    try {
      const formDataToSend = new FormData();

      // Append required fields
      formDataToSend.append('Title', formData.title);
      formDataToSend.append('Author', formData.author);
      formDataToSend.append('ISBN', formData.isbn);
      formDataToSend.append('Price', formData.price);
      formDataToSend.append('StockQuantity', formData.stockQuantity);
      formDataToSend.append('Category', formData.category);
      formDataToSend.append('IsOnSale', formData.isOnSale.toString());

      // Append sale info if on sale
      if (formData.isOnSale) {
        formDataToSend.append('SalePrice', formData.salePrice);
        formDataToSend.append('SaleStartDate', formData.saleStartDate);
        formDataToSend.append('SaleEndDate', formData.saleEndDate);
      }

      // Append tags
      formData.tags.forEach(tag => {
        formDataToSend.append('Tags', tag);
      });

      // Append image if exists
      if (formData.bookImage) {
        formDataToSend.append('BookImage', formData.bookImage);
      }

      // Debug: Log FormData contents
      for (let [key, value] of formDataToSend.entries()) {
        console.log(`${key}:`, value);
      }

      const token = localStorage.getItem('token');
      const response = await axios.post('http://localhost:5098/api/book', formDataToSend, {
        headers: {
          'Authorization': `Bearer ${token}`,
          // Let browser set Content-Type automatically for FormData
        }
      });

      console.log('Book added successfully:', response.data);
      alert('✅ Book added successfully!');
      navigate('/books'); // Redirect after success

    } catch (error) {
      console.error('Error adding book:', error);
      let errorMessage = 'Failed to add book. Please try again.';
      
      if (error.response) {
        if (error.response.status === 401) {
          errorMessage = 'Please login first';
        } else if (error.response.status === 403) {
          errorMessage = 'You need admin privileges to add books';
        } else if (error.response.data?.errors) {
          errorMessage = Object.values(error.response.data.errors).join('\n');
        } else if (error.response.data?.message) {
          errorMessage = error.response.data.message;
        }
      }
      
      setError(errorMessage);
    } finally {
      setLoading(false);
    }
  };


  return (
    <div className="max-w-3xl mx-auto p-6 bg-white shadow-md rounded-lg mt-6">
      <h2 className="text-2xl font-semibold mb-6">Add New Book</h2>
      {error && <div className="text-red-600 mb-4">{error}</div>}
      <form onSubmit={handleSubmit} encType="multipart/form-data" className="space-y-4">
        {/* Title */}
        <input type="text" name="title" placeholder="Title" value={formData.title} onChange={handleChange} className={inputClass} required />

        {/* Author */}
        <input type="text" name="author" placeholder="Author" value={formData.author} onChange={handleChange} className={inputClass} required />

        {/* ISBN */}
        <input type="text" name="isbn" placeholder="ISBN" value={formData.isbn} onChange={handleChange} className={inputClass} required />

        {/* Price */}
        <input type="number" name="price" placeholder="Price" value={formData.price} onChange={handleChange} className={inputClass} required />

        {/* Stock Quantity */}
        <input type="number" name="stockQuantity" placeholder="Stock Quantity" value={formData.stockQuantity} onChange={handleChange} className={inputClass} required />

        {/* Category */}
        <input type="text" name="category" placeholder="Category" value={formData.category} onChange={handleChange} className={inputClass} required />

        {/* Book Image */}
        <div className="space-y-2">
          <button type="button" onClick={triggerFileInput} className="px-4 py-2 bg-indigo-500 text-white rounded hover:bg-indigo-600">
            Upload Image
          </button>
          <input
            type="file"
            name="bookImage"
            accept="image/*"
            onChange={handleChange}
            ref={fileInputRef}
            className="hidden"
          />
          {imagePreview && (
            <img src={imagePreview} alt="Preview" className="w-40 h-40 object-cover border rounded-md mt-2" />
          )}
        </div>

        {/* On Sale */}
        <label className="flex items-center">
          <input type="checkbox" name="isOnSale" checked={formData.isOnSale} onChange={handleChange} className="mr-2" />
          On Sale
        </label>

        {/* Sale Price and Dates */}
        {formData.isOnSale && (
          <>
            <input type="number" name="salePrice" placeholder="Sale Price" value={formData.salePrice} onChange={handleChange} className={inputClass} />
            <label className="block text-sm">Sale Start Date</label>
            <input type="datetime-local" name="saleStartDate" value={formData.saleStartDate} onChange={handleChange} className={inputClass} />

            <label className="block text-sm">Sale End Date</label>
            <input type="datetime-local" name="saleEndDate" value={formData.saleEndDate} onChange={handleChange} className={inputClass} />
          </>
        )}

        {/* Tags */}
        <div>
          <div className="flex items-center space-x-2">
            <input
              type="text"
              placeholder="Add tag"
              value={tagInput}
              onChange={(e) => setTagInput(e.target.value)}
              className={inputClass}
            />
            <button type="button" onClick={handleAddTag} className="px-3 py-2 bg-blue-500 text-white rounded hover:bg-blue-600">Add</button>
          </div>
          <div className="mt-2 flex flex-wrap gap-2">
            {formData.tags.map((tag, index) => (
              <span key={index} className="bg-gray-200 px-3 py-1 rounded-full text-sm flex items-center">
                {tag}
                <button type="button" onClick={() => handleRemoveTag(tag)} className="ml-2 text-red-500">×</button>
              </span>
            ))}
          </div>
        </div>

        {/* Submit */}
        <button type="submit" className={`w-full bg-green-600 text-white py-2 px-4 rounded hover:bg-green-700 ${loading ? 'opacity-50 cursor-not-allowed' : ''}`} disabled={loading}>
          {loading ? 'Adding Book...' : 'Add Book'}
        </button>
      </form>
    </div>
  );
};

export default AddBookPage;
