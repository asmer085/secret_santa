import React, { useState, useEffect } from 'react';
import { Alert, Card, Container, Row, Col, Button } from 'react-bootstrap';
import { useAuth } from '../context/AuthContext';
import api from '../services/api';
import Navigation from '../components/Navigation';
import santa from '../assets/santa.png';
import WishlistCard from '../components/WishlistCard';
import '../App.css';

function UserPage() {
  const [pair, setPair] = useState('');
  const [error, setError] = useState('');
  const [showWishlist, setShowWishlist] = useState(false);
  const [wishlistType, setWishlistType] = useState(null); // 'view' or 'edit'
  const { user } = useAuth();

  useEffect(() => {
    if (!user?.email) return;

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
        setError('Failed to fetch pair.');
      }
    };

    fetchPair();
  }, [user?.email]);

  return (
    <div className="user-wrapper">
      <Container>
        <Navigation />
      </Container>
      <Container fluid className="mt-5">
        <Row className="align-items-center">
          <Col md={4} className="d-none d-md-block text-center">
            <img src={santa} alt="Santa Left" className="img-fluid santa-float" />
          </Col>
          <Col md={4}>
            <Card className="text-center shadow card-container">
              <Card.Header className="bg-danger text-white">
                <h2 className="mb-0">Your Secret Santa Pair</h2>
              </Card.Header>
              <Card.Body>
                {error && <Alert variant="danger">{error}</Alert>}
                {pair ? (
                  <>
                    <Card.Title className="fs-4">🎁 You are giving a gift to:</Card.Title>
                    <Card.Text className="fs-3 fw-bold text-success">{pair}</Card.Text>
                    <Button
                      variant="danger"
                      className="me-2"
                      onClick={() => {
                        setWishlistType('view');
                        setShowWishlist(true);
                      }}
                    >
                      See Wishlist
                    </Button>
                  </>
                ) : (
                  <Card.Text className="fs-4">No pair assigned yet.</Card.Text>
                )}
              </Card.Body>
              <Card.Footer className="text-muted">Happy gifting! 🎅</Card.Footer>
            </Card>
            <div className="mt-5 text-center">
              <Button
                variant="light"
                size="lg"
                onClick={() => {
                  setWishlistType('edit');
                  setShowWishlist(true);
                }}
              >
                Add to Your Wishlist
              </Button>
            </div>
          </Col>
          <Col md={4} className="d-none d-md-block text-center">
            <img src={santa} alt="Santa Right" className="img-fluid santa-float" />
          </Col>
        </Row>
        {showWishlist && (
          <div className="overlay">
            <div className="wishlist-card-wrapper">
              <WishlistCard
                userEmail={wishlistType === 'view' ? pair : user.email}
                isEditable={wishlistType === 'edit'}
                onClose={() => setShowWishlist(false)}
              />
            </div>
          </div>
        )}
      </Container>
    </div>
  );
}

export default UserPage;
