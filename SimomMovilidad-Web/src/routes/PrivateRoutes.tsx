import { Navigate, Outlet } from 'react-router-dom';
import { useAuth } from '../hooks/useAuth';

export const PrivateRoutes = () => {

  const { isAuthenticated } = useAuth();
 
  return isAuthenticated ? <Outlet /> : <Navigate to='/login' replace/>
}