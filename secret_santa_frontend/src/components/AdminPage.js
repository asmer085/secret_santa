import React, { useState, useEffect } from 'react';
import { Button, Table, Alert } from 'react-bootstrap';
import api from '../services/api'; 

function AdminPage() {
  const [pairs, setPairs] = useState([]);
  const [error, setError] = useState('');

  // Fetch pairs on first load
  useEffect(() => {
    const fetchPairs = async () => {
      try {
        const response = await api.get('/Pairing/getPairs');
        setPairs(response.data);
      } catch (error) {
        setError('Failed to fetch pairs');
      }
    };

    fetchPairs();
  }, []);

  const generatePairs = async () => {
    try {
      const response = await api.post('/Pairing/pairUsers');
      setPairs(response.data);
    } catch (error) {
      setError('Failed to generate pairs');
    }
  };

  return (
    <div>
      <h2>Admin Dashboard</h2>
      {error && <Alert variant="danger">{error}</Alert>}
      <Button onClick={generatePairs} className="mb-3">
        Generate Pairs
      </Button>
      <Table striped bordered hover>
        <thead>
          <tr>
            <th>Giver</th>
            <th>Receiver</th>
          </tr>
        </thead>
        <tbody>
          {pairs.map((pair, index) => (
            <tr key={index}>
              <td>{pair.split(' -> ')[0]}</td>
              <td>{pair.split(' -> ')[1]}</td>
            </tr>
          ))}
        </tbody>
      </Table>
    </div>
  );
}

export default AdminPage;