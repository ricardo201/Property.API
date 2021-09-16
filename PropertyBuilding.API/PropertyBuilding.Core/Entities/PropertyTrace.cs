using System;

namespace PropertyBuilding.Core.Entities
{
    public class PropertyTrace : Entity
    {
        public DateTime DateSale { get; set; }
        public string Name { get; set; }
        public double Value { get; set; }
        public double Tax { get; set; }
        public int IdUser { get; set; }
        public int IdProperty { get; set; }
        public virtual User User { get; set; }
        public virtual Property Property { get; set; }
    }
}
