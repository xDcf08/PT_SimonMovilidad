export interface VehicleLocation  {
  vehicleId: string;
  deviceId: string;
  latitude: number;
  longitude: number;
  fuelLevel: number;
}

export interface HistoricalDataPoint {
  timeStamp: string;
  fuelLevel: number;
  speedKmH: number;
}