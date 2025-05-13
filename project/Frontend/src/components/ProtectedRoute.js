import React from 'react';
import { Navigate } from 'react-router-dom';

const ProtectedRoute = ({ children, requiredRole }) => {
  let user = null;
  const token = localStorage.getItem('token');

  try {
    user = JSON.parse(localStorage.getItem('user'));
  } catch (e) {
    localStorage.removeItem('user');
  }

  // If no token or invalid user, redirect to login
  if (!token || !user) {
    return <Navigate to="/login" replace />;
  }

  // If role is required but doesn't match the user role, redirect to home
  if (requiredRole && user.role !== requiredRole) {
    return <Navigate to="/" replace />;
  }

  // All good — render protected content
  return children;
};

export default ProtectedRoute;
