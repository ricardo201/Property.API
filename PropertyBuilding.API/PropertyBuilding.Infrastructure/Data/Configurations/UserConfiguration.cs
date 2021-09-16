using PropertyBuilding.Core.Entities;
using PropertyBuilding.Core.Enumerations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace PropertyBuilding.Infrastructure.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasMany(d => d.PropertyImages)
                      .WithOne(d => d.User)
                      .HasForeignKey(d => d.IdUser)
                      .OnDelete(DeleteBehavior.NoAction);
            builder.HasMany(d => d.PropertyTraces)
                  .WithOne(d => d.User)
                  .HasForeignKey(d => d.IdUser)
                  .OnDelete(DeleteBehavior.NoAction);
            builder.HasMany(d => d.Properties)
                  .WithOne(d => d.User)
                  .HasForeignKey(d => d.IdUser)
                  .OnDelete(DeleteBehavior.NoAction);
            builder.HasMany(d => d.Owners)
                  .WithOne(d => d.User)
                  .HasForeignKey(d => d.IdUser)
                  .OnDelete(DeleteBehavior.NoAction);
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id)
            .HasColumnName("IdUser");
            builder.Property(e => e.Role)
            .HasColumnName("Role")
            .HasConversion(
                value => value.ToString(),
                value => (RoleType)(Enum.Parse(typeof(RoleType), value))
                );

        }
    }
}
