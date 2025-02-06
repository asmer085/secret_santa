import React, { useState, useEffect } from 'react';
import { Table, Alert } from 'react-bootstrap';
import api from '../services/api';
import Navigation from '../components/Navigation';
import WishlistCard from '../components/WishlistCard';

function AdminPage() {
  const [pairs, setPairs] = useState([]);
  const [error, setError] = useState('');
  const [selectedUser, setSelectedUser] = useState(null); // Track selected user
  const [showWishlist, setShowWishlist] = useState(false);

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

  const handleRowClick = (userEmail) => {
    setSelectedUser(userEmail);
    setShowWishlist(true);
  };

  return (
    <div className="container mt-4">
      <Navigation />
      <h2 className="text-center mb-4 mt-3">Admin Dashboard</h2>
      {error && <Alert variant="danger">{error}</Alert>}
      <div className="d-flex justify-content-between align-items-center mb-3">
        <button onClick={generatePairs} className="btn btn-danger">
          Generate New Pairs
        </button>
      </div>
      <Table striped bordered hover responsive className="shadow-sm">
        <thead className="bg-light">
          <tr>
            <th className="text-center">Giver</th>
            <th className="text-center">Receiver</th>
          </tr>
        </thead>
        <tbody>
          {pairs.map((pair, index) => {
            const [giver, receiver] = pair.split(' -> ');
            return (
              <tr
                key={index}
                onClick={() => handleRowClick(giver)}
                className="clickable-row"
              >
                <td className="text-center">{giver}</td>
                <td
                  className="text-center"
                  onClick={(e) => {
                    e.stopPropagation(); // Prevent triggering the row's click event
                    handleRowClick(receiver);
                  }}
                >
                  {receiver}
                </td>
              </tr>
            );
          })}
        </tbody>
      </Table>
      {showWishlist && selectedUser && (
        <div className="overlay">
          <div className="wishlist-card-wrapper">
            <WishlistCard
              userEmail={selectedUser}
              isEditable={true} // Admin can always edit
              onClose={() => setShowWishlist(false)}
            />
          </div>
        </div>
      )}
    </div>
  );
}

export default AdminPage;
