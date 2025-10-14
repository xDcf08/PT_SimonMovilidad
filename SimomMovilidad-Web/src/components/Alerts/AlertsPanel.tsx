import { useEffect, useState } from "react"
import type { Alert } from "../../models/AlertModels";
import { alertService } from "../../services/alertService";
import './AlertsPanel.css'
import { FuelIcon } from "../Icons/FuelIcon";
import { TempIcon } from "../Icons/TempIcon";

const AlertTypeIcon = ({ alertType }: { alertType: string }) => {
  switch (alertType) {
    case 'PredictiveLowFuel':
      return <FuelIcon />;
    case 'HighTemperature':
      return <TempIcon />;
    default:
      return null; // O un icono genérico
  }
};

export const AlertsPanel  = () => {

  const [alerts, setAlerts] = useState<Alert[]>([]);
  const [isLoading, setIsLoading] = useState(false);

  useEffect(() => {
    const fetchAlerts = async () => {
      try{
        setIsLoading(true)
        const activeAlerts = await alertService.getAlerts();
        setAlerts(activeAlerts);
      }catch(error){
        console.error(error)
      }finally{
        setIsLoading(false);
      }
    }

    fetchAlerts();
  },[]);

  console.log(alerts)

  if (isLoading) return <div className="alerts-panel">Cargando alertas...</div>;

  return (
    <div className="alerts-panel">
      <h4>Alertas Activas</h4>
      {alerts.length === 0 ? (
        <p>No hay alertas activas.</p>
      ) : (
        <ul>
          {alerts.map(alert => (
            <li key={alert.alertId} className="alert-item">
              <div className="alert-icon">
                <AlertTypeIcon alertType={alert.alertType} />
              </div>
              <div className="alert-content">
                <div className="alert-header">
                  <strong>{alert.licensePlate}</strong>
                  <span>{new Date(alert.timestamp).toLocaleTimeString()}</span>
                </div>
                <p className="alert-message">{alert.message}</p>
              </div>
            </li>
          ))}
        </ul>
      )}
    </div>
  )
}
