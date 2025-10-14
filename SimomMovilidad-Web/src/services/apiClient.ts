import axios, { AxiosError } from "axios";
import { useAuthStore } from "../stores/authStore";

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL || "http://localhost:3000";

export const apiClient = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    "Content-Type": "application/json",
  },
})

apiClient.interceptors.request.use(
  (config) => {
    const token = useAuthStore.getState().token;
    if(token){
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error : AxiosError) => Promise.reject(error)
);

apiClient.interceptors.response.use(
  (response) => response.data,
  (error : AxiosError) => {

    if(error.response?.status === 401){
      useAuthStore.getState().logout();
    }

    return Promise.reject(error.response?.data);
  }
)