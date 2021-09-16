using PropertyBuilding.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PropertyBuilding.Infrastructure.Data.Configurations
{
    public class PropertyConfiguration : IEntityTypeConfiguration<Property>
    {
        public void Configure(EntityTypeBuilder<Property> builder)
        {
            builder.HasMany(d => d.PropertyImages)
                      .WithOne(d => d.Property)
                      .HasForeignKey(d => d.IdProperty)
                      .OnDelete(DeleteBehavior.NoAction);
            builder.HasMany(d => d.PropertyTraces)
                  .WithOne(d => d.Property)
                  .HasForeignKey(d => d.IdProperty)
                  .OnDelete(DeleteBehavior.NoAction);
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id)
            .HasColumnName("IdProperty");
        }
    }
}
