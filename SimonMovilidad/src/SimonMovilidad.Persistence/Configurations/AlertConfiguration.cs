using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimonMovilidad.Domain.Entities;

namespace SimonMovilidad.Persistence.Configurations
{
    internal sealed class AlertConfiguration : IEntityTypeConfiguration<Alert>
    {
        public void Configure(EntityTypeBuilder<Alert> builder)
        {
            builder.ToTable("alerts");

            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id)
                .HasColumnName("id");

            builder.Property(a => a.AlertType)
                .HasColumnName("alert_type")
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(a => a.Message)
                .HasColumnName("message")
                .HasColumnType("Text");

            builder.Property(a => a.TimeStamp)
                .HasColumnName("timestamp");

            builder.Property(a => a.IsResolved)
                .HasColumnName("is_resolved");
        }
    }
}
