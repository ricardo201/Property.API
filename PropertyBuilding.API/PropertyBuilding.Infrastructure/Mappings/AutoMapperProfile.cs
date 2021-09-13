using AutoMapper;
using PropertyBuilding.Core.DTOs;
using PropertyBuilding.Core.Entities;

namespace PropertyBuilding.Infrastructure.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, SignInDto>().ReverseMap();
            CreateMap<Owner, OwnerDto>().ReverseMap();
        }
    }
}
