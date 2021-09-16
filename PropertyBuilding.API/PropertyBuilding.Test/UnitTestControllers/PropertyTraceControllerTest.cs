using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using PropertyBuilding.API.Controllers;
using PropertyBuilding.API.Responses;
using PropertyBuilding.Core.DTOs;
using PropertyBuilding.Core.Entities;
using PropertyBuilding.Core.Enumerations;
using PropertyBuilding.Core.Interfaces;
using PropertyBuilding.Core.Options;
using PropertyBuilding.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PropertyBuilding.Test.UnitTestControllers
{
    [TestFixture]
    public class PropertyTraceControllerTest: TestBase
    {
        private IPropertyTraceService _propertyTraceService;
        private IPropertyService _propertyService;
        private IOwnerService _ownerService;
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        private IOptions<PaginationOptions> _paginationOptions;
        const int MIN_BIRTHDAY_YEAR = -90;
        const int MAX_BIRTHDAY_YEAR = -22;
        const int MAX_PRICE = 50000;
        const int MIN_PRICE = 1000;
        const int MIN_YEAR = 1900;

        [SetUp]
        public void Setup()
        {
            _paginationOptions = Options.Create(new PaginationOptions() { DefaultPageNumber = 5, DefaultPageSize = 1 });
            var dataBaseName = Guid.NewGuid().ToString();
            _unitOfWork = CreateUnitOfwork(dataBaseName);
            _ownerService = new OwnerService(_unitOfWork);
            _propertyService = new PropertyService(_unitOfWork, _paginationOptions, _ownerService);
            _propertyTraceService = new PropertyTraceService(_unitOfWork);
            _mapper = ConfigAutoMapper();            
        }
        [Test]
        public async Task GetSucess()
        {
            var userMock = CreateUserMock(1, "userTest01", RoleType.User);
            int idUser = int.Parse(userMock.Claims.FirstOrDefault(claim => claim.Type.Contains("IdUser")).Value);
            DateTime birthday = DateTime.Now.AddYears(new Random().Next(MIN_BIRTHDAY_YEAR, MAX_BIRTHDAY_YEAR)).Date;
            var owner = new Owner { Name = "Name Owner Test", Address = "Address test", Photo = "Url Photo Test", Birthday = birthday, Status = StatusType.Active, IdUser = idUser };
            await _ownerService.SaveOwnerAsync(owner);
            double price = new Random().Next(MIN_PRICE, MAX_PRICE);
            var propertyToSave = new Property { Name = "Name Test", Address = "Address test", Year = DateTime.Now.Year, Status = StatusType.Active, Price = price, IdOwner = owner.Id, IdUser = idUser };
            var property = await _propertyService.SavePropertyAsync(propertyToSave);
            for (int index = 1; index <= 10; index++)
            {
                var propertyTraceToSave = new PropertyTrace { Name = "Name Property Trace Test", Tax = 10, DateSale = DateTime.Now.Date, IdUser = idUser };
                await _propertyTraceService.SavePropertyTraceAsync(propertyTraceToSave);
            }
            var propertyTraceController = new PropertyTraceController(_propertyTraceService, _mapper);
            propertyTraceController.ControllerContext = new ControllerContext();
            propertyTraceController.ControllerContext.HttpContext = new DefaultHttpContext { User = userMock };
            var propertyTraceResponse = await propertyTraceController.Get() as OkObjectResult;
            Assert.IsNotNull(propertyTraceResponse);
            var standardResponse = (StandardResponse<IEnumerable<PropertyTraceDto>>)propertyTraceResponse.Value;
            var propertyTraceList = standardResponse.Data;
            Assert.AreEqual(10, propertyTraceList.Count());
        }

        [Test]
        public async Task GetByIdSucess()
        {
            var userMock = CreateUserMock(1, "userTest01", RoleType.User);
            int idUser = int.Parse(userMock.Claims.FirstOrDefault(claim => claim.Type.Contains("IdUser")).Value);
            DateTime birthday = DateTime.Now.AddYears(new Random().Next(MIN_BIRTHDAY_YEAR, MAX_BIRTHDAY_YEAR)).Date;
            var owner = new Owner { Name = "Name Owner Test", Address = "Address test", Photo = "Url Photo Test", Birthday = birthday, Status = StatusType.Active, IdUser = idUser };
            await _ownerService.SaveOwnerAsync(owner);
            double price = new Random().Next(MIN_PRICE, MAX_PRICE);
            var propertyToSave = new Property { Name = "Name Test", Address = "Address test", Year = DateTime.Now.Year, Status = StatusType.Active, Price = price, IdOwner = owner.Id, IdUser = idUser };
            var property = await _propertyService.SavePropertyAsync(propertyToSave);
            var propertyTraceToSave = new PropertyTrace { Name = "Name Property Trace Test", Tax = 10, DateSale = DateTime.Now.Date, IdUser = idUser, Value = 1000 };
            var propertyTraceSaved = await _propertyTraceService.SavePropertyTraceAsync(propertyTraceToSave);            
            var propertyTraceController = new PropertyTraceController(_propertyTraceService, _mapper);
            propertyTraceController.ControllerContext = new ControllerContext();
            propertyTraceController.ControllerContext.HttpContext = new DefaultHttpContext { User = userMock };
            var propertyTraceResponse = await propertyTraceController.Get(propertyTraceToSave.Id) as OkObjectResult;
            Assert.IsNotNull(propertyTraceResponse);
            var standardResponse = (StandardResponse<PropertyTraceDto>)propertyTraceResponse.Value;
            var propertyTrace = standardResponse.Data;
            Assert.AreEqual(propertyTraceToSave.Id, propertyTrace.Id);
            Assert.AreEqual(propertyTraceToSave.Name, propertyTrace.Name);
            Assert.AreEqual(propertyTraceToSave.DateSale, propertyTrace.DateSale);
            Assert.AreEqual(propertyTraceToSave.Tax, propertyTrace.Tax);
            Assert.AreEqual(propertyTraceToSave.Value, propertyTrace.Value);
        }

        [Test]
        public async Task PostSucess()
        {
            var userMock = CreateUserMock(1, "userTest01", RoleType.User);
            int idUser = int.Parse(userMock.Claims.FirstOrDefault(claim => claim.Type.Contains("IdUser")).Value);
            DateTime birthday = DateTime.Now.AddYears(new Random().Next(MIN_BIRTHDAY_YEAR, MAX_BIRTHDAY_YEAR)).Date;
            var owner = new Owner { Name = "Name Owner Test", Address = "Address test", Photo = "Url Photo Test", Birthday = birthday, Status = StatusType.Active, IdUser = idUser };
            await _ownerService.SaveOwnerAsync(owner);
            double price = new Random().Next(MIN_PRICE, MAX_PRICE);
            var propertyToSave = new Property { Name = "Name Test", Address = "Address test", Year = DateTime.Now.Year, Status = StatusType.Active, Price = price, IdOwner = owner.Id, IdUser = idUser };
            var property = await _propertyService.SavePropertyAsync(propertyToSave);
            var propertyTraceDto = new PropertyTraceDto { Name = "Name Trace Test", DateSale = DateTime.Now.Date, Tax = 10, Value = 2000, IdProperty = property.Id };
            var propertyTraceController = new PropertyTraceController(_propertyTraceService, _mapper);
            propertyTraceController.ControllerContext = new ControllerContext();
            propertyTraceController.ControllerContext.HttpContext = new DefaultHttpContext { User = userMock };
            var propertyTraceResponse = await propertyTraceController.Post(propertyTraceDto) as OkObjectResult;
            Assert.IsNotNull(propertyTraceResponse);
            Assert.AreEqual(200, propertyTraceResponse.StatusCode);
            var standardResponse = (StandardResponse<PropertyTraceDto>)propertyTraceResponse.Value;
            var propertyTrace = standardResponse.Data;
            Assert.AreEqual(propertyTraceDto.IdProperty, propertyTrace.IdProperty);
            Assert.AreEqual(propertyTraceDto.Tax, propertyTrace.Tax);
            Assert.AreEqual(propertyTraceDto.Name, propertyTrace.Name);
            Assert.AreEqual(propertyTraceDto.DateSale, propertyTrace.DateSale);
        }

        [Test]
        public async Task PatchSucess()
        {
            var userMock = CreateUserMock(1, "userTest01", RoleType.User);
            int idUser = int.Parse(userMock.Claims.FirstOrDefault(claim => claim.Type.Contains("IdUser")).Value);
            DateTime birthday = DateTime.Now.AddYears(new Random().Next(MIN_BIRTHDAY_YEAR, MAX_BIRTHDAY_YEAR)).Date;
            var owner = new Owner { Name = "Name Owner Test", Address = "Address test", Photo = "Url Photo Test", Birthday = birthday, Status = StatusType.Active, IdUser = idUser };
            await _ownerService.SaveOwnerAsync(owner);
            double price = new Random().Next(MIN_PRICE, MAX_PRICE);
            var propertyToSave = new Property { Name = "Name Test", Address = "Address test", Year = DateTime.Now.Year, Status = StatusType.Active, Price = price, IdOwner = owner.Id, IdUser = idUser };
            var property = await _propertyService.SavePropertyAsync(propertyToSave);
            var propertyTraceDto = new PropertyTraceDto { Name = "Name Trace Test", DateSale = DateTime.Now.Date, Tax = 10, Value = 2000, IdProperty = property.Id };
            var propertyTraceController = new PropertyTraceController(_propertyTraceService, _mapper);
            propertyTraceController.ControllerContext = new ControllerContext();
            propertyTraceController.ControllerContext.HttpContext = new DefaultHttpContext { User = userMock };
            var propertyTraceResponse = await propertyTraceController.Post(propertyTraceDto) as OkObjectResult;
            _unitOfWork.ChangeTrackerClear();
            var standardResponse = (StandardResponse<PropertyTraceDto>)propertyTraceResponse.Value;
            var propertyTraceToChange = standardResponse.Data;
            propertyTraceToChange.Name = "New Name Test";
            propertyTraceToChange.DateSale = DateTime.Now.AddYears(-1).Date;
            propertyTraceToChange.Tax = 0;
            propertyTraceToChange.Value = 100;
            var propertyTraceToChangeDto = _mapper.Map<PropertyTraceDto>(propertyTraceToChange);
            propertyTraceResponse = await propertyTraceController.Patch(propertyTraceToChangeDto) as OkObjectResult;
            Assert.IsNotNull(propertyTraceResponse);
            Assert.AreEqual(200, propertyTraceResponse.StatusCode);
            standardResponse = (StandardResponse<PropertyTraceDto>)propertyTraceResponse.Value;
            var propertyTraceChanged = standardResponse.Data;
            Assert.AreEqual("New Name Test", propertyTraceChanged.Name);
            Assert.AreEqual(0, propertyTraceChanged.Tax);
            Assert.AreEqual(DateTime.Now.AddYears(-1).Date, propertyTraceChanged.DateSale);
            Assert.AreEqual(100, propertyTraceChanged.Value);
        }

        [Test]
        public async Task DeleteSucess()
        {
            var userMock = CreateUserMock(1, "userTest01", RoleType.User);
            int idUser = int.Parse(userMock.Claims.FirstOrDefault(claim => claim.Type.Contains("IdUser")).Value);
            DateTime birthday = DateTime.Now.AddYears(new Random().Next(MIN_BIRTHDAY_YEAR, MAX_BIRTHDAY_YEAR)).Date;
            var owner = new Owner { Name = "Name Owner Test", Address = "Address test", Photo = "Url Photo Test", Birthday = birthday, Status = StatusType.Active, IdUser = idUser };
            await _ownerService.SaveOwnerAsync(owner);
            double price = new Random().Next(MIN_PRICE, MAX_PRICE);
            var propertyToSave = new Property { Name = "Name Test", Address = "Address test", Year = DateTime.Now.Year, Status = StatusType.Active, Price = price, IdOwner = owner.Id, IdUser = idUser };
            var property = await _propertyService.SavePropertyAsync(propertyToSave);
            var propertyTraceDto = new PropertyTraceDto { Name = "Name Trace Test", DateSale = DateTime.Now.Date, Tax = 10, Value = 2000, IdProperty = property.Id };
            var propertyTraceController = new PropertyTraceController(_propertyTraceService, _mapper);
            propertyTraceController.ControllerContext = new ControllerContext();
            propertyTraceController.ControllerContext.HttpContext = new DefaultHttpContext { User = userMock };
            var propertyTraceResponse = await propertyTraceController.Post(propertyTraceDto) as OkObjectResult;
            _unitOfWork.ChangeTrackerClear();
            var standardResponse = (StandardResponse<PropertyTraceDto>)propertyTraceResponse.Value;
            var propertyTraceToDelete = standardResponse.Data;
            propertyTraceResponse = await propertyTraceController.Delete(propertyTraceToDelete.Id) as OkObjectResult;
            Assert.IsNotNull(propertyTraceResponse);
            Assert.AreEqual(200, propertyTraceResponse.StatusCode);            
            var propertyTraceValidateDelete = (StandardResponse<Boolean>)propertyTraceResponse.Value;
            Assert.AreEqual(true,propertyTraceValidateDelete.Data);
        }
    }
}
