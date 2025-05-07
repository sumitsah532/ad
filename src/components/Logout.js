import { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import './Logout.css';

const Logout = () => {
  const navigate = useNavigate();
  
  useEffect(() => {
    const timeout = setTimeout(() => {
      sessionStorage.clear();
      localStorage.clear();
      navigate('/login');
    }, 2000); // simulate loading
    
    return () => clearTimeout(timeout);
  }, [navigate]);
  
  return (
    <div className="logout-container">
      <div className="logout-content">
        <div className="spinner"></div>
        <p className="logout-message">Clearing session, please wait...</p>
      </div>
    </div>
  );
};

export default Logout;