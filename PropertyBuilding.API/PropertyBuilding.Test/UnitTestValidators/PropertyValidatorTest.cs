using FluentValidation.TestHelper;
using NUnit.Framework;
using PropertyBuilding.Core.Const.ErrorMessages;
using PropertyBuilding.Core.DTOs;
using PropertyBuilding.Infrastructure.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyBuilding.Test.UnitTestValidators
{
    class PropertyValidatorTest
    {
        private PropertyValidator _propertyValidator;
        [SetUp]
        public void Setup()
        {
            _propertyValidator = new PropertyValidator();
        }

        [Test]
        public void ShouldHaveErrorWhenIdOwnerIsNull()
        {
            var propertyDto = new PropertyDto { Name = "Name Owner Test", Address = "Address test", Year = 2000 };
            var result = _propertyValidator.TestValidate(propertyDto);
            result.ShouldHaveValidationErrorFor(propertyDto => propertyDto.IdOwner)
                .WithErrorMessage(OwnerErrorMessages.IdOwnerCannotNull);
        }

        [Test]
        public void ShouldHaveErrorWhenIdOwnerIsLessThanZero()
        {
            var propertyDto = new PropertyDto { Name = "Name Owner Test", Address = "Address test", Year = 2000, IdOwner = -1 };
            var result = _propertyValidator.TestValidate(propertyDto);
            result.ShouldHaveValidationErrorFor(propertyDto => propertyDto.IdOwner)
                .WithErrorMessage(OwnerErrorMessages.IdOwnerCannotZero);
        }

        public void ShouldHaveErrorWhenIdOwnerEqualZero()
        {
            var propertyDto = new PropertyDto { Name = "Name Test", Address = "Address test", Year = 2000, IdOwner = 0 };
            var result = _propertyValidator.TestValidate(propertyDto);
            result.ShouldHaveValidationErrorFor(propertyDto => propertyDto.IdOwner)
                .WithErrorMessage(OwnerErrorMessages.IdOwnerCannotZero);
        }

        [Test]
        public void ShouldHaveErrorWhenYearIsNull()
        {
            var propertyDto = new PropertyDto { Name = "Name Test", Address = "Address test", IdOwner = 1 };
            var result = _propertyValidator.TestValidate(propertyDto);
            result.ShouldHaveValidationErrorFor(propertyDto => propertyDto.Year)
                .WithErrorMessage(PropertyErrorMessages.YearCannotNull);
        }

        [Test]
        public void ShouldHaveErrorWhenYearIsLessThan1900()
        {
            var propertyDto = new PropertyDto { Name = "Name Test", Address = "Address test", IdOwner = 1, Year = 1800 };
            var result = _propertyValidator.TestValidate(propertyDto);
            result.ShouldHaveValidationErrorFor(propertyDto => propertyDto.Year)
                .WithErrorMessage(PropertyErrorMessages.YearLessThan1900);
        }

        [Test]
        public void ShouldHaveErrorWhenNameIsNull()
        {
            var propertyDto = new PropertyDto { Address = "Address test", IdOwner = 1, Year = 1901 };
            var result = _propertyValidator.TestValidate(propertyDto);
            result.ShouldHaveValidationErrorFor(propertyDto => propertyDto.Name)
                .WithErrorMessage(PropertyErrorMessages.NameCannotNull);
        }

        [Test]
        public void ShouldHaveErrorWhenNameLenghtIsLessThan3()
        {
            var propertyDto = new PropertyDto { Name = "Na", Address = "Address test", IdOwner = 1, Year = 1901 };
            var result = _propertyValidator.TestValidate(propertyDto);
            result.ShouldHaveValidationErrorFor(propertyDto => propertyDto.Name)
                .WithErrorMessage(PropertyErrorMessages.NameLength);
        }

        [Test]
        public void ShouldHaveErrorWhenNameLenghtIsgreaterThan100()
        {
            var propertyDto = new PropertyDto { Name = "Name Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.", Address = "Address test", IdOwner = 1, Year = 1901 };
            var result = _propertyValidator.TestValidate(propertyDto);
            result.ShouldHaveValidationErrorFor(propertyDto => propertyDto.Name)
                .WithErrorMessage(PropertyErrorMessages.NameLength);
        }
    }
}
