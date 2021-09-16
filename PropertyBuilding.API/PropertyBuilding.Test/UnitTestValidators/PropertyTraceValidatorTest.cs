using FluentValidation.TestHelper;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using PropertyBuilding.Core.Const.ErrorMessages;
using PropertyBuilding.Core.DTOs;
using PropertyBuilding.Core.Interfaces;
using PropertyBuilding.Core.Options;
using PropertyBuilding.Core.Services;
using PropertyBuilding.Infrastructure.Validators;
using System;

namespace PropertyBuilding.Test.UnitTestValidators
{
    [TestFixture]
    class PropertyTraceValidatorTest : TestBase
    {
        private PropertyTraceValidator _propertyTraceValidator;
        private IPropertyService _propertyService;
        private IOwnerService _ownerService;
        private IUnitOfWork _unitOfWork;
        private IOptions<PaginationOptions> _paginationOptions;

        [SetUp]
        public void Setup()
        {
            var dataBaseName = Guid.NewGuid().ToString();
            _unitOfWork = CreateUnitOfwork(dataBaseName);
            _paginationOptions = Options.Create(new PaginationOptions() { DefaultPageNumber = 5, DefaultPageSize = 1 });
            _ownerService = new OwnerService(_unitOfWork);
            _propertyService = new PropertyService(_unitOfWork, _paginationOptions, _ownerService);
            _propertyTraceValidator = new PropertyTraceValidator(_propertyService);
        }
        [Test]
        public void ShouldHaveErrorWhenPropertyIdIsNull()
        {
            var propertyTraceDto = new PropertyTraceDto { Name = "Name Test", Tax = 20, Value = 2000, DateSale = DateTime.Now.Date };
            var result = _propertyTraceValidator.TestValidate(propertyTraceDto);
            result.ShouldHaveValidationErrorFor(propertyTraceDto => propertyTraceDto.IdProperty)
               .WithErrorMessage(PropertyErrorMessages.PropertyCannotNull);
        }

        [Test]
        public void ShouldHaveErrorWhenPropertyIdIsZero()
        {
            var propertyTraceDto = new PropertyTraceDto { Name = "Name Test", Tax = 20, Value = 2000, DateSale = DateTime.Now.Date, IdProperty = 0 };
            var result = _propertyTraceValidator.TestValidate(propertyTraceDto);
            result.ShouldHaveValidationErrorFor(propertyTraceDto => propertyTraceDto.IdProperty)
               .WithErrorMessage(PropertyErrorMessages.PropertyCannotZero);
        }

        [Test]
        public void ShouldHaveErrorWhenPropertyDoesNotExist()
        {
            var propertyTraceDto = new PropertyTraceDto { Name = "Name Test", Tax = 20, Value = 2000, DateSale = DateTime.Now.Date, IdProperty = 1 };
            var result = _propertyTraceValidator.TestValidate(propertyTraceDto);
            result.ShouldHaveValidationErrorFor(propertyTraceDto => propertyTraceDto.IdProperty)
               .WithErrorMessage(PropertyErrorMessages.PropertyDoesNotExist);
        }
    }
}
