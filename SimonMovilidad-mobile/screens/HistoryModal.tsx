import React, { useState } from 'react';
import { View, Text, Button, StyleSheet, Dimensions, ActivityIndicator, ScrollView } from 'react-native';
import { Picker } from '@react-native-picker/picker';
import { LineChart } from 'react-native-chart-kit';
import { vehicleService } from '../services/vehicleService';
import { HistoricalDataPoint, VehicleLocation } from '@/models/VehicleModels';

interface Props {
  vehicles: VehicleLocation[];
  onClose: () => void;
}

// Un tipo para controlar qué vista mostramos en la modal
type ViewState = 'filters' | 'loading' | 'charts' | 'noData';

export default function HistoryModal({ vehicles, onClose }: Props) {
  const [selectedVehicleId, setSelectedVehicleId] = useState<string>('');
  const [historyData, setHistoryData] = useState<HistoricalDataPoint[]>([]);
  const [view, setView] = useState<ViewState>('filters');

  const handleSearch = async () => {
    if (!selectedVehicleId) {
      alert("Por favor, selecciona un vehículo.");
      return;
    }
    setView('loading');
    try {
      const endDate = new Date();
      const startDate = new Date();
      startDate.setDate(endDate.getDate() - 2); // Últimas 48 horas

      const data = await vehicleService.getVehicleHistory(selectedVehicleId, startDate.toISOString(), endDate.toISOString());
      if (data.length > 0) {
        setHistoryData(data);
        setView('charts');
      } else {
        setView('noData');
      }
    } catch (error) {
      console.error("Error al buscar el historial:", error);
      alert("No se pudo cargar el historial.");
      setView('filters');
    }
  };

  const handleBackToFilters = () => {
    setHistoryData([]);
    setView('filters');
  };

  // --- PREPARACIÓN DE DATOS PARA LOS GRÁFICOS ---
  const chartLabels = historyData
    .map(d => new Date(d.timeStamp).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' }))
    .filter((_, index) => index % 5 === 0); // Mostramos solo 1 de cada 5 etiquetas para evitar superposición

  const speedData = historyData.map(d => Math.round(d.speedKmH));
  const fuelData = historyData.map(d => d.fuelLevel);

  const renderContent = () => {
    switch (view) {
      case 'loading':
        return <ActivityIndicator size="large" color="#00A99D" />;
      
      case 'noData':
        return (
          <>
            <Text style={styles.infoText}>No se encontró historial para este vehículo en el rango de fechas seleccionado.</Text>
            <Button title="Volver a Buscar" onPress={handleBackToFilters} color="#00A99D" />
          </>
        );

      case 'charts':
        return (
          <ScrollView>
            <Text style={styles.title}>Historial del Vehículo</Text>
            
            <Text style={styles.chartTitle}>Velocidad (km/h)</Text>
            <LineChart
              data={{ labels: chartLabels, datasets: [{ data: speedData }] }}
              width={Dimensions.get('window').width - 80}
              height={220}
              yAxisSuffix=" km/h"
              chartConfig={chartConfig}
              bezier
            />

            <Text style={styles.chartTitle}>Nivel de Combustible (%)</Text>
            <LineChart
              data={{ labels: chartLabels, datasets: [{ data: fuelData, color: (opacity=1) => `rgba(229, 62, 62, ${opacity})` }] }}
              width={Dimensions.get('window').width - 80}
              height={220}
              yAxisSuffix="%"
              chartConfig={{...chartConfig, color: (opacity=1) => `rgba(229, 62, 62, ${opacity})`}}
              bezier
            />
            
            <Button title="Volver a Buscar" onPress={handleBackToFilters} color="#00A99D" />
          </ScrollView>
        );
        
      case 'filters':
      default:
        return (
          <>
            <Text style={styles.title}>Buscar Historial</Text>
            <Picker
              selectedValue={selectedVehicleId}
              onValueChange={(itemValue: string) => setSelectedVehicleId(itemValue)}
              style={styles.picker}
              dropdownIconColor="white"
            >
              <Picker.Item label="-- Selecciona un vehículo --" value="" color="#888" />
              {vehicles.map(v => <Picker.Item key={v.vehicleId} label={v.deviceId} value={v.vehicleId} color="black"/>)}
            </Picker>
            <Button title="Buscar Historial (Últimas 48h)" onPress={handleSearch} color="#00A99D" />
            <View style={{ marginTop: 10 }}>
              <Button title="Cancelar" onPress={onClose} color="#888" />
            </View>
          </>
        );
    }
  };

  return (
    <View style={styles.modalOverlay}>
      <View style={styles.modalContent}>
        {renderContent()}
      </View>
    </View>
  );
}

// --- ESTILOS Y CONFIGURACIÓN ---
const chartConfig = {
  backgroundColor: '#1a1d21',
  backgroundGradientFrom: '#272b30',
  backgroundGradientTo: '#272b30',
  decimalPlaces: 0,
  color: (opacity = 1) => `rgba(0, 169, 157, ${opacity})`,
  labelColor: (opacity = 1) => `rgba(255, 255, 255, ${opacity})`,
  propsForDots: { r: '4', strokeWidth: '2', stroke: '#00A99D' },
};

const styles = StyleSheet.create({
  modalOverlay: { flex: 1, justifyContent: 'center', alignItems: 'center', backgroundColor: 'rgba(0,0,0,0.7)' },
  modalContent: { backgroundColor: '#1a1d21', borderRadius: 10, padding: 20, width: '90%', maxHeight: '80%', elevation: 5 },
  title: { fontSize: 20, fontWeight: 'bold', color: 'white', marginBottom: 20, textAlign: 'center' },
  chartTitle: { fontSize: 16, color: '#a0a0a0', marginTop: 20, marginBottom: 10, textAlign: 'center' },
  picker: { width: '100%', backgroundColor: '#272b30', color: 'white', marginBottom: 20, borderRadius: 8 },
  infoText: { color: 'white', marginVertical: 20, textAlign: 'center', fontSize: 16 },
});