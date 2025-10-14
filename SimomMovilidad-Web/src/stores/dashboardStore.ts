import { create } from "zustand";
import type { VehicleLocation } from "../models/VehicleModels";
import { persist } from "zustand/middleware";
import { vehicleService } from "../services/vehicleService";

interface DashboardState {
  vehicles: VehicleLocation[];
  isLoading: boolean;
  fetchVehicles: () => Promise<void>;
  updateVehicleLocation: (update: VehicleLocation) => void;
}


export const useDashboardStore = create<DashboardState>()(
  persist(
    (set) => ({
      vehicles: [],
      isLoading: true,
      fetchVehicles: async () => {
        try {
          const liveVehicles = await vehicleService.getLiveVehicles();
          set({vehicles: liveVehicles, isLoading: false})
        } catch (error) {
          set({isLoading: false})
        }
      },
      updateVehicleLocation: (locationUpdate) => {
        set(state => {
          const vehicleIndex = state.vehicles.findIndex(v => v.vehicleId === locationUpdate.vehicleId);
          if (vehicleIndex > -1) {
            const updatedVehicles = [...state.vehicles];
            updatedVehicles[vehicleIndex] = { ...updatedVehicles[vehicleIndex], ...locationUpdate };
            return { vehicles: updatedVehicles };
          }
          return { vehicles: [...state.vehicles, locationUpdate] };
        })
      }
    }), {
      name: 'dashboard-storage'
    }
  )
)