using System;

namespace PropertyBuilding.Core.DTOs
{
    public class PropertyTraceDto : Dto
    {
        public DateTime? DateSale { get; set; }
        public string Name { get; set; }
        public double Value { get; set; }
        public double Tax { get; set; }
        public int? IdProperty { get; set; }
    }
}
