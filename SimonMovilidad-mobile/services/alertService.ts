import type { Alert } from "../models/AlertModels";
import { apiClient } from "./apiClient";


export const alertService = {
  getAlerts: async () : Promise<Alert[]> => {
    return await apiClient.get('/alerts ');
  }
}