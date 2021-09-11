using PropertyBuilding.Core.Enumerations;
using System.Collections.Generic;

namespace PropertyBuilding.Core.Entities
{
    public class User : Entity
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public RoleType Role { get; set; }
        virtual public ICollection<Owner> Owners { get; set; }
        virtual public ICollection<Property> Properties { get; set; }
        virtual public ICollection<PropertyImage> PropertyImages { get; set; }
        virtual public ICollection<PropertyTrace> PropertyTraces { get; set; }

    }
}
