import React from 'react';
import { Outlet } from 'react-router-dom';
import AdminSidebar from '../components/AdminSidebar'; // adjust path if needed

const AdminLayout = () => {
  return (
    <div style={{ display: 'flex', height: '100vh' }}>
      <AdminSidebar />

      <div style={{ flex: 1, padding: '20px' }}>
        <Outlet />
      </div>
    </div>
  );
};

export default AdminLayout;
