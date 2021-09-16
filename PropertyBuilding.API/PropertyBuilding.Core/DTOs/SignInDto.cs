using PropertyBuilding.Core.Enumerations;

namespace PropertyBuilding.Core.DTOs
{
    public class SignInDto : UserDto
    {
        public RoleType? Role { get; set; }
    }
}
