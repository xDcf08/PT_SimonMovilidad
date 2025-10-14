import type { SidebarProps } from "../../Layouts/Dashboard/DashboardLayout";

export const Sidebar = ({ 
  vehicles, 
  onVehicleSelect,
  onStartDateChange,
  onEndDateChange,
  onHistorySearch,
  isHistoryLoading
}: SidebarProps) => {

  const handleSubmit = (event : React.FormEvent) => {
    event.preventDefault();
    onHistorySearch()
  }

  return (
    <aside className="sidebar">
      <div className="sidebar-header">Menú</div>
      <ul className="sidebar-menu">
        <li className="active">Dashboard</li>
      </ul>

      {/* --- SECCIÓN NUEVA: FILTROS DE HISTORIAL --- */}
      <div className="sidebar-section">
        <div className="sidebar-header">Historial</div>
        <form className="history-form" onSubmit={handleSubmit}>
          <label htmlFor="vehicle-select">Seleccionar Vehículo:</label>
          <select 
            id="vehicle-select" 
            onChange={(e) => onVehicleSelect(e.target.value)}
          >
            <option value="">-- Todos los vehículos --</option>
            {vehicles.map(vehicle => (
              <option key={vehicle.vehicleId} value={vehicle.vehicleId}>
                {vehicle.deviceId} 
              </option>
            ))}
          </select>

          <label htmlFor="start-date">Fecha de Inicio:</label>
          <input type="date" id="start-date" onChange={(e) => onStartDateChange(e.target.value)} />

          <label htmlFor="end-date">Fecha de Fin:</label>
          <input type="date" id="end-date" onChange={(e) => onEndDateChange(e.target.value)} />

          <button type="submit" className="history-button" disabled={isHistoryLoading}>
            {isHistoryLoading ? 'Buscando...' : 'Buscar Historial'}
          </button>
        </form>
      </div>
    </aside>
  );
};