import React, { useState, useEffect, useCallback } from 'react';
import { Card, Button, Form, Alert } from 'react-bootstrap';
import api from '../services/api';
import '../App.css';

function WishlistCard({ userEmail, isEditable, onClose }) {
  const [wishlist, setWishlist] = useState([]);
  const [newItemName, setNewItemName] = useState('');
  const [error, setError] = useState('');

  const fetchWishlist = useCallback(async () => {
    try {
      const response = await api.get(`/Wishlist?userEmail=${userEmail}`);
      setWishlist(response.data);
    } catch (error) {
      setError('Failed to fetch wishlist.');
    }
  }, [userEmail]);

  useEffect(() => {
    fetchWishlist();
  }, [fetchWishlist]);

  const handleAddItem = async () => {
    if (!newItemName) {
      setError('Please enter an item name.');
      return;
    }

    try {
      await api.post('/Wishlist', {
        UserEmail: userEmail,
        ItemName: newItemName,
      });
      setNewItemName('');
      setError('');
      await fetchWishlist();
    } catch (error) {
      setError('Failed to add item to wishlist.');
    }
  };

  const handleDeleteItem = async (id) => {
    try {
      await api.delete(`/Wishlist/${id}`);
      await fetchWishlist();
    } catch (error) {
      setError('Failed to delete item from wishlist.');
    }
  };

  return (
    <Card className="wishlist-card shadow-lg">
      <Card.Header className="bg-danger text-white d-flex justify-content-between align-items-center">
        <h2 className="mb-0">{isEditable ? 'Wishlist' : "Pair's Wishlist"}</h2>
        <Button variant="light" onClick={onClose}>
          &times;
        </Button>
      </Card.Header>
      <Card.Body>
        {error && <Alert variant="danger">{error}</Alert>}
        {isEditable && (
          <Form className="mb-3">
            <Form.Group>
              <Form.Control
                type="text"
                placeholder="Item Name"
                value={newItemName}
                onChange={(e) => setNewItemName(e.target.value)}
              />
            </Form.Group>
            <Button variant="danger" className="mt-2" onClick={handleAddItem}>
              Add Item
            </Button>
          </Form>
        )}
        <div className="d-flex flex-wrap gap-2">
          {wishlist.length > 0 ? (
            wishlist.map((item) => (
              <Button
                key={item.id}
                variant="outline-danger"
                onClick={() => isEditable && handleDeleteItem(item.id)}
              >
                {item.itemName || 'Unnamed Item'}
              </Button>
            ))
          ) : (
            <p className="text-muted">The wishlist is empty.</p>
          )}
        </div>
      </Card.Body>
      <Card.Footer className="text-muted">
        {isEditable ? 'Here you manage your wishlist' : "Here you can view your pair's wishlist"}
      </Card.Footer>
    </Card>
  );
}

export default WishlistCard;
