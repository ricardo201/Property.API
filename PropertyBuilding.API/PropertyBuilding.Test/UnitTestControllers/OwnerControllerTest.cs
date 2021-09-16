using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using PropertyBuilding.API.Controllers;
using PropertyBuilding.API.Responses;
using PropertyBuilding.Core.DTOs;
using PropertyBuilding.Core.Entities;
using PropertyBuilding.Core.Enumerations;
using PropertyBuilding.Core.Services;
using PropertyBuilding.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PropertyBuilding.Test.UnitTestControllers
{
    [TestFixture]
    class OwnerControllerTest : TestBase
    {
        [Test]
        public async Task GetListSuccess()
        {
            var dataBaseName = Guid.NewGuid().ToString();
            var unitOfWork = CreateUnitOfwork(dataBaseName);
            var mapper = ConfigAutoMapper();
            var ownerService = new OwnerService(unitOfWork);
            for (int index = 0; index < 10; index++)
            {
                var ownerToSave = new Owner { Name = "Name Owner Test " + index, Address = "Address test", Photo = "Url Photo Test", Birthday = DateTime.Now.AddYears(-25), Status = StatusType.Active };
                await ownerService.SaveOwnerAsync(ownerToSave);
            }
            var ownerController = new OwnerController(ownerService, mapper);
            var onwerGetResponse = await ownerController.Get() as OkObjectResult;
            Assert.IsNotNull(onwerGetResponse);
            Assert.AreEqual(200, onwerGetResponse.StatusCode);
            var standardResponse = (StandardResponse<IEnumerable<OwnerDto>>)onwerGetResponse.Value;
            Assert.AreEqual(10, standardResponse.Data.Count());
        }

        [Test]
        public async Task GetByIdSuccess()
        {
            var dataBaseName = Guid.NewGuid().ToString();
            var unitOfWork = CreateUnitOfwork(dataBaseName);
            var mapper = ConfigAutoMapper();            
            var ownerService = new OwnerService(unitOfWork);
            for (int index = 1; index <= 10; index++)
            {
                var ownerToSave = new Owner { Name = "Name Owner Test " + index, Address = "Address test", Photo = "Url Photo Test", Birthday = DateTime.Now.AddYears(-25), Status = StatusType.Active };
                await ownerService.SaveOwnerAsync(ownerToSave);
            }
            var ownerController = new OwnerController(ownerService, mapper);
            var onwerGetResponse = await ownerController.Get(2) as OkObjectResult;
            Assert.IsNotNull(onwerGetResponse);
            Assert.AreEqual(200, onwerGetResponse.StatusCode);
            var standardResponse = (StandardResponse<OwnerDto>)onwerGetResponse.Value;
            Assert.AreEqual("Name Owner Test 2", standardResponse.Data.Name);
        }

        [Test]
        public async Task PostSuccess()
        {
            var dataBaseName = Guid.NewGuid().ToString();
            var unitOfWork = CreateUnitOfwork(dataBaseName);
            var mapper = ConfigAutoMapper();
            var ownerService = new OwnerService(unitOfWork);
            var userMock = CreateUserMock(1, "userTest01", RoleType.User);
            var ownerToSaveDto = new OwnerDto { Name = "Name Owner Test Post", Address = "Address test", Photo = "Url Photo Test", Birthday = DateTime.Now.AddYears(-25) };
            var ownerController = new OwnerController(ownerService, mapper);
            ownerController.ControllerContext = new ControllerContext();
            ownerController.ControllerContext.HttpContext = new DefaultHttpContext { User = userMock };
            var responsePost = await ownerController.Post(ownerToSaveDto) as OkObjectResult;
            Assert.IsNotNull(responsePost);
            Assert.AreEqual(200, responsePost.StatusCode);
        }

        [Test]
        public async Task PutSuccess()
        {
            var dataBaseName = Guid.NewGuid().ToString();
            var unitOfWork = CreateUnitOfwork(dataBaseName);
            var mapper = ConfigAutoMapper();
            var ownerService = new OwnerService(unitOfWork);
            var userMock = CreateUserMock(1, "userTest01", RoleType.User);
            for (int index = 1; index <= 10; index++)
            {
                var ownerToSave = new Owner { Name = "Name Owner Test " + index, Address = "Address test", Photo = "Url Photo Test", Birthday = DateTime.Now.AddYears(-25), Status = StatusType.Active,  IdUser = int.Parse(userMock.Claims.FirstOrDefault(claim => claim.Type.Contains("IdUser")).Value) };
                await ownerService.SaveOwnerAsync(ownerToSave);
            }
            unitOfWork.ChangeTrackerClear();
            int idOwnerToUpdate = new Random().Next(1, 10);
            var ownerToUpdate = await unitOfWork.OwnerRepository.GetByIdAsync(idOwnerToUpdate);
            var ownerToUpdateDto = mapper.Map<OwnerDto>(ownerToUpdate);
            ownerToUpdateDto.Address = "Change address test";
            ownerToUpdateDto.Name = "Change Name Test";
            var ownerController = new OwnerController(ownerService, mapper);
            ownerController.ControllerContext = new ControllerContext();
            ownerController.ControllerContext.HttpContext = new DefaultHttpContext { User = userMock };
            var ownerPutResponse = await ownerController.Put(ownerToUpdateDto) as OkObjectResult;            
            Assert.IsNotNull(ownerPutResponse);
            Assert.AreEqual(200, ownerPutResponse.StatusCode);
            unitOfWork.ChangeTrackerClear();
            var ownerUpdated = await unitOfWork.OwnerRepository.GetByIdAsync(idOwnerToUpdate);
            Assert.AreEqual("Change address test", ownerUpdated.Address);
            Assert.AreEqual("Change Name Test", ownerUpdated.Name);

        }
        [Test] 
        public async Task DeleteSuccess()
        {
            var dataBaseName = Guid.NewGuid().ToString();
            var unitOfWork = CreateUnitOfwork(dataBaseName);
            var mapper = ConfigAutoMapper();
            var ownerService = new OwnerService(unitOfWork);
            var userMock = CreateUserMock(1, "userTest01", RoleType.User);
            for (int index = 1; index <= 10; index++)
            {
                var ownerToSave = new Owner { Name = "Name Owner Test " + index, Address = "Address test", Photo = "Url Photo Test", Birthday = DateTime.Now.AddYears(-25), Status = StatusType.Active, IdUser = int.Parse(userMock.Claims.FirstOrDefault(claim => claim.Type.Contains("IdUser")).Value) };
                await ownerService.SaveOwnerAsync(ownerToSave);
            }
            unitOfWork.ChangeTrackerClear();
            var ownerController = new OwnerController(ownerService, mapper);
            int idOwnerToDelete = new Random().Next(1, 10);
            ownerController.ControllerContext = new ControllerContext();
            ownerController.ControllerContext.HttpContext = new DefaultHttpContext { User = userMock };
            var ownerToDelete = await unitOfWork.OwnerRepository.GetByIdAsync(idOwnerToDelete);
            unitOfWork.ChangeTrackerClear();
            var ownerDeleteResponse = await ownerController.Delete(ownerToDelete.Id) as OkObjectResult;
            Assert.IsNotNull(ownerDeleteResponse);
            Assert.AreEqual(200, ownerDeleteResponse.StatusCode);
            Assert.IsNull(await unitOfWork.OwnerRepository.GetByIdAsync(idOwnerToDelete));
        }
    }
}
