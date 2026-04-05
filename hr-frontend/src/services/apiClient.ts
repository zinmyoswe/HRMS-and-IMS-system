import axios from 'axios';
import type { AxiosInstance } from 'axios';

const API_BASE_URL = 'https://localhost:7192/api';

// Create axios instance with custom config
export const apiClient: AxiosInstance = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
  // Disable SSL certificate validation for development only
  httpsAgent: {
    rejectUnauthorized: false,
  } as any,
});

// Add request interceptor for logging
apiClient.interceptors.request.use((config) => {
  console.log(`📤 API Request: ${config.method?.toUpperCase()} ${config.url}`);
  return config;
});

// Add response interceptor for error handling
apiClient.interceptors.response.use(
  (response) => {
    console.log(`✅ API Response: ${response.status} ${response.statusText}`);
    return response;
  },
  (error) => {
    console.error(`❌ API Error:`, error.response?.status, error.message);
    return Promise.reject(error);
  }
);

export default apiClient;
