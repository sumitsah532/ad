// src/main.jsx or src/index.js
import React from 'react';
import ReactDOM from 'react-dom/client';
import App from './App';
import './index.css'; // <-- this line

ReactDOM.createRoot(document.getElementById('root')).render(
  <React.StrictMode>
    <App />
  </React.StrictMode>,
);
