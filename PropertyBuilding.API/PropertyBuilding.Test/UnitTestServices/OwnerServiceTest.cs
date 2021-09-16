using NUnit.Framework;
using PropertyBuilding.Core.Entities;
using PropertyBuilding.Core.Enumerations;
using PropertyBuilding.Core.Interfaces;
using PropertyBuilding.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyBuilding.Test.UnitTestServices
{
    [TestFixture]
    public class OwnerServiceTest : TestBase
    {
        private IOwnerService _ownerService;
        private IUnitOfWork _unitOfWork;

        [SetUp]
        public void Setup()
        {
            var dataBaseName = Guid.NewGuid().ToString();
            _unitOfWork = CreateUnitOfwork(dataBaseName);            
            _ownerService = new OwnerService(_unitOfWork);
        }

        [Test]
        public async Task SaveOwnerAsyncSuccess()
        {
            var ownerToSave = new Owner { Name = "Name Owner Test", Address = "Address test", Photo = "Url Photo Test", Birthday = DateTime.Now.AddYears(-25), Status = StatusType.Active };
            await _ownerService.SaveOwnerAsync(ownerToSave);            
            var owner = await _ownerService.GetOwnerAsync(1);
            Assert.IsNotNull(owner);
            Assert.IsInstanceOf<Owner>(owner);
            Assert.AreEqual("Name Owner Test", owner.Name);
        }

        [Test]
        public async Task GetOwnersSuccess()
        {
            for (int index = 0; index < 10; index++)
            {
                var ownerToSave = new Owner { Name = "Name Owner Test " + index, Address = "Address test", Photo = "Url Photo Test", Birthday = DateTime.Now.AddYears(-25), Status = StatusType.Active };
                await _ownerService.SaveOwnerAsync(ownerToSave);
            }
            var ownerList = _ownerService.GetOwners();
            Assert.IsNotNull(ownerList);
            Assert.AreEqual(10, ownerList.Count());
            Assert.IsInstanceOf<IEnumerable<Owner>>(ownerList);
        }

        [Test]
        public async Task GetOwnerAsyncSuccess()
        {
            for (int index = 1; index <= 10; index++)
            {
                var ownerToSave = new Owner { Name = "Name Owner Test " + index, Address = "Address test", Photo = "Url Photo Test", Birthday = DateTime.Now.AddYears(-25), Status = StatusType.Active };
                await _ownerService.SaveOwnerAsync(ownerToSave);
            }
            var ownerId08 = await _ownerService.GetOwnerAsync(8);
            Assert.IsNotNull(ownerId08);
            Assert.IsInstanceOf<Owner>(ownerId08);
            Assert.AreEqual("Name Owner Test 8", ownerId08.Name);
        }

        [Test]
        public async Task ValidateOwnerAsyncSuccess()
        {
            var ownerToSave = new Owner { Name = "Name Owner Test", Address = "Address test", Photo = "Url Photo Test", Birthday = DateTime.Now.AddYears(-25), Status = StatusType.Active };
            await _ownerService.SaveOwnerAsync(ownerToSave);
            var ownerValidate = await _ownerService.ValidateOwner(1);
            Assert.IsNotNull(ownerValidate);
            Assert.IsInstanceOf<bool>(ownerValidate);
            Assert.AreEqual(true, ownerValidate);
        }

        [Test]
        public async Task DeleteOwnerAsyncSuccess()
        {
            for (int index = 0; index < 10; index++)
            {
                var ownerToSave = new Owner { Name = "Name Owner Test " + index, Address = "Address test", Photo = "Url Photo Test", Birthday = DateTime.Now.AddYears(-25), Status = StatusType.Active };
                await _ownerService.SaveOwnerAsync(ownerToSave);
            }
            _unitOfWork.ChangeTrackerClear();
            var ownerValidate = await _ownerService.ValidateOwner(5);
            Assert.IsNotNull(ownerValidate);
            Assert.IsInstanceOf<bool>(ownerValidate);
            Assert.AreEqual(true, ownerValidate);
            _unitOfWork.ChangeTrackerClear();
            bool ownerDeleteSuccess = await _ownerService.DeleteOwnerAsync(5);
            Assert.IsNotNull(ownerDeleteSuccess);
            Assert.IsInstanceOf<bool>(ownerDeleteSuccess);
            Assert.AreEqual(true, ownerDeleteSuccess);
            ownerValidate = await _ownerService.ValidateOwner(5);
            Assert.IsNotNull(ownerValidate);
            Assert.IsInstanceOf<bool>(ownerValidate);
            Assert.AreEqual(false, ownerValidate);
        }
    }
}
