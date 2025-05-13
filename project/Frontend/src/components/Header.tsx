import React from 'react';
import { Link } from 'react-router-dom';

const Header = () => {
  const isLoggedIn = localStorage.getItem('token');

  return (
    <header className="bg-white shadow-sm">
      <div className="container mx-auto px-4 py-4 flex justify-between items-center">
        <Link to="/" className="text-xl font-bold text-blue-600">EcommerceBook</Link>
        
        <nav className="flex items-center space-x-6">
          <Link to="/" className="hover:text-blue-500">Home</Link>
          {isLoggedIn && (
            <>
              <Link to="/whitelist" className="hover:text-blue-500">Whitelist</Link>
              <Link to="/cart" className="hover:text-blue-500">Cart</Link>
            </>
          )}
          
          {isLoggedIn ? (
            <button 
              onClick={() => {
                localStorage.removeItem('token');
                localStorage.removeItem('user');
                window.location.href = '/login';
              }}
              className="px-4 py-2 bg-red-500 text-white rounded hover:bg-red-600"
            >
              Logout
            </button>
          ) : (
            <Link to="/login" className="px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-600">
              Login
            </Link>
          )}
        </nav>
      </div>
    </header>
  );
};

export default Header;