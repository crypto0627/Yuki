import axios from 'axios';
import { RegisterUser, LoginUser } from '@/api/interface'
const apiClient = axios.create({
  baseURL: '/api',
  headers: {
    'Content-Type': 'application/json',
  },
});

export const registerUser = (email: string, username: string, password: string) => apiClient.post<RegisterUser>('/Account/register', { email, username, password });
export const loginUser = (email: string, password: string) => apiClient.post<LoginUser>('/Account/login', { email, password });
export const logoutUser = () => apiClient.post('/Account/logout');