import {
  LineChart,
  Line,
  XAxis,
  YAxis,
  CartesianGrid,
  Tooltip,
  Legend,
  ResponsiveContainer,
} from 'recharts';
import './HistoryCharts.css';
import type { HistoricalDataPoint } from '../../models/VehicleModels';

interface Props {
  data: HistoricalDataPoint[];
  onClear: () => void; // Función para volver al mapa
}

export const HistoryCharts = ({ data, onClear }: Props) => {
  // Formateamos la fecha para que se vea bien en el eje X
  const formatXAxis = (tickItem: string) => {
    return new Date(tickItem).toLocaleTimeString();
  };

  console.log(data)

  return (
    <div className="charts-container">
      <div className="charts-header">
        <h2>Historial del Vehículo</h2>
        <button onClick={onClear}>Volver al Mapa</button>
      </div>

      {/* --- GRÁFICO DE VELOCIDAD --- */}
      <h3>Velocidad (km/h)</h3>
      <ResponsiveContainer width="100%" height={300}>
        <LineChart data={data}>
          <CartesianGrid strokeDasharray="3 3" stroke="#4a4a4a" />
          <XAxis dataKey="timeStamp" tickFormatter={formatXAxis} stroke="#a0a0a0" />
          <YAxis stroke="#a0a0a0" />
          <Tooltip contentStyle={{ backgroundColor: '#272b30', border: '1px solid #4a4a4a' }} />
          <Legend />
          <Line type="monotone" dataKey="speedKmH" name="Velocidad" stroke="#00A99D" />
        </LineChart>
      </ResponsiveContainer>

      {/* --- GRÁFICO DE COMBUSTIBLE --- */}
      <h3>Nivel de Combustible (%)</h3>
      <ResponsiveContainer width="100%" height={300}>
        <LineChart data={data}>
          <CartesianGrid strokeDasharray="3 3" stroke="#4a4a4a" />
          <XAxis dataKey="timeStamp" tickFormatter={formatXAxis} stroke="#a0a0a0" />
          <YAxis domain={[0, 100]} stroke="#a0a0a0" />
          <Tooltip contentStyle={{ backgroundColor: '#272b30', border: '1px solid #4a4a4a' }} />
          <Legend />
          <Line type="monotone" dataKey="fuelLevel" name="Combustible" stroke="#e53e3e" />
        </LineChart>
      </ResponsiveContainer>
    </div>
  );
};