import React, { useState, useRef } from 'react';
import { useNavigate } from 'react-router-dom';
import { FaUserPlus, FaImage, FaArrowLeft } from 'react-icons/fa';
import axios from 'axios';

const AddStaffPage = () => {
  const [formData, setFormData] = useState({
    email: '',
    password: '',
    fullName: '',
    role: 'staff', // Default role for new staff
    profileImage: null
  });
  const [previewImage, setPreviewImage] = useState(null);
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);
  const fileInputRef = useRef(null);
  const navigate = useNavigate();

  // Create axios instance with auth token
  const api = axios.create({
    baseURL: 'http://localhost:5098/api',
    headers: {
      'Authorization': `Bearer ${localStorage.getItem('token')}`
    }
  });

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData(prev => ({ ...prev, [name]: value }));
  };

  const handleImageChange = (e) => {
    const file = e.target.files[0];
    if (file) {
      // Validate file type
      const validTypes = ['image/jpeg', 'image/png', 'image/gif'];
      if (!validTypes.includes(file.type)) {
        setError('Please select a valid image file (JPEG, PNG, GIF)');
        return;
      }

      // Validate file size (2MB max)
      if (file.size > 2 * 1024 * 1024) {
        setError('Image size must be less than 2MB');
        return;
      }

      setFormData(prev => ({ ...prev, profileImage: file }));

      // Create preview
      const reader = new FileReader();
      reader.onloadend = () => {
        setPreviewImage(reader.result);
      };
      reader.readAsDataURL(file);
      setError('');
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    setError('');
    
    try {
      const formDataToSend = new FormData();
      formDataToSend.append('email', formData.email);
      formDataToSend.append('password', formData.password);
      formDataToSend.append('fullName', formData.fullName);
      formDataToSend.append('role', formData.role);
      if (formData.profileImage) {
        formDataToSend.append('profileImage', formData.profileImage);
      }

      await api.post('/User', formDataToSend, {
        headers: {
          'Content-Type': 'multipart/form-data'
        }
      });
      
      navigate('/admin/users', { state: { success: 'Staff member added successfully!' } });
    } catch (err) {
      console.error('Error adding staff:', err);
      setError(err.response?.data?.message || 'Failed to add staff member. Please try again.');
    } finally {
      setLoading(false);
    }
  };

  const triggerFileInput = () => {
    fileInputRef.current.click();
  };

  return (
    <div className="container mx-auto px-4 py-6 max-w-3xl">
      <div className="flex items-center mb-6">
        <button 
          onClick={() => navigate(-1)}
          className="mr-4 p-2 rounded-full hover:bg-gray-100"
        >
          <FaArrowLeft className="text-gray-600" />
        </button>
        <h1 className="text-2xl font-bold flex items-center">
          <FaUserPlus className="mr-2" /> Add New Staff Member
        </h1>
      </div>

      {error && (
        <div className="mb-4 p-4 bg-red-100 border border-red-400 text-red-700 rounded">
          {error}
        </div>
      )}

      <div className="bg-white p-6 rounded-lg shadow">
        <form onSubmit={handleSubmit}>
          <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
            {/* Left Column - Profile Image */}
            <div className="flex flex-col items-center">
              <div className="relative mb-4">
                <div 
                  className="w-32 h-32 rounded-full bg-gray-200 flex items-center justify-center overflow-hidden cursor-pointer border-2 border-dashed border-gray-300"
                  onClick={triggerFileInput}
                >
                  {previewImage ? (
                    <img 
                      src={previewImage} 
                      alt="Profile preview" 
                      className="w-full h-full object-cover"
                    />
                  ) : (
                    <div className="text-center p-4">
                      <FaImage className="mx-auto text-gray-400 text-3xl mb-2" />
                      <p className="text-sm text-gray-500">Click to upload photo</p>
                    </div>
                  )}
                </div>
                <input
                  type="file"
                  ref={fileInputRef}
                  onChange={handleImageChange}
                  accept="image/*"
                  className="hidden"
                />
                <p className="text-xs text-gray-500 text-center mt-2">
                  JPG, PNG or GIF (max 2MB)
                </p>
              </div>
            </div>

            {/* Right Column - Form Fields */}
            <div>
              <div className="mb-4">
                <label className="block text-gray-700 mb-2 font-medium" htmlFor="fullName">
                  Full Name *
                </label>
                <input
                  id="fullName"
                  name="fullName"
                  type="text"
                  value={formData.fullName}
                  onChange={handleChange}
                  className="w-full px-4 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500"
                  required
                />
              </div>

              <div className="mb-4">
                <label className="block text-gray-700 mb-2 font-medium" htmlFor="email">
                  Email Address *
                </label>
                <input
                  id="email"
                  name="email"
                  type="email"
                  value={formData.email}
                  onChange={handleChange}
                  className="w-full px-4 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500"
                  required
                />
              </div>

              <div className="mb-4">
                <label className="block text-gray-700 mb-2 font-medium" htmlFor="password">
                  Temporary Password *
                </label>
                <input
                  id="password"
                  name="password"
                  type="password"
                  value={formData.password}
                  onChange={handleChange}
                  className="w-full px-4 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500"
                  required
                  minLength={6}
                  placeholder="At least 6 characters"
                />
              </div>

              <div className="mb-4">
                <label className="block text-gray-700 mb-2 font-medium" htmlFor="role">
                  Role *
                </label>
                <select
                  id="role"
                  name="role"
                  value={formData.role}
                  onChange={handleChange}
                  className="w-full px-4 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500"
                  required
                >
                  <option value="staff">Staff</option>
                  <option value="admin">Administrator</option>
                  <option value="manager">Manager</option>
                </select>
              </div>
            </div>
          </div>

          <div className="mt-8 flex justify-end space-x-4">
            <button
              type="button"
              onClick={() => navigate('/admin/users')}
              className="px-6 py-2 border border-gray-300 rounded-lg text-gray-700 hover:bg-gray-50"
            >
              Cancel
            </button>
            <button
              type="submit"
              disabled={loading}
              className="px-6 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 disabled:opacity-50 flex items-center"
            >
              {loading ? (
                <>
                  <svg className="animate-spin -ml-1 mr-3 h-5 w-5 text-white" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
                    <circle className="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" strokeWidth="4"></circle>
                    <path className="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
                  </svg>
                  Adding...
                </>
              ) : (
                <>
                  <FaUserPlus className="mr-2" /> Add Staff Member
                </>
              )}
            </button>
          </div>
        </form>
      </div>

      <div className="mt-6 p-4 bg-blue-50 rounded-lg">
        <h3 className="font-medium text-blue-800 mb-2">Instructions:</h3>
        <ul className="text-sm text-blue-700 list-disc pl-5 space-y-1">
          <li>All fields marked with * are required</li>
          <li>The staff member will receive an email with instructions to set up their account</li>
          <li>Temporary password should be at least 6 characters long</li>
          <li>Choose appropriate role based on the staff member's responsibilities</li>
        </ul>
      </div>
    </div>
  );
};

export default AddStaffPage;