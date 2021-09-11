using System;
using System.Collections.Generic;

namespace Property.Core.Entities
{
    public class Owner : Entity
    {
        public Owner()
        {
            Property = new HashSet<Property>();
        }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Photo { get; set; }
        public DateTime Birthday { get; set; }
        public int IdUser { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Property> Property { get; set; }
    }
}
