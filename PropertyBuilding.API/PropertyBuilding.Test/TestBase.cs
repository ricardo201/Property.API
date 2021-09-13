using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PropertyBuilding.Core.Enumerations;
using PropertyBuilding.Core.Interfaces;
using PropertyBuilding.Infrastructure.Data;
using PropertyBuilding.Infrastructure.Mappings;
using PropertyBuilding.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PropertyBuilding.Test
{
    public class TestBase
    {
        protected IUnitOfWork CreateUnitOfwork(string dataBaseName)
        {
            var options = new DbContextOptionsBuilder<PropertyBuildingDataBaseContext>()
                .UseInMemoryDatabase(dataBaseName).Options;
            var dataBaseContext = new PropertyBuildingDataBaseContext(options);
            IUnitOfWork unitOfWork = new UnitOfWork(dataBaseContext);
            return unitOfWork;
        }

        protected IMapper ConfigAutoMapper()
        {
            var config = new MapperConfiguration(options =>
            {
                options.AddProfile(new AutoMapperProfile());
            });

            return config.CreateMapper();
        }

        protected IConfiguration CreateConfiguration()
        {
            var authenticationConfiguration = new Dictionary<string, string>
            {
                {"Authentication:SecretKey", "9zQobBuKXo5yzUC-jb-t2nN4tfHCY80xE6mJEOvVdEk$hKJaid"},
                {"Authentication:ValidIssuer", "https://localhost:44373/"},
                {"Authentication:ValidAudience", "https://localhost:44373/"},
                {"Authentication:LifetimeInMinutes", "25"}
            };
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(authenticationConfiguration)
                .Build();

            return configuration;
        }

        protected ClaimsPrincipal CreateUserMock(int idUser, string userName, RoleType roleType)
        {
            var claims = new[]
            {
                new Claim("IdUser", idUser.ToString()),
                new Claim("UserName", userName),
                new Claim(ClaimTypes.Role, roleType.ToString())
            };
            var userMock = new ClaimsPrincipal(new ClaimsIdentity(claims, "TestAuthentication"));
            return userMock;
        }
    }
}
