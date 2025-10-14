using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimonMovilidad.Domain.Entities;
using SimonMovilidad.Domain.Enums;

namespace SimonMovilidad.Persistence.Configurations
{
    internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users");

            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id)
                .HasColumnName("id");

            builder.Property(u => u.Email)
                .IsRequired()
                .HasColumnName("email")
                .HasMaxLength(255);

            builder.HasIndex(u => u.Email).IsUnique();

            builder.Property(u => u.Password)
                .IsRequired()
                .HasColumnName("password_hash")
                .HasMaxLength(255);

            builder.Property(u => u.Role)
                .IsRequired()
                .HasColumnName("role")
                .HasConversion(
                    roleEnum => roleEnum.ToString().ToLower(),
                    roleString => (RoleEnum)Enum.Parse(typeof(RoleEnum), roleString, true))
                .HasMaxLength(50);

            builder.Property(u => u.CreatedAt)
                .HasColumnName("created_at");
        }
    }
}
