using PropertyBuilding.Core.Enumerations;

namespace PropertyBuilding.Core.Entities
{
    public abstract class Entity
    {
        public int Id { get; set; }
        public StatusType Status { get; set; }
    }
}
