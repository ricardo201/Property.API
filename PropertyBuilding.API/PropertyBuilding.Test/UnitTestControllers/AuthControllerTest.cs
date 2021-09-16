using PropertyBuilding.Core.DTOs;
using PropertyBuilding.Core.Entities;
using PropertyBuilding.Core.Interfaces;
using PropertyBuilding.Infrastructure.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using NUnit.Framework;
using System;
using PropertyBuilding.Infrastructure.Services;
using PropertyBuilding.Core.Services;
using PropertyBuilding.API.Controllers;
using PropertyBuilding.Core.Enumerations;

namespace PropertyBuilding.Test.UnitTestControllers
{
    [TestFixture]
    class AuthControllerTest : TestBase
    {
        [Test]
        public async Task AuthUserSuccess()
        {
            var dataBaseName = Guid.NewGuid().ToString();
            var unitOfWork = CreateUnitOfwork(dataBaseName);
            var mapper = ConfigAutoMapper();
            var encriptService = new EncriptService();
            var configuration = CreateConfiguration();
            var userService = new UserService(configuration, unitOfWork);
            var userToRegister = new User { UserName = "TestUserName01", Password = encriptService.GetSHA256("PasswordTestUserRegister01*") , Role = RoleType.User, Status = StatusType.Active };            
            var registerSuccess = await userService.Register(userToRegister);
            Assert.AreEqual(true, registerSuccess);
            var authController = new AuthController(userService, mapper, encriptService);
            var userDto = new UserDto();
            userDto.UserName = "TestUserName01";
            userDto.Password = "PasswordTestUserRegister01*";
            var response = (await authController.Token(userDto)) as OkObjectResult;
            Assert.IsNotNull(response);
            Assert.AreEqual(200, response.StatusCode);
        }

        [Test]
        public async Task AuthUserFailure()
        {
            var dataBaseName = Guid.NewGuid().ToString();
            var unitOfWork = CreateUnitOfwork(dataBaseName);
            var mapper = ConfigAutoMapper();
            var encriptService = new EncriptService();
            var configuration = CreateConfiguration();
            var userService = new UserService(configuration, unitOfWork);
            var userToRegister = new User { UserName = "TestUserName01", Password = encriptService.GetSHA256("PasswordTestUserRegister01*"), Role = RoleType.User, Status = StatusType.Active };
            var registerSuccess = await userService.Register(userToRegister);
            Assert.AreEqual(true, registerSuccess);
            var authController = new AuthController(userService, mapper, encriptService);
            var userDto = new UserDto();
            userDto.UserName = "TestUserName01";
            userDto.Password = "*PasswordTestUserRegister01*";
            var response = (await authController.Token(userDto)) as ForbidResult;
            Assert.IsNotNull(response);
        }
    }
}
