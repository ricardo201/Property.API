using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PropertyBuilding.Core.Entities;
using PropertyBuilding.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PropertyBuilding.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
        }

        public async Task<string> Authentication(User user)
        {
            var validation = await IsValid(user);
            if (validation.Item2)
            {
                var token = GenerateToken(validation.Item1);
                return token;
            }
            return null;
        }
        public async Task<(User, bool)> IsValid(User user)
        {
            var userValid = await GetLoginByCredentials(user);
            return (userValid, userValid != null);
        }
        public async Task<User> GetLoginByCredentials(User user)
        {
            return await _unitOfWork.UserRepository.GetLoginByCredentials(user);
        }

        public string GenerateToken(User user)
        {

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Authentication:SecretKey"]));
            var singninCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var header = new JwtHeader(singninCredentials);
            var claims = new[]
            {
                new Claim("IdUser", user.Id.ToString()),
                new Claim("UserName", user.UserName),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };
            var payload = new JwtPayload
            (
                _configuration["Authentication:ValidIssuer"],
                _configuration["Authentication:ValidAudience"],
                claims,
                DateTime.Now,
                DateTime.UtcNow.AddMinutes(Double.Parse(_configuration["Authentication:LifetimeInMinutes"]))
            );
            var token = new JwtSecurityToken(header, payload);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public void GetUserByToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);
        }

        public async Task<bool> Register(User user)
        {
            try
            {
                await _unitOfWork.UserRepository.AddAsync(user);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch
            {
                ///TODO: Implements logger
                return false;
            }
        }

        public async Task<bool> NotExistUserName(string userName)
        {
            return await _unitOfWork.UserRepository.CountByUserNameAsync(userName) == 0;
        }
    }
}
