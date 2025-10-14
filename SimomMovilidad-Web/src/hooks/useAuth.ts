import { useShallow } from "zustand/shallow";
import { useAuthStore } from "../stores/authStore"

export const useAuth = () => {
  const { isAuthenticated, token, role,login, logout } = useAuthStore(
  useShallow((state) => ({
    isAuthenticated: state.isAuthenticated,
    token: state.token,
    role: state.role,
    login: state.login,
    logout: state.logout,
  })));

  return { isAuthenticated, token, role,login, logout };
}