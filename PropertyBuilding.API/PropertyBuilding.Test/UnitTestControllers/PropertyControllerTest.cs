using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using PropertyBuilding.API.Controllers;
using PropertyBuilding.API.Responses;
using PropertyBuilding.Core.DTOs;
using PropertyBuilding.Core.Entities;
using PropertyBuilding.Core.Enumerations;
using PropertyBuilding.Core.Options;
using PropertyBuilding.Core.Services;
using PropertyBuilding.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyBuilding.Test.UnitTestControllers
{
    [TestFixture]
    class PropertyControllerTest : TestBase
    {
        [Test]
        public async Task GetbyIdSuccess()
        {
            var dataBaseName = Guid.NewGuid().ToString();
            var unitOfWork = CreateUnitOfwork(dataBaseName);
            var mapper = ConfigAutoMapper();
            var ownerService = new OwnerService(unitOfWork);
            var paginationOptions = Options.Create(new PaginationOptions() { DefaultPageNumber = 5, DefaultPageSize = 1 });
            var propertyService = new PropertyService(unitOfWork, paginationOptions, ownerService);
            var owner = new Owner { Name = "Name Owner Test ", Address = "Address test", Photo = "Url Photo Test", Birthday = DateTime.Now.AddYears(-25), Status = StatusType.Active };
            await ownerService.SaveOwnerAsync(owner);
            var propertyToSave = new Property { Name = "Name Test", Address = "Address test", Year = DateTime.Now.Year, Status = StatusType.Active, Price = 1000, IdOwner = owner.Id };
            var propertySaved = await propertyService.SavePropertyAsync(propertyToSave);
            var userMock = CreateUserMock(1, "userTest01", RoleType.User);
            var uriService = new UriService("http://localhost/");
            var propertyController = new PropertyController(propertyService, mapper, uriService);
            propertyController.ControllerContext = new ControllerContext();
            propertyController.ControllerContext.HttpContext = new DefaultHttpContext { User = userMock };
            var propertyResponse = await propertyController.Get(1) as OkObjectResult;
            Assert.IsNotNull(propertyResponse);
            Assert.AreEqual(200, propertyResponse.StatusCode);
        }

        [Test]
        public async Task PostSuccess()
        {
            var dataBaseName = Guid.NewGuid().ToString();
            var unitOfWork = CreateUnitOfwork(dataBaseName);
            var mapper = ConfigAutoMapper();
            var ownerService = new OwnerService(unitOfWork);
            var paginationOptions = Options.Create(new PaginationOptions() { DefaultPageNumber = 5, DefaultPageSize = 1 });
            var propertyService = new PropertyService(unitOfWork, paginationOptions, ownerService);
            var userMock = CreateUserMock(1, "userTest01", RoleType.User);
            var uriService = new UriService("http://localhost/");
            var owner = new Owner { Name = "Name Owner Test ", Address = "Address test", Photo = "Url Photo Test", Birthday = DateTime.Now.AddYears(-25), Status = StatusType.Active };
            await ownerService.SaveOwnerAsync(owner);
            var propertyToSaveDto = new PropertyDto { Name = "Name Test", Address = "Address test", Year = DateTime.Now.Year, Price = 1000, IdOwner = owner.Id };
            var propertyController = new PropertyController(propertyService, mapper, uriService);
            propertyController.ControllerContext = new ControllerContext();
            propertyController.ControllerContext.HttpContext = new DefaultHttpContext { User = userMock };
            var responsePost = await propertyController.Post(propertyToSaveDto) as OkObjectResult;
            Assert.IsNotNull(responsePost);
            Assert.AreEqual(200, responsePost.StatusCode);
            var property = await propertyService.GetPropertyByIdAsync(1);
            var standardResponse = (StandardResponse<PropertyDto>)responsePost.Value;
            var propertyResponse = standardResponse.Data;
            Assert.AreEqual(property.Name, propertyResponse.Name);
            Assert.AreEqual(property.Address, propertyResponse.Address);
            Assert.AreEqual(property.Year, propertyResponse.Year);
            Assert.AreEqual(property.Price, propertyResponse.Price);
            Assert.AreEqual(property.IdOwner, propertyResponse.IdOwner);

        }
        [Test]
        public async Task PatchSuccess()
        {
            var dataBaseName = Guid.NewGuid().ToString();
            var unitOfWork = CreateUnitOfwork(dataBaseName);
            var mapper = ConfigAutoMapper();
            var ownerService = new OwnerService(unitOfWork);
            var paginationOptions = Options.Create(new PaginationOptions() { DefaultPageNumber = 5, DefaultPageSize = 1 });
            var propertyService = new PropertyService(unitOfWork, paginationOptions, ownerService);
            var userMock = CreateUserMock(1, "userTest01", RoleType.User);
            var uriService = new UriService("http://localhost/");
            var owner = new Owner { Name = "Name Owner Test ", Address = "Address test", Photo = "Url Photo Test", Birthday = DateTime.Now.AddYears(-25), Status = StatusType.Active };
            await ownerService.SaveOwnerAsync(owner);
            var propertyToSave = new Property { Name = "Name Test", Address = "Address test", Year = DateTime.Now.Year, Price = 1000, IdOwner = owner.Id, IdUser = 1 };            
            await propertyService.SavePropertyAsync(propertyToSave);
            unitOfWork.ChangeTrackerClear();
            var propertyToChange = await propertyService.GetPropertyByIdAsync(1);
            var propertyController = new PropertyController(propertyService, mapper, uriService);
            propertyController.ControllerContext = new ControllerContext();
            propertyController.ControllerContext.HttpContext = new DefaultHttpContext { User = userMock };
            propertyToChange.Name = "New Name";
            propertyToChange.Address = "New Address";
            propertyToChange.CodeInternal = "New CodeInternal";
            propertyToChange.Year = DateTime.Now.AddYears(-2).Year;
            propertyToChange.Price = 2000;
            var propertyToChangeDto = mapper.Map<PropertyDto>(propertyToChange);
            var responsePost = await propertyController.Patch(propertyToChangeDto) as OkObjectResult;            
            Assert.IsNotNull(responsePost);
            Assert.AreEqual(200, responsePost.StatusCode);
            var propertyChanged = await propertyService.GetPropertyByIdAsync(1);
            var standardResponse = (StandardResponse<PropertyDto>)responsePost.Value;
            var propertyResponse = standardResponse.Data;
            Assert.AreEqual(propertyChanged.Name, propertyResponse.Name);
            Assert.AreEqual(propertyChanged.Address, propertyResponse.Address);
            Assert.AreEqual(propertyChanged.Year, propertyResponse.Year);
            Assert.AreEqual(propertyChanged.Price, propertyResponse.Price);
            Assert.AreEqual(propertyChanged.IdOwner, propertyResponse.IdOwner);
        }
        [Test]
        public async Task UpdatePriceSuccess()
        {
            var dataBaseName = Guid.NewGuid().ToString();
            var unitOfWork = CreateUnitOfwork(dataBaseName);
            var mapper = ConfigAutoMapper();
            var ownerService = new OwnerService(unitOfWork);
            var paginationOptions = Options.Create(new PaginationOptions() { DefaultPageNumber = 5, DefaultPageSize = 1 });
            var propertyService = new PropertyService(unitOfWork, paginationOptions, ownerService);
            var userMock = CreateUserMock(1, "userTest01", RoleType.User);
            var uriService = new UriService("http://localhost/");
            var owner = new Owner { Name = "Name Owner Test ", Address = "Address test", Photo = "Url Photo Test", Birthday = DateTime.Now.AddYears(-25), Status = StatusType.Active };
            await ownerService.SaveOwnerAsync(owner);
            var propertyToSave = new Property { Name = "Name Test", Address = "Address test", Year = DateTime.Now.Year, Price = 1000, IdOwner = owner.Id, IdUser = 1 };
            await propertyService.SavePropertyAsync(propertyToSave);
            unitOfWork.ChangeTrackerClear();
            var propertyToChange = await propertyService.GetPropertyByIdAsync(1);
            var propertyController = new PropertyController(propertyService, mapper, uriService);
            propertyController.ControllerContext = new ControllerContext();
            propertyController.ControllerContext.HttpContext = new DefaultHttpContext { User = userMock };            
            propertyToChange.Price = 2000;
            var propertyToChangeDto = mapper.Map<PropertyChangePriceDto>(propertyToChange);
            var responsePost = await propertyController.UpdatePrice(propertyToChangeDto) as OkObjectResult;
            Assert.IsNotNull(responsePost);
            Assert.AreEqual(200, responsePost.StatusCode);
            var propertyChanged = await propertyService.GetPropertyByIdAsync(1);
            var standardResponse = (StandardResponse<PropertyDto>)responsePost.Value;
            var propertyResponse = standardResponse.Data;           
            Assert.AreEqual(propertyChanged.Price, propertyResponse.Price);            
        }
    }
}
