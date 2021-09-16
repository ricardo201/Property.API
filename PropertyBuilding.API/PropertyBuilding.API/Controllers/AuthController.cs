using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PropertyBuilding.Core.DTOs;
using PropertyBuilding.Core.Entities;
using PropertyBuilding.Core.Interfaces;
using PropertyBuilding.Infrastructure.Interfaces;
using System.Threading.Tasks;

namespace PropertyBuilding.API.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IEncriptService _encriptService;
        public AuthController(IUserService userService, IMapper mapper, IEncriptService encriptService)
        {
            _userService = userService;
            _mapper = mapper;
            _encriptService = encriptService;
        }

        [HttpPost]
        public async Task<IActionResult> Token(UserDto userDto)
        {
            var user = _mapper.Map<User>(userDto);
            user.Password = _encriptService.GetSHA256(user.Password);
            var token = await _userService.Authentication(user);
            if (token != null) return Ok(new { token });
            else return Forbid();
        }
    }
}
