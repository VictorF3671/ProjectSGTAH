import axios from 'axios';

const axiosInstance = axios.create({
  baseURL: 'http://localhost:8000/api', // Coloque a URL base da sua API
  timeout: 10000, // Opcional: tempo limite da requisição em milissegundos
  headers: {
    'Content-Type': 'application/json',
  },
});


export default axiosInstance;