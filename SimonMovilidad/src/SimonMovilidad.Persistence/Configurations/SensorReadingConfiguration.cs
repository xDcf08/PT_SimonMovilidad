using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimonMovilidad.Domain.Entities;

namespace SimonMovilidad.Persistence.Configurations
{
    internal sealed class SensorReadingConfiguration : IEntityTypeConfiguration<SensorReading>
    {
        public void Configure(EntityTypeBuilder<SensorReading> builder)
        {
            builder.ToTable("sensor_readings");

            builder.HasKey(sr => sr.Id);
            builder.Property(sr => sr.Id)
                .HasColumnName("id");

            builder.Property(sr => sr.Latitude)
                .HasColumnName("latitude")
                .HasColumnType("decimal(9,6)")
                .IsRequired();

            builder.Property(sr => sr.Longitude)
                .HasColumnName("longitude")
                .HasColumnType("decimal(9,6)")
                .IsRequired();

            builder.Property(sr => sr.FuelLevel)
                .HasColumnName("fuel_level")
                .HasColumnType("decimal(5,2)")
                .IsRequired();

            builder.Property(sr => sr.Temperature)
                .HasColumnName("temperature")
                .HasColumnType("decimal(5,2)");

            builder.Property(sr => sr.TimeStamp)
                .HasColumnName("timestamp");

            builder.Property(sr => sr.VehicleId)
                .HasColumnName("vehicle_id");

            builder.HasIndex(sr => new {sr.VehicleId, sr.TimeStamp})
                .IsDescending(false, true);

        }
    }
}
