import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import DashboardPage from './pages/admin/DashboardPage';
import AdminLayout from './layouts/AdminLayout';
import LoginPage from './pages/LoginPage';
import RegisterPage from './pages/RegisterPage';
import ManageBooksPage from './pages/admin/ManageBook';
import ManageUsersPage from './pages/admin/ManageUser';
import AddBookPage from './pages/admin/AddBookPage';
import EditBookPage from './pages/admin/EditBookPage';
import HomePage from './pages/HomePage';
import BookDetailsPage from './pages/BookDetailsPage';
import CartPage from './pages/CartPage';
import AddBannerAnnouncement from './pages/admin/AddBannerAnnouncement';
import Logout from './components/Logout';
import ProtectedRoute from './components/ProtectedRoute';
import WishlistPage from './pages/WishlistPage';
import ErrorBoundary from './ErrorBoundary'; // Adjust the path if needed
import ErrorPage from './pages/ErrorPage';
import NotFoundPage from './pages/NotFoundPage';

function App() {
  return (
    <Router>
      <ErrorBoundary>
        <Routes>
          {/* Public Routes - Accessible without login */}
          <Route path="/" element={<HomePage />} />
          <Route path="/login" element={<LoginPage />} />
          <Route path="/register" element={<RegisterPage />} />
          <Route path="/logout" element={<Logout />} />
          <Route path="/books/:id" element={<BookDetailsPage />} />
          <Route path="/something-went-wrong" element={<ErrorPage />} />

          {/* Protected Routes - Require login */}
          <Route
            path="/cart"
            element={
              <ProtectedRoute>
                <CartPage />
              </ProtectedRoute>
            }
          />
          <Route
            path="/wishlist"
            element={
              <ProtectedRoute>
                <WishlistPage />
              </ProtectedRoute>
            }
          />

          {/* Admin Routes - Require admin role */}
          <Route
            path="/admin"
            element={
              <ProtectedRoute requiredRole="admin">
                <AdminLayout />
              </ProtectedRoute>
            }
          >
            <Route index element={<DashboardPage />} />
            <Route path="dashboard" element={<DashboardPage />} />
            <Route path="books" element={<ManageBooksPage />} />
            <Route path="add-book" element={<AddBookPage />} />
            <Route path="books/edit/:id" element={<EditBookPage />} />
            <Route path="users" element={<ManageUsersPage />} />
            <Route path="add-banner" element={<AddBannerAnnouncement />} />
          </Route>

          {/* Catch-all Route for 404 - Page Not Found */}
          <Route path="*" element={<NotFoundPage />} />
        </Routes>
      </ErrorBoundary>
    </Router>
  );
}

export default App;
