using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimonMovilidad.Domain.Entities;

namespace SimonMovilidad.Persistence.Configurations
{
    internal sealed class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
    {
        public void Configure(EntityTypeBuilder<Vehicle> builder)
        {
            builder.ToTable("vehicles");

            builder.HasKey(v => v.Id);
            builder.Property(v => v.Id).HasColumnName("id");

            builder.Property(v => v.DeviceId)
                .HasColumnName("device_id")
                .IsRequired().HasMaxLength(100);

            builder.HasIndex(v => v.DeviceId).IsUnique();

            builder.Property(v => v.LicensePlate)
                .HasColumnName("license_plate")
                .HasConversion(
                    lp => lp != null ? lp.ToUpper() : null,
                    lp => lp)
                .HasMaxLength(20);

            builder.Property(v => v.AvgConsumption)
                .HasColumnName("avg_consumption")
                .HasColumnType("decimal(5,2)");

            builder.Property(v => v.CreateAt)
                .HasColumnName("created_at");

            builder.HasMany(v => v.SensorReadings)
                .WithOne(sr => sr.Vehicle)
                .HasForeignKey(sr => sr.VehicleId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(v => v.Alerts)
                .WithOne(a => a.Vehicle)
                .HasForeignKey(a => a.VehicleId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
