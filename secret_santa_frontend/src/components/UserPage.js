import React, { useState, useEffect } from 'react';
import { Alert } from 'react-bootstrap';
import { useAuth } from '../context/AuthContext';
import api from '../services/api'; 

function UserPage() {
  const [pair, setPair] = useState('');
  const [error, setError] = useState('');
  const { user } = useAuth();

  useEffect(() => {
    if (!user?.email) return; // Skip if user.email is not available
  
    const fetchPair = async () => {
      try {
        const response = await api.get('/Pairing/getPairs');
        const data = response.data;
  
        const userPair = data.find((p) =>
          p.toLowerCase().startsWith(`${user.email.toLowerCase()} -> `)
        );
  
        if (userPair) {
          const receiver = userPair.split(' -> ')[1];
          setPair(receiver);
        }
      } catch (error) {
        setError('Failed to fetch pair');
      }
    };
  
    fetchPair();
  }, [user?.email]); // Only re-run if user.email changes

  return (
    <div>
      <h2>Your Secret Santa Pair</h2>
      {error && <Alert variant="danger">{error}</Alert>}
      {pair ? (
        <p>
          You are giving a gift to: <strong>{pair}</strong>
        </p>
      ) : (
        <p>No pair assigned yet.</p>
      )}
    </div>
  );
}

export default UserPage;