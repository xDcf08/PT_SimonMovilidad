import { useEffect, useState } from "react";
import type {
  HistoricalDataPoint
} from "../../models/VehicleModels";
import {
  onTelemetryUpdate,
  startSignalRConnection,
  stopSignalRConnection,
} from "../../services/signalRService";
import { useDashboard } from "../../hooks/useDashboard";
import { HistoryCharts, MapComponent } from "../../components";
import { DashboardLayout } from "../../Layouts";
import { vehicleService } from "../../services";

export const DashboardPage = () => {

  const { vehicles, isLoading, isHydrated, fetchVehicles, updateVehicleLocation } = useDashboard();
  const [selectedVehicleId, setSelectedVehicleId] = useState<string | null>(null);
  const [startDate, setStartDate] = useState<string>("");
  const [endDate, setEndDate] = useState<string>("");
  const [historyData, setHistoryData] = useState<HistoricalDataPoint[]>([]);
  const [isHistoryLoading, setIsHistoryLoading] = useState<boolean>(false);

  const handleHistorySearch = async () => {
    if (!selectedVehicleId || !startDate || !endDate) {
      alert("Por favor, selecciona un vehículo y un rango de fechas.");
      return;
    }
    
    setIsHistoryLoading(true);
    setHistoryData([]); // Limpiamos los datos anteriores
    
    try {
      const data = await vehicleService.getVehicleHistory(selectedVehicleId, startDate, endDate);
      setHistoryData(data);
    } catch (error) {
      console.error("Error al buscar el historial:", error);
      // alert(error?.name);
    } finally {
      setIsHistoryLoading(false);
    }
  }

  const handleClearHistory = () => {
    setHistoryData([]);
  };

  //Cargar datos iniciales
  useEffect(() => {
    if(isHydrated){
      fetchVehicles();
    }
  }, [fetchVehicles]);

  //Efecto para manejar actualizaciones en tiempo real
  useEffect(() => {
    startSignalRConnection();
    onTelemetryUpdate(updateVehicleLocation);

    return () => {
      stopSignalRConnection();
    };
  }, []);

  const renderContent = () => {
    if(!isHydrated){
      return <p style={messageStyle}>Iniciando aplicación...</p>
    }
    if (isLoading) {
      return <p style={messageStyle}>Cargando datos de la flota...</p>;
    }
    if (isHistoryLoading) {
      return <p style={messageStyle}>Buscando historial del vehículo...</p>;
    }
    if (historyData.length > 0) {
      // Si hay datos históricos, muestra los gráficos
      return <HistoryCharts data={historyData} onClear={handleClearHistory} />;
    }
    if (vehicles.length === 0) {
      return <p style={messageStyle}>No se encontraron vehículos para mostrar en el mapa. 🗺️</p>;
    }
    // Si ninguna de las condiciones anteriores se cumple, muestra el mapa
    return <MapComponent vehicles={vehicles} />;
  };

  return (
    <DashboardLayout 
      vehicles={vehicles} 
      onVehicleSelect={setSelectedVehicleId}
      onStartDateChange={setStartDate}
      onEndDateChange={setEndDate}
      onHistorySearch={handleHistorySearch}
      isHistoryLoading={isHistoryLoading}
    >
      {renderContent()}
    </DashboardLayout>
  );
};

const messageStyle: React.CSSProperties = {
  margin: 'auto',
  textAlign: 'center',
  fontSize: '1.2rem',
  color: 'var(--color-text-secondary)',
};
