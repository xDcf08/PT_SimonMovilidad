export interface VehicleLocation  {
  vehicleId: string;
  deviceId: string;
  latitude: number;
  longitude: number;
  fuelLevel: number;
}

export interface HistoricalDataPoint {
  timestamp: string;
  fuelLevel: number;
  speedKph: number;
}