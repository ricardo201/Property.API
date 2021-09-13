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

        
        public async Task PostSuccess()
        {
           
        }

        
        public async Task PutSuccess(OwnerDto ownerDto)
        {
           
        }
         
        public async Task DeleteSuccess(int id)
        {
        
        }
    }
}
