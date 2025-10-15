import { useDashboard } from "@/hooks/useDashboard";
import {
  onTelemetryUpdate,
  startSignalRConnection,
  stopSignalRConnection,
} from "@/services";
import { useEffect, useState } from "react";
import { Button, Modal, StyleSheet, TouchableOpacity, View } from "react-native";
import MapView, { Marker, PROVIDER_GOOGLE } from "react-native-maps";
import { useAuth } from "../../hooks/useAuth";
import HistoryModal from "@/screens/HistoryModal";
import { HistoryIcon } from "@/components/icons/HistoryIcon";

export default function DashboardScreen() {
  const { vehicles, fetchVehicles, updateVehicleLocation } = useDashboard();
  const { logout } = useAuth();
  const [isHistoryModalOpen, setIsHistoryModalOpen] = useState(false);

  useEffect(() => {
    fetchVehicles();
  }, [fetchVehicles]);

  useEffect(() => {
    startSignalRConnection();

    onTelemetryUpdate(updateVehicleLocation);

    return () => {
      stopSignalRConnection();
    };
  }, [updateVehicleLocation]);

  return (
    <View style={styles.container}>
      <MapView
        style={styles.map}
        provider={PROVIDER_GOOGLE}
        initialRegion={{
          latitude: 4.711, // Centrado en Bogotá
          longitude: -74.0721,
          latitudeDelta: 5, // Nivel de zoom
          longitudeDelta: 5,
        }}
      >
        {/* Hacemos un bucle sobre los vehículos para crear un marcador para cada uno */}
        {vehicles.map((vehicle) => (
          <Marker
            key={vehicle.vehicleId}
            coordinate={{
              latitude: vehicle.latitude,
              longitude: vehicle.longitude,
            }}
            title={vehicle.deviceId}
            description={`Combustible: ${vehicle.fuelLevel}%`}
            pinColor="#00A99D" 
          />
        ))}
      </MapView>
      
      <View style={styles.logoutButton}>
        <Button title="Cerrar Sesión" onPress={logout} color="#e53e3e" />
      </View>

      <TouchableOpacity style={styles.historyButton} onPress={() => setIsHistoryModalOpen(true)}>
        <HistoryIcon />
      </TouchableOpacity>

      <Modal
        animationType="slide"
        transparent={true}
        visible={isHistoryModalOpen} // La modal es visible si hay un vehículo seleccionado
        onRequestClose={() => setIsHistoryModalOpen(false)}
      >
        <HistoryModal
          vehicles={vehicles!}
          onClose={() => setIsHistoryModalOpen(false)}
        />
      </Modal>
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
    position: "absolute",
    top: 60, // Ajusta la posición desde la parte superior
    right: 15,
  },
  historyButton: {
    position: 'absolute',
    bottom: 30,
    right: 20,
    backgroundColor: '#00A99D',
    width: 60,
    height: 60,
    borderRadius: 30,
    justifyContent: 'center',
    alignItems: 'center',
    elevation: 8, // Sombra para Android
  },
});
