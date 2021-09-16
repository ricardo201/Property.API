using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PropertyBuilding.API.Responses;
using PropertyBuilding.Core.DTOs;
using PropertyBuilding.Core.Entities;
using PropertyBuilding.Core.Interfaces;
using PropertyBuilding.Infrastructure.Interfaces;
using System.Threading.Tasks;

namespace PropertyBuilding.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignInController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IEncriptService _encriptService;
        public SignInController(IUserService userService, IMapper mapper, IEncriptService encriptService)
        {
            _userService = userService;
            _mapper = mapper;
            _encriptService = encriptService;
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInDto userDto)
        {
            try
            {
                var user = _mapper.Map<User>(userDto);
                user.Password = _encriptService.GetSHA256(user.Password);
                var signInSuccess = await _userService.Register(user);
                if(!signInSuccess) return StatusCode(500);
                var response = new StandardResponse<SignInDto>(userDto);
                return Ok(response);
            }
            catch
            {
                return StatusCode(500);
            }
        }
    }
}