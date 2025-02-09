import axios from 'axios';

const api = axios.create({
  baseURL: 'https://localhost:44394/api',
  headers: {
    'Content-Type': 'application/json',
  },
  withCredentials: true, // Include cookies in requests
  
});

export default api;