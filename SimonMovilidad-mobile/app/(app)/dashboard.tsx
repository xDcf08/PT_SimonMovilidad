import { useDashboard } from '@/hooks/useDashboard';
import { onTelemetryUpdate, startSignalRConnection, stopSignalRConnection } from '@/services';
import { useEffect } from 'react';
import { Button, StyleSheet, View } from 'react-native';
import MapView, { Marker, PROVIDER_GOOGLE } from 'react-native-maps';
import { useAuth } from '../../hooks/useAuth';

export default function DashboardScreen() {
  const { vehicles, fetchVehicles, updateVehicleLocation } = useDashboard();
  const { logout } = useAuth();

  useEffect(() => {
    fetchVehicles();
  }, [fetchVehicles]);

  useEffect( () => {
    startSignalRConnection();

    onTelemetryUpdate(updateVehicleLocation)

    return () => {
      stopSignalRConnection();
    }
  },[updateVehicleLocation])

  return (
    <View style={styles.container}>
      <MapView
        style={styles.map}
        provider={PROVIDER_GOOGLE} 
        initialRegion={{
          latitude: 4.7110,    // Centrado en Bogotá
          longitude: -74.0721,
          latitudeDelta: 5,   // Nivel de zoom
          longitudeDelta: 5,
        }}
      >
        {/* Hacemos un bucle sobre los vehículos para crear un marcador para cada uno */}
        {vehicles.map(vehicle => (
          <Marker
            key={vehicle.vehicleId}
            coordinate={{ 
              latitude: vehicle.latitude, 
              longitude: vehicle.longitude 
            }}
            title={vehicle.deviceId}
            description={`Combustible: ${vehicle.fuelLevel}%`}
            pinColor="#00A99D" // Color personalizado para los marcadores
          />
        ))}
      </MapView>
      {/* Botón para cerrar sesión superpuesto en el mapa */}
      <View style={styles.logoutButton}>
        <Button title="Cerrar Sesión" onPress={logout} color="#e53e3e" />
      </View>
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
  },
  map: {
    ...StyleSheet.absoluteFillObject, // Hace que el mapa ocupe toda la pantalla
  },
  logoutButton: {
    position: 'absolute',
    top: 60, // Ajusta la posición desde la parte superior
    right: 15,
  },
});