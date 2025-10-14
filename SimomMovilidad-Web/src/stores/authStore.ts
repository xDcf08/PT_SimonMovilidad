import { create } from 'zustand';
import { persist } from 'zustand/middleware';
import { authService } from '../services/authService';
import { jwtDecode } from 'jwt-decode';

interface DecodedToken{
  role: string
}

interface AuthState {
  token: string | null;
  isAuthenticated: boolean;
  role: string | null
}

interface AuthActions {
  login: (email: string, password: string) => Promise<{ success: boolean; message?: string }>;
  logout: () => void;
}

type AuthStore = AuthState & AuthActions;

export const useAuthStore = create<AuthStore>()(
  persist(
    (set) => ({
      token: null,
      isAuthenticated: false,
      role: null,
      login: async (email: string, password: string) => {
        try{
          const token = await authService.login(email, password);
          
          const decodedToken = jwtDecode<DecodedToken>(token);
          console.log(decodedToken)
          const useRole = decodedToken.role;

          set({token, isAuthenticated: true, role: useRole});
          return { success: true };
        }catch(error){
          set({token: null, isAuthenticated: false, role: null});
          return { success: false, message: (error as Error).name };
        }
      },
      logout: () => {
        set({token: null, isAuthenticated: false, role: null});
      }
    }),{
      name: 'auth-storage',
    }
  )
)