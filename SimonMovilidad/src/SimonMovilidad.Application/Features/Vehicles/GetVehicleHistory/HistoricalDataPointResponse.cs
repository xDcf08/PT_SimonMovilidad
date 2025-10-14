namespace SimonMovilidad.Application.Features.Vehicles.GetVehicleHistory
{
    public class HistoricalDataPointResponse
    {
        public DateTime TimeStamp { get; set; }
        public decimal FuelLevel { get; set; }
        public double SpeedKmH { get; set; }
    }
}
