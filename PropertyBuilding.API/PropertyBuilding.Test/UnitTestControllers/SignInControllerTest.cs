using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using PropertyBuilding.API.Controllers;
using PropertyBuilding.Core.DTOs;
using PropertyBuilding.Core.Enumerations;
using PropertyBuilding.Core.Services;
using PropertyBuilding.Infrastructure.Services;
using System;
using System.Threading.Tasks;

namespace PropertyBuilding.Test.UnitTestControllers
{
    [TestFixture]
    class SignInControllerTest : TestBase
    {
        [Test]
        public async Task SignInUserSuccess()
        {
            var dataBaseName = Guid.NewGuid().ToString();
            var unitOfWork = CreateUnitOfwork(dataBaseName);
            var mapper = ConfigAutoMapper();
            var encriptService = new EncriptService();
            var configuration = CreateConfiguration();
            var userService = new UserService(configuration, unitOfWork);
            var signInController = new SignInController(userService, mapper, encriptService);
            var signDto = new SignInDto();
            signDto.UserName = "userNameTest";
            signDto.Password = "passwordTest";
            signDto.Role = RoleType.Administrator;
            var response = (await signInController.SignIn(signDto)) as OkObjectResult;
            Assert.IsNotNull(response);
            Assert.AreEqual(200, response.StatusCode);
        }
        [Test]
        public async Task SignInUserFailureForNotPassword()
        {
            var dataBaseName = Guid.NewGuid().ToString();
            var unitOfWork = CreateUnitOfwork(dataBaseName);
            var mapper = ConfigAutoMapper();
            var encriptService = new EncriptService();
            var configuration = CreateConfiguration();
            var userService = new UserService(configuration, unitOfWork);
            var signInController = new SignInController(userService, mapper, encriptService);
            var signDto = new SignInDto();
            signDto.UserName = "userNameTest";            
            signDto.Role = RoleType.Administrator;
            var response = (await signInController.SignIn(signDto) as StatusCodeResult);
            Assert.IsNotNull(response);
            Assert.AreEqual(500, response.StatusCode);
        }
    }
}
