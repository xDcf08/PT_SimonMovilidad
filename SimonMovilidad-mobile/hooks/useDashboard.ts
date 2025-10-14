import { useShallow } from "zustand/shallow";
import { useDashboardStore } from "../stores/dashboardStore";

export const useDashboard = () => {
  const { vehicles, isLoading, fetchVehicles, updateVehicleLocation } = useDashboardStore(
    useShallow(state => ({
      vehicles: state.vehicles,
      isLoading: state.isLoading,
      fetchVehicles: state.fetchVehicles,
      updateVehicleLocation: state.updateVehicleLocation,
    }))
  );

  return {vehicles, isLoading, fetchVehicles, updateVehicleLocation}
}