using FluentValidation.TestHelper;
using NUnit.Framework;
using PropertyBuilding.Core.Const.ErrorMessages;
using PropertyBuilding.Core.DTOs;
using PropertyBuilding.Core.Enumerations;
using PropertyBuilding.Infrastructure.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyBuilding.Test.UnitTestValidators
{
    [TestFixture]
    public class SignInValidatorTest
    {
        private SignInValidator signInValidator;
        [SetUp]
        public void Setup()
        {
            signInValidator = new SignInValidator();
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
