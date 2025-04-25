import React from 'react';

const Header = ({ user, onLogout }) => {
  return (
    <div className="header">
      <h1>Welcome, {user ? user.username : 'Guest'}</h1>
      {user && (
        <button onClick={onLogout} className="logout-btn">
          Logout
        </button>
      )}
    </div>
  );
};

export default Header;
