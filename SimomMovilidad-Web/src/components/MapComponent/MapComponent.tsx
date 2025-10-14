import { MapContainer, Marker, Popup, TileLayer } from 'react-leaflet'
import 'leaflet/dist/leaflet.css';
import type { VehicleLocation } from '../../models/VehicleModels'

interface Props {
  vehicles: VehicleLocation[]
}

export const MapComponent = ( {vehicles} : Props ) => {
  const initalPosition : [number, number] = [4.7110, -74.0721]

  return (
    <MapContainer
      center={initalPosition}
      zoom={6}
      style={{ flexGrow: 1, width: '100%', height: '100%', borderRadius: '8px' }}
    >
      <TileLayer 
        attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
        url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
      />
      {
        vehicles.map( vehicle => (
          <Marker
            key={vehicle.vehicleId}
            position={[vehicle.latitude, vehicle.longitude]}
          >
            <Popup>
              <strong>ID:</strong> {vehicle.deviceId} <br />
              <strong>Combustible:</strong>{vehicle.fuelLevel}%
            </Popup>
          </Marker>
        ))
      }
    </MapContainer>
  )
}
