using System.Collections.Generic;

namespace Property.Core.Entities
{
    public class Property : Entity
    {
        public Property()
        {
            PropertyImages = new HashSet<PropertyImage>();
            PropertyTraces = new HashSet<PropertyTrace>();
        }
        public string Name { get; set; }
        public string Address { get; set; }
        public double Price { get; set; }
        public string CodeInternal { get; set; }
        public int Year { get; set; }
        public int IdUser { get; set; }
        public int IdOwner { get; set; }
        public virtual User User { get; set; }
        public virtual Owner Owner { get; set; }
        public virtual ICollection<PropertyImage> PropertyImages { get; set; }
        public virtual ICollection<PropertyTrace> PropertyTraces { get; set; }

    }
}
