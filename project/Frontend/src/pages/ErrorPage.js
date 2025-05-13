import React from 'react';
import { FaExclamationTriangle, FaArrowLeft, FaHome } from 'react-icons/fa';

const ErrorPage = ({ 
  title = "Something Went Wrong", 
  message = "We're sorry, but an unexpected error has occurred.",
  error = null 
}) => {
  return (
    <div className="min-h-screen bg-gray-50 flex flex-col items-center justify-center px-4">
      <div className="max-w-lg w-full bg-white rounded-lg shadow-md p-8 text-center">
        <div className="w-16 h-16 bg-red-100 rounded-full flex items-center justify-center mx-auto mb-4">
          <FaExclamationTriangle className="text-red-500 text-2xl" />
        </div>

        <h2 className="text-2xl font-bold text-gray-800 mb-4">{title}</h2>
        <p className="text-gray-600 mb-6">{message}</p>

        <div className="flex flex-col sm:flex-row justify-center space-y-3 sm:space-y-0 sm:space-x-4 mt-6">
          <button
            onClick={() => window.history.back()}
            className="flex items-center justify-center px-4 py-2 bg-gray-200 text-gray-700 rounded-md hover:bg-gray-300 transition-colors"
          >
            <FaArrowLeft className="mr-2" />
            Go Back
          </button>

          <a
            href="/"
            className="flex items-center justify-center px-4 py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700 transition-colors"
          >
            <FaHome className="mr-2" />
            Return Home
          </a>
        </div>
      </div>
    </div>
  );
};

export default ErrorPage;
