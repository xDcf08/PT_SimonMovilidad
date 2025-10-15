import { useShallow } from "zustand/shallow";
import { useDashboardStore } from "../stores/dashboardStore";

export const useDashboard = () => {
  const { vehicles, isLoading, isHydrated, fetchVehicles, updateVehicleLocation } = useDashboardStore(
    useShallow(state => ({
      vehicles: state.vehicles,
      isLoading: state.isLoading,
      isHydrated: state.isHydrated,
      fetchVehicles: state.fetchVehicles,
      updateVehicleLocation: state.updateVehicleLocation,
    }))
  );

  return {vehicles, isLoading, isHydrated, fetchVehicles, updateVehicleLocation}
}