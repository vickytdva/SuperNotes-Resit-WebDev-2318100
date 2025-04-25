import React, { useState, useEffect } from 'react';
import axios from 'axios';
import Login from './components/Login';
import Notes from './components/Notes';
import Header from './components/Header';
import './App.css';

// Configure axios defaults
axios.defaults.withCredentials = true;

function App() {
  const [user, setUser] = useState(null);
  const [error, setError] = useState('');

  useEffect(() => {
    checkAuth();
  }, []);

  const checkAuth = async () => {
    try {
      const response = await axios.get('https://localhost:7159/auth/check');
      if (response.data.username) {
        setUser({ username: response.data.username });
      }
    } catch (err) {
      console.error('Auth check failed:', err);
      setError('Failed to check authentication status');
    }
  };

  const handleLogin = async (username) => {
    try {
      const response = await axios.post('https://localhost:7159/auth/login', { username });
      if (response.data.message === "Logged in successfully") {
        setUser({ username: response.data.username });
        setError('');
      } else {
        setError('Login failed: ' + response.data.message);
      }
    } catch (err) {
      console.error('Login failed:', err);
      setError('Login failed: ' + (err.response?.data || 'Unknown error'));
    }
  };

  const handleLogout = async () => {
    try {
      await axios.post('https://localhost:7159/auth/logout');
      setUser(null);
      setError('');
    } catch (err) {
      console.error('Logout failed:', err);
      setError('Logout failed');
    }
  };

  return (
    <div className="App">
      <Header user={user} onLogout={handleLogout} />
      {error && <div className="error-message">{error}</div>}
      {user ? (
        <Notes user={user} />
      ) : (
        <Login onLogin={handleLogin} />
      )}
    </div>
  );
}

export default App;
