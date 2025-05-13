import React from 'react';
import { Link } from 'react-router-dom';
import { FaQuestionCircle, FaHome, FaArrowLeft } from 'react-icons/fa';

const NotFoundPage = () => {
  return (
    <div className="min-h-screen bg-gray-50 flex flex-col items-center justify-center px-4">
      <div className="max-w-lg w-full bg-white rounded-lg shadow-md p-8 text-center">
        <div className="w-16 h-16 bg-blue-100 rounded-full flex items-center justify-center mx-auto mb-4">
          <FaQuestionCircle className="text-blue-500 text-2xl" />
        </div>
        
        <h1 className="text-6xl font-bold text-blue-500 mb-2">404</h1>
        
        <h2 className="text-2xl font-bold text-gray-800 mb-4">
          Page Not Found
        </h2>
        
        <p className="text-gray-600 mb-6">
          The page you're looking for doesn't exist or has been moved.
        </p>
        
        <div className="flex flex-col sm:flex-row justify-center space-y-3 sm:space-y-0 sm:space-x-4 mt-6">
          <button
            onClick={() => window.history.back()}
            className="flex items-center justify-center px-4 py-2 bg-gray-200 text-gray-700 rounded-md hover:bg-gray-300 transition-colors"
          >
            <FaArrowLeft className="mr-2" />
            Go Back
          </button>
          
          <Link
            to="/"
            className="flex items-center justify-center px-4 py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700 transition-colors"
          >
            <FaHome className="mr-2" />
            Return Home
          </Link>
        </div>
      </div>
    </div>
  );
};

export default NotFoundPage;