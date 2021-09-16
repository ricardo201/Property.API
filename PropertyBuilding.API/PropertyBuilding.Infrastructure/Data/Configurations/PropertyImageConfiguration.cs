using PropertyBuilding.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PropertyBuilding.Infrastructure.Data.Configurations
{
    public class PropertyImageConfiguration : IEntityTypeConfiguration<PropertyImage>
    {
        public void Configure(EntityTypeBuilder<PropertyImage> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id)
            .HasColumnName("IdPropertyImage");
            builder.HasOne(d => d.Property);
        }
    }
}
