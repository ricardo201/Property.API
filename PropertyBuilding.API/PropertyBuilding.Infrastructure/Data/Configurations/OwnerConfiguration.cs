using PropertyBuilding.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace PropertyBuilding.Infrastructure.Data.Configurations
{
    public class OwnerConfiguration : IEntityTypeConfiguration<Owner>
    {
        public void Configure(EntityTypeBuilder<Owner> builder)
        {
            builder.HasMany(d => d.Property)
                      .WithOne(d => d.Owner)
                      .HasForeignKey(d => d.IdOwner)
                      .OnDelete(DeleteBehavior.NoAction);
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id)
            .HasColumnName("IdOwner");
        }
    }
}