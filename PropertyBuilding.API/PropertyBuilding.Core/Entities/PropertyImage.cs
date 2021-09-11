namespace PropertyBuilding.Core.Entities
{
    public class PropertyImage : Entity
    {
        public string File { get; set; }
        public bool Enabled { get; set; }
        public int IdUser { get; set; }
        public int IdProperty { get; set; }
        public virtual User User { get; set; }
        public virtual Property Property { get; set; }
    }
}
