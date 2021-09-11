using PropertyBuilding.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PropertyBuilding.Infrastructure.Data.Configurations
{
    public class PropertyTraceConfiguration : IEntityTypeConfiguration<PropertyTrace>
    {
        public void Configure(EntityTypeBuilder<PropertyTrace> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id)
            .HasColumnName("IdPropertyTrace");
            builder.HasOne(d => d.Property);
        }
    }
}
