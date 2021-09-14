using PropertyBuilding.Core.DTOs;

namespace PropertyBuilding.API.Responses
{
    public class StandardResponse<T>
    {
        public T Data { get; set; }
        public MetaDataDto Meta { get; set; }
        public StandardResponse(T data)
        {
            Data = data;
        }
    }
}
