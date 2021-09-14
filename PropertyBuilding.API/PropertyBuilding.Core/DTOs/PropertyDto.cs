namespace PropertyBuilding.Core.DTOs
{
    public class PropertyDto : Dto
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public double? Price { get; set; }
        public string CodeInternal { get; set; }
        public int? Year { get; set; }
        public int? IdOwner { get; set; }
    }
}