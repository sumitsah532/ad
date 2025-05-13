import React from 'react';
import { Link, useLocation } from 'react-router-dom';
import { 
  FaTachometerAlt, 
  FaBook, 
  FaUsers, 
  FaShoppingCart, 
  FaTags,
  FaBullhorn,
  FaCog,
  FaSignOutAlt
} from 'react-icons/fa';

const AdminSidebar = () => {
  const location = useLocation();

  const menuItems = [
    { path: '/admin/dashboard', icon: <FaTachometerAlt />, label: 'Dashboard' },
    { path: '/admin/books', icon: <FaBook />, label: 'Books' },
    { path: '/admin/users', icon: <FaUsers />, label: 'Users' },
    { path: '/admin/orders', icon: <FaShoppingCart />, label: 'Orders' },
    { path: '/admin/discounts', icon: <FaTags />, label: 'Discounts' },
    { path: '/admin/promotions', icon: <FaBullhorn />, label: 'Promotions' },
    { path: '/admin/settings', icon: <FaCog />, label: 'Settings' },
    { path: '/admin/add-banner', icon: <FaBullhorn />, label: 'Add Banner Announcement' } // New menu item
  ];

  return (
    <div className="fixed left-0 top-0 h-full w-64 bg-gray-800 text-white shadow-lg">
      <div className="p-4 text-xl font-bold border-b border-gray-700">
        EcommerceBook Admin
      </div>
      <nav className="mt-4">
        <ul>
          {menuItems.map((item) => (
            <li key={item.path}>
              <Link
                to={item.path}
                className={`flex items-center p-4 hover:bg-gray-700 transition-colors ${location.pathname === item.path ? 'bg-gray-700' : ''}`}
              >
                <span className="mr-3">{item.icon}</span>
                {item.label}
              </Link>
            </li>
          ))}
        </ul>
      </nav>
      <div className="absolute bottom-0 w-full p-4 border-t border-gray-700">
        <Link to={'/logout'}>
        
        <button className="flex items-center text-red-400 hover:text-red-300" >
          <FaSignOutAlt className="mr-2" />
          Logout
        </button>
        </Link>
      </div>
    </div>
  );
};

export default AdminSidebar;
