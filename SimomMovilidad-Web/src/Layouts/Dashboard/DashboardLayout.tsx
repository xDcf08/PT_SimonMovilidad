import { Navbar } from '../../components/Navbar/Navbar';
import { Sidebar } from '../../components/SideBar/SideBar';
import type { VehicleLocation } from '../../models/VehicleModels';
import './DashboardLayout.css'; // Crearemos este archivo ahora

interface LayoutProps {
  vehicles: VehicleLocation[];
  onVehicleSelect: (vehicleId: string) => void;
  onStartDateChange: (date: string) => void;
  onEndDateChange: (date: string) => void;
  onHistorySearch: () => void;
  isHistoryLoading: boolean;
  children: React.ReactNode;
}

export type SidebarProps = Omit<LayoutProps, 'children'>

export const DashboardLayout = (
  { vehicles, 
    onVehicleSelect, 
    onStartDateChange,
    onEndDateChange,
    onHistorySearch,
    isHistoryLoading,
    children 
  } : LayoutProps
) => {

  return (
    <div className="layout-container">
      <Navbar />
      <div className="main-content">
        <Sidebar 
          vehicles={vehicles} 
          onStartDateChange={onStartDateChange}
          onEndDateChange={onEndDateChange}
          onHistorySearch={onHistorySearch}
          isHistoryLoading={isHistoryLoading}
          onVehicleSelect={onVehicleSelect} 
        />
        <main className="content-area">
          {children}
        </main>
      </div>
    </div>
  );
}