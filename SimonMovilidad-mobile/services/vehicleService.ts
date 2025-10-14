import type { HistoricalDataPoint, VehicleLocation } from "../models/VehicleModels";
import { apiClient } from "./apiClient";

export const vehicleService = {
  getLiveVehicles: async() :Promise<VehicleLocation[]> => {
    return await apiClient.get('/vehicles/live');
  },
  getVehicleHistory: async (
    vehicleId:string,
    startTime:string,
    endTime: string
  ) : Promise<HistoricalDataPoint[]> => {
    console.log(endTime)
    return await apiClient.get(`/vehicles/${vehicleId}/history`,{
      params:{
        startTime,
        endTime
      }
    });
  }
}


