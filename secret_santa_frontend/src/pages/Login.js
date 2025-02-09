import React, { useState } from 'react';
import { Form, Button, Alert } from 'react-bootstrap';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import api from '../services/api';
import '../App.css';

function Login() {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [errors, setErrors] = useState({});
  const { login } = useAuth();
  const navigate = useNavigate();

  const handleSubmit = async (event) => {
    event.preventDefault();
    setErrors({});

    try {
      const response = await api.post('/Account/login', { email, password });
      const userData = response.data;
      login(userData); // Save user data in context
      navigate(userData.role === 'ADMIN' ? '/admin' : '/user');
    } catch (error) {
      setErrors({ form: 'Invalid email or password' });
    }
  };

  return (
    <div className="login-wrapper">
      <div className="login-form-container">
        <h2 className="login-title fst-italic">Login</h2>
        {errors.form && <Alert variant="danger">{errors.form}</Alert>}
        <Form onSubmit={handleSubmit} className="login-form">
          <Form.Group className="mb-3" controlId="formBasicEmail">
            <Form.Label className="text-light fst-italic">Username</Form.Label>
            <Form.Control
              type="text"
              placeholder="Enter username"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
            />
          </Form.Group>

          <Form.Group className="mb-3" controlId="formBasicPassword">
            <Form.Label className="text-light fst-italic">Password</Form.Label>
            <Form.Control
              type="password"
              placeholder="Password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
            />
            <Form.Text className="text-light fst-italic">
              We'll never share your password with anyone else.
            </Form.Text>
          </Form.Group>

          <Button variant="light" type="submit" className="login-button d-block mx-auto fst-italic">
            Login
          </Button>
          <p className="lead text-light fw-bold fst-italic">Don't have an account?</p>
          <a className="lead text-light fst-italic" href="/register">
            Register
          </a>
        </Form>
      </div>
    </div>
  );
}

export default Login;