import AsyncStorage from '@react-native-async-storage/async-storage';
import { jwtDecode } from 'jwt-decode';
import { create } from 'zustand';
import { createJSONStorage, persist } from 'zustand/middleware';
import { authService } from '../services/authService';

interface DecodedToken{
  role: string
}

interface AuthState {
  token: string | null;
  isAuthenticated: boolean;
  role: string | null;
}

interface AuthActions {
  login: (email: string, password: string) => Promise<{ success: boolean; message?: string }>;
  logout: () => void;
}

type AuthStore = AuthState & AuthActions;

export const useAuthStore = create<AuthStore>()(
  persist(
    (set, get) => ({
      token: null,
      isAuthenticated: false,
      role: null,
      login: async (email: string, password: string) => {
        try{
          const token = await authService.login(email, password);

          const decodedToken = jwtDecode<DecodedToken>(token);
          const useRole = decodedToken.role;

          set({token, isAuthenticated: true, role: useRole});
          return { success: true };
        }catch(error: any){
          console.error("Login failed:", error?.response?.data || error.message);
          set({token: null, isAuthenticated: false, role: null});
          return { success: false, message: error?.response?.data?.message || "Error de conexión o ruta inválida", };
        }
      },
      logout: () => {
        set({token: null, isAuthenticated: false, role: null});
      }
    }),{
      name: 'auth-storage',
      storage: createJSONStorage(() => AsyncStorage),
    }
  )
)