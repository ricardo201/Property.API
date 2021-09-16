using Microsoft.AspNetCore.Http;

namespace PropertyBuilding.Core.DTOs
{
    public class PropertyImageDto : Dto
    {
        public string File { get; set; }
        public bool? Enabled { get; set; }
        public int? IdProperty { get; set; }
        public IFormFile BlobFile { get; set; }
    }
}
