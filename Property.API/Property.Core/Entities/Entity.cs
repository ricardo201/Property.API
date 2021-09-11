using Property.Core.Enumerations;

namespace Property.Core.Entities
{
    public abstract class Entity
    {
        public int Id { get; set; }
        public StatusType Status { get; set; }
    }
}
