using FluentValidation.TestHelper;
using NUnit.Framework;
using PropertyBuilding.Core.Const.ErrorMessages;
using PropertyBuilding.Core.DTOs;
using PropertyBuilding.Core.Entities;
using PropertyBuilding.Core.Enumerations;
using PropertyBuilding.Core.Interfaces;
using PropertyBuilding.Core.Services;
using PropertyBuilding.Infrastructure.Validators;
using System;

namespace PropertyBuilding.Test.UnitTestValidators
{
    [TestFixture]
    public class SignInValidatorTest : TestBase
    {
        private SignInValidator signInValidator;
        private IUserService _userService;
        private IUnitOfWork _unitOfWork;
        [SetUp]
        public void Setup()
        {
            var dataBaseName = Guid.NewGuid().ToString();            
            _unitOfWork = CreateUnitOfwork(dataBaseName);
            var configuration = CreateConfiguration();
            _userService = new UserService(configuration, _unitOfWork);
            signInValidator = new SignInValidator(_userService);
        }

        [Test]
        public void ShouldHaveErrorWhenUserNameIsNull()
        {
            var signInDto = new SignInDto { Password = "PasswordTest123*", Role = RoleType.User};
            var result = signInValidator.TestValidate(signInDto);
            result.ShouldHaveValidationErrorFor(signInDto => signInDto.UserName)
                .WithErrorMessage(SignInErrorMessages.UserNameCanNotNull);
        }

        [Test]
        public void ShouldHaveErrorWhenUserNameExistInDataBase()
        {
            var userToRegister = new User { UserName = "TestUserName", Password = "PasswordTestUserRegister01*", Role = RoleType.User, Status = StatusType.Active };
            _userService.Register(userToRegister);
            var signInDto = new SignInDto { UserName = "TestUserName", Password = "PasswordTest123*", Role = RoleType.User };
            var result = signInValidator.TestValidate(signInDto);
            result.ShouldHaveValidationErrorFor(signInDto => signInDto.UserName)
                .WithErrorMessage(SignInErrorMessages.UserNameExist);
        }

        [Test]
        public void ShouldHaveErrorWhenUserNameLenghtIsLessThan10()
        {
            var signInDto = new SignInDto { Password = "PasswordTest123*", Role = RoleType.User, UserName = "UserName" };
            var result = signInValidator.TestValidate(signInDto);
            result.ShouldHaveValidationErrorFor(signInDto => signInDto.UserName)
                .WithErrorMessage(SignInErrorMessages.UserNameCanNotLessThan10);
        }

        [Test]
        public void ShouldHaveErrorWhenUserNameLenghtIsGreaterThan50()
        {
            var signInDto = new SignInDto { Password = "PasswordTest123*", Role = RoleType.User, UserName = "UserNameIsGreaterThan50_123456789009876543211234567890" };
            var result = signInValidator.TestValidate(signInDto);
            result.ShouldHaveValidationErrorFor(signInDto => signInDto.UserName)
                .WithErrorMessage(SignInErrorMessages.UserNameCanNotGreaterThan50);
        }

        [Test]
        public void ShouldHaveErrorWhenPasswordLenghtIsGreaterThan50()
        {
            var signInDto = new SignInDto { Password = "PasswordIsGreaterThan50123456789009876543211234567890*", Role = RoleType.User, UserName = "UserNameTest1*" };
            var result = signInValidator.TestValidate(signInDto);
            result.ShouldHaveValidationErrorFor(signInDto => signInDto.Password)
                .WithErrorMessage(SignInErrorMessages.PasswordCanNotGreaterThan50);
        }

        [Test]
        public void ShouldHaveErrorWhenPasswordLenghtIsLessThan10()
        {
            var signInDto = new SignInDto { Password = "Pas12*", Role = RoleType.User, UserName = "UserNameTest1*" };
            var result = signInValidator.TestValidate(signInDto);
            result.ShouldHaveValidationErrorFor(signInDto => signInDto.Password)
                .WithErrorMessage(SignInErrorMessages.PasswordCanNotLessThan10);
        }

        [Test]
        public void ShouldHaveErrorWhenPasswordDoesNotContainsUpperCase()
        {
            var signInDto = new SignInDto { Password = "paswordtest12*", Role = RoleType.User, UserName = "UserNameTest1*" };
            var result = signInValidator.TestValidate(signInDto);
            result.ShouldHaveValidationErrorFor(signInDto => signInDto.Password)
                .WithErrorMessage(SignInErrorMessages.PasswordUppercaseLetter);
        }
        [Test]
        public void ShouldHaveErrorWhenPasswordDoesNotContainsLowerCase()
        {
            var signInDto = new SignInDto { Password = "PASSWORDTES123*", Role = RoleType.User, UserName = "UserNameTest1*" };
            var result = signInValidator.TestValidate(signInDto);
            result.ShouldHaveValidationErrorFor(signInDto => signInDto.Password)
                .WithErrorMessage(SignInErrorMessages.PasswordLowercaseLetter);
        }
        [Test]
        public void ShouldHaveErrorWhenPasswordDoesNotContainsDigits()
        {
            var signInDto = new SignInDto { Password = "PaswordTest*_", Role = RoleType.User, UserName = "UserNameTest1*" };
            var result = signInValidator.TestValidate(signInDto);
            result.ShouldHaveValidationErrorFor(signInDto => signInDto.Password)
                .WithErrorMessage(SignInErrorMessages.PasswordDigit);
        }
        [Test]
        public void ShouldHaveErrorWhenPasswordDoesNotContainsSpecialCharacters()
        {
            var signInDto = new SignInDto { Password = "PaswordTest123", Role = RoleType.User, UserName = "UserNameTest1*" };
            var result = signInValidator.TestValidate(signInDto);
            result.ShouldHaveValidationErrorFor(signInDto => signInDto.Password)
                .WithErrorMessage(SignInErrorMessages.PasswordSpecialCharacter);
        }

        [Test]
        public void ShouldHaveErrorWhenPasswordIsNull()
        {
            var signInDto = new SignInDto { Role = RoleType.User, UserName = "UserNameTest1*" };
            var result = signInValidator.TestValidate(signInDto);
            result.ShouldHaveValidationErrorFor(signInDto => signInDto.Password)
                .WithErrorMessage(SignInErrorMessages.PasswordCanNotNull);
        }
    }
}
