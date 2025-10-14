namespace SimonMovilidad.Domain.Abstractions.Errors
{
    public static class VehicleError
    {
        public static Error AlreadyExistsByLicensePlate = new("Vehicle.AlreadyExistsByLicensePlate", "Ya existe un vehiculo con esta matricula");
        public static Error DeviceIdAlreadyInUse = new("Vehicle.DeviceIdAlreadyInUse", "El ID del dispositivo ya está en uso");
        public static Error NotFound = new("Vehicle.NotFound", "El vehiculo no fue encontrado");
        public static Error InsufficientData = new("Vehicle.InsufficientData", "No hay datos suficientes para el vehiculo en el rango de tiempo especificado");
    }
}
    