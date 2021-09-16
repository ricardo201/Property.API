using System;

namespace PropertyBuilding.Core.DTOs
{
    public class OwnerDto : Dto
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Photo { get; set; }
        public DateTime? Birthday { get; set; }
    }
}
