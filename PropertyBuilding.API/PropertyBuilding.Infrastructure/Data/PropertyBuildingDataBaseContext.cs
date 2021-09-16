using Microsoft.EntityFrameworkCore;
using PropertyBuilding.Core.Entities;
using PropertyBuilding.Infrastructure.Data.Configurations;
using System.Reflection;

namespace PropertyBuilding.Infrastructure.Data
{
    public class PropertyBuildingDataBaseContext : DbContext
    {
        public virtual DbSet<Property> Properties { get; set; }
        public virtual DbSet<Owner> Owners { get; set; }
        public virtual DbSet<PropertyImage> PropertyImages { get; set; }
        public virtual DbSet<PropertyTrace> PropertyTraces { get; set; }
        public virtual DbSet<User> User { get; set; }
        public PropertyBuildingDataBaseContext(DbContextOptions<PropertyBuildingDataBaseContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PropertyConfiguration());
            modelBuilder.ApplyConfiguration(new OwnerConfiguration());
            modelBuilder.ApplyConfiguration(new PropertyImageConfiguration());
            modelBuilder.ApplyConfiguration(new PropertyTraceConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
