using NUnit.Framework;
using PropertyBuilding.Core.DTOs;
using PropertyBuilding.Infrastructure.Validators;
using System;
using FluentValidation.TestHelper;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyBuilding.Core.Const.ErrorMessages;

namespace PropertyBuilding.Test.UnitTestValidators
{
    class OwnerValidatorTest
    {
        private OwnerValidator _ownerValidator;
        [SetUp]
        public void Setup()
        {
            _ownerValidator = new OwnerValidator();
        }
        [Test]
        public void ShouldHaveErrorWhenUserBirthdayIsLessThan21YearsAgoToday()
        {
            var ownerDto = new OwnerDto { Name = "Name Owner Test", Address = "Address test", Photo = "Url Photo Test", Birthday = DateTime.Now.AddYears(-20).AddMonths(-11).AddDays(-29)  };
            var result = _ownerValidator.TestValidate(ownerDto);
            result.ShouldHaveValidationErrorFor(ownerDto => ownerDto.Birthday)
                .WithErrorMessage(OwnerErrorMessages.BirthdayLessThan21Years);
        }

        [Test]
        public void ShouldHaveErrorWhenUserBirthdayIsNull()
        {
            var ownerDto = new OwnerDto { Name = "Name Owner Test", Address = "Address test", Photo = "Url Photo Test" };
            var result = _ownerValidator.TestValidate(ownerDto);
            result.ShouldHaveValidationErrorFor(ownerDto => ownerDto.Birthday)
                .WithErrorMessage(OwnerErrorMessages.BirthdayCannotNull);
        }

        [Test]
        public void ShouldHaveErrorWhenUserBirthdayIsGreaterThan100YearsAgoToday()
        {
            var ownerDto = new OwnerDto { Name = "Name Owner Test", Address = "Address test", Photo = "Url Photo Test", Birthday = DateTime.Now.AddYears(-100).AddMonths(-11).AddDays(-29) };
            var result = _ownerValidator.TestValidate(ownerDto);
            result.ShouldHaveValidationErrorFor(ownerDto => ownerDto.Birthday)
                .WithErrorMessage(OwnerErrorMessages.BirthdayGreaterThan100Years);
        }

        [Test]
        public void ShouldHaveErrorWhenNameIsNull()
        {
            var ownerDto = new OwnerDto { Address = "Address test", Photo = "Url Photo Test", Birthday = DateTime.Now.AddYears(-22).AddDays(2) };
            var result = _ownerValidator.TestValidate(ownerDto);
            result.ShouldHaveValidationErrorFor(ownerDto => ownerDto.Name)
                .WithErrorMessage(OwnerErrorMessages.NameCannotNull);
        }

        [Test]
        public void ShouldHaveErrorWhenAddressIsNull()
        {
            var ownerDto = new OwnerDto { Name = "Address test", Photo = "Url Photo Test", Birthday = DateTime.Now.AddYears(-22).AddDays(2) };
            var result = _ownerValidator.TestValidate(ownerDto);
            result.ShouldHaveValidationErrorFor(ownerDto => ownerDto.Address)
                .WithErrorMessage(OwnerErrorMessages.AddressCannotNull);
        }
    }
}
