import { useState } from 'react';
import { useAuth } from '../../hooks/useAuth';
import { AlertIcon } from '../Icons/AlertIcon';
import { AlertsPanel } from '../Alerts/AlertsPanel';

export const Navbar = () => {

  const { logout, role } = useAuth();
  const [isAlertsOpen, setIsAlertsOpen] = useState(false);

  return (
    <nav className="navbar">
      <span className="navbar-brand">Simon Movilidad</span>
      <div className="navbar-actions">
        {role === 'Admin' && (
          <div className="alerts-container">
            <button 
              className="alerts-button" 
              onClick={() => setIsAlertsOpen(!isAlertsOpen)}
            >
              <AlertIcon />
            </button>
            {isAlertsOpen && <AlertsPanel />}
          </div>
        )}
        <button onClick={logout} className="logout-button">Cerrar Sesión</button>
      </div>
    </nav>
  )
}
