using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PropertyBuilding.API.Responses
{
    public class StandardResponse<T>
    {
        public T Data { get; set; }
        public StandardResponse(T data)
        {
            Data = data;
        }
    }
}
