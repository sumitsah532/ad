import { Link, useNavigate } from 'react-router-dom';
import { FaHome, FaShoppingCart, FaBook, FaBoxOpen } from 'react-icons/fa';
import { FiUser } from 'react-icons/fi';
var isAuthenticated=false;
const user =JSON.parse( localStorage.getItem('user'));
const Navbar = () => {
const details = localStorage.getItem('token')
if (details){
isAuthenticated=true;
}

  const navigate = useNavigate();

  return (
    <nav className="bg-white shadow-md sticky top-0 z-50">
      <div className="container mx-auto px-4">
        <div className="flex justify-between items-center h-16">
          {/* Logo */}
          <div className="flex items-center space-x-4">
            <Link to="/" className="flex items-center">
              <FaBook className="text-blue-600 text-2xl" />
              <span className="ml-2 text-xl font-bold text-gray-800">BookStore</span>
            </Link>
          </div>

          {/* Navigation Links */}
          <div className="hidden md:flex items-center space-x-8">
            <Link 
              to="/" 
              className="flex items-center text-gray-700 hover:text-blue-600 transition-colors"
            >
              <FaHome className="mr-1" /> Home
            </Link>
            
            {isAuthenticated && (
              <Link 
                to="/orders" 
                className="flex items-center text-gray-700 hover:text-blue-600 transition-colors"
              >
                <FaBoxOpen className="mr-1" /> My Orders
              </Link>
            )}

            <Link 
              to="/cart" 
              className="flex items-center text-gray-700 hover:text-blue-600 transition-colors"
            >
              <FaShoppingCart className="mr-1" /> Cart
            </Link>
            <Link 
        to="/wishlist" 
        className="flex items-center text-gray-700 hover:text-blue-600 transition-colors"
      >
        <FaBook className="mr-1" /> Wishlist
      </Link>
          </div>

          {/* User Profile / Login */}
          <div className="flex items-center space-x-4">
            {isAuthenticated ? (
              <div className="flex items-center space-x-4">
                <button 
                  onClick={() => navigate('/profile')}
                  className="flex items-center space-x-1"
                >
                  {user?.avatar ? (
                    <img 
                      src={user.avatar} 
                      alt="Profile" 
                      className="w-8 h-8 rounded-full object-cover"
                    />
                  ) : (
                    <div className="w-8 h-8 rounded-full bg-blue-100 flex items-center justify-center">
                      <FiUser className="text-blue-600" />
                    </div>
                  )}
                  <span className="hidden md:inline text-sm text-gray-700">
                    {user?.name || 'Profile'}
                  </span>
                </button>
              </div>
            ) : (
              <button
                onClick={() => navigate('/login')}
                className="bg-blue-600 hover:bg-blue-700 text-white px-4 py-2 rounded-md transition-colors flex items-center"
              >
                <span>Login</span>
              </button>
            )}
          </div>
        </div>
      </div>
    </nav>
  );
};

export default Navbar;