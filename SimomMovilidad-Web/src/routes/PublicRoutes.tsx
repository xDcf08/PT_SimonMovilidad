import { Navigate, Outlet } from 'react-router-dom';
import { useAuth } from '../hooks/useAuth';

export const PublicRoutes = () => {

  const { isAuthenticated } = useAuth();

  return isAuthenticated ? <Navigate to='/dashboard' replace/> : <Outlet />
}
