import * as signalR from '@microsoft/signalr';
import type { VehicleLocation } from '../models/VehicleModels';

const HUB_URL = `${process.env.EXPO_PUBLIC_API_BASE_URL}/hubs/telemetryHub`.replace('/api/v1', '');

const connection = new signalR.HubConnectionBuilder()
                              .withUrl(HUB_URL)
                              .withAutomaticReconnect()
                              .build();


export const startSignalRConnection = async () => {
  try{
    if(connection.state === signalR.HubConnectionState.Disconnected){
      await connection.start();
      console.log("Conectado")
    }
  }catch(error){
    console.error("Error al conectarse", error)
  }
}

export const onTelemetryUpdate = (callBack : (data: VehicleLocation) => void) => {
  console.log("Registrando 'oyente' para el evento 'ReceiveLocationUpdate'")
  connection.on("ReceiveLocationUpdate", callBack);
}

export const stopSignalRConnection = async () => {
  if(connection.state === signalR.HubConnectionState.Connected){
    await connection.stop();
    console.log("Conexión detenida")
  }
}