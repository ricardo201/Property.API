using NUnit.Framework;
using PropertyBuilding.Core.Entities;
using PropertyBuilding.Core.Enumerations;
using PropertyBuilding.Core.Interfaces;
using PropertyBuilding.Core.Services;
using System;
using System.Threading.Tasks;

namespace PropertyBuilding.Test.UnitTestServices
{
    [TestFixture]
    class UserServiceTest: TestBase
    {
        private IUserService _userService;
        private IUnitOfWork _unitOfWork;
        [SetUp]
        public void Setup()
        {
            var dataBaseName = Guid.NewGuid().ToString();
            _unitOfWork = CreateUnitOfwork(dataBaseName);
            var configuration = CreateConfiguration();
            _userService = new UserService(configuration, _unitOfWork);
        }

        [Test]
        public async Task RegisterSuccessTest()
        {
            var userToRegister = new User { UserName = "TestUserName", Password = "PasswordTestUserRegister01*", Role = RoleType.User, Status = StatusType.Active };
            var signInSuccess = await _userService.Register(userToRegister);
            Assert.IsNotNull(signInSuccess);
            Assert.AreEqual(true, signInSuccess);
        }

        [Test]
        public async Task NotExistUserNameInDBTest()
        {
            var userToRegister = new User { UserName = "TestUserName01", Password = "PasswordTestUserRegister01*", Role = RoleType.User, Status = StatusType.Active };
            var signInSuccess = await _userService.Register(userToRegister);
            userToRegister = new User { UserName = "TestUserName02", Password = "PasswordTestUserRegister02*", Role = RoleType.Administrator, Status = StatusType.Active };
            signInSuccess = await _userService.Register(userToRegister);
            var userNameNotExist = await _userService.NotExistUserName("TestUserName");
            Assert.IsNotNull(userNameNotExist);
            Assert.AreEqual(true, userNameNotExist);
        }

        [Test]
        public async Task AuthenticationSuccess()
        {
            var userToRegister = new User { UserName = "TestUserName01", Password = "PasswordTestUserRegister01*", Role = RoleType.User, Status = StatusType.Active };
            await _userService.Register(userToRegister);
            var userToAuthenticate = new User { UserName = "TestUserName01", Password = "PasswordTestUserRegister01*" };
            var token = await _userService.Authentication(userToAuthenticate);
            Assert.IsNotNull(token);
            Assert.IsNotEmpty(token);
        }

        [Test]
        public async Task GetLoginByCredentialsSuccess()
        {
            var userToRegister = new User { UserName = "TestUserName01", Password = "PasswordTestUserRegister01*", Role = RoleType.User, Status = StatusType.Active };
            await _userService.Register(userToRegister);
            var userToAuthenticate = new User { UserName = "TestUserName01", Password = "PasswordTestUserRegister01*" };
            var user  = await _userService.GetLoginByCredentials(userToAuthenticate);
            Assert.IsNotNull(user);
            Assert.IsInstanceOf<User>(user);
        }

        [Test]
        public async Task IsValidUserSuccess()
        {
            var userToRegister = new User { UserName = "TestUserName01", Password = "PasswordTestUserRegister01*", Role = RoleType.User, Status = StatusType.Active };
            await _userService.Register(userToRegister);
            var userToAuthenticate = new User { UserName = "TestUserName01", Password = "PasswordTestUserRegister01*" };
            var isUserValidSuccess = await _userService.IsValid(userToAuthenticate);
            Assert.IsNotNull(isUserValidSuccess);
            Assert.AreEqual(userToRegister, isUserValidSuccess.Item1);
            Assert.AreEqual(true, isUserValidSuccess.Item2);
        }

        [Test]
        public void GenerateTokenSuccess()
        {
            var userToRequestToken = new User { UserName = "TestUserName01", Password = "PasswordTestUserRegister01*", Role = RoleType.User, Status = StatusType.Active };                       
            var token = _userService.GenerateToken(userToRequestToken);
            Assert.IsNotNull(token);
            Assert.IsNotEmpty(token);            
        }
    }
}
