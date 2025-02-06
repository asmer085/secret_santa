import React from 'react';
import { Navbar, Nav, Container } from 'react-bootstrap';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import api from '../services/api';
import logo from '../assets/santa.png';
import '../App.css'

function Navigation() {
  const { user, logout } = useAuth();
  const navigate = useNavigate();

  const handleLogout = async () => {
    try {
      await api.post('/Account/logout');
      logout();
      navigate('/login');
    } catch (error) {
      console.error('Failed to logout:', error);
    }
  };

  if (!user) {
    return null;
  }

  return (
    <Navbar bg="white" expand="lg" className="shadow-sm rounded-bottom">
      <Container>
        <Navbar.Brand>
          <img
            src={logo}
            alt="Logo"
            style={{ height: '40px', width: 'auto' }}
          />
        </Navbar.Brand>

        <p className="lead text-dark fst-italic my-auto mb-0 mb-2">
          Welcome, {user.email}
        </p>

        <Nav className="ms-auto">
          <Nav.Link
            onClick={handleLogout}
            className="text-dark fst-italic fs-5 p-2 rounded hover-effect"
          >
            Logout
          </Nav.Link>
        </Nav>
      </Container>
    </Navbar>
  );
}

export default Navigation;