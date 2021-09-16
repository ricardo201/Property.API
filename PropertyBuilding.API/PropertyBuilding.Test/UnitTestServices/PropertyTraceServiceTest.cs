using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using PropertyBuilding.Core.Const.PropertyImages;
using PropertyBuilding.Core.Entities;
using PropertyBuilding.Core.Enumerations;
using PropertyBuilding.Core.Interfaces;
using PropertyBuilding.Core.Options;
using PropertyBuilding.Core.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PropertyBuilding.Test.UnitTestServices
{
    class PropertyTraceServiceTest:TestBase
    {
        private IPropertyTraceService _propertyTraceService;
        private IPropertyService _propertyService;
        private IOwnerService _ownerService;
        private IUnitOfWork _unitOfWork;
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
        }
        [Test]
        public async Task GetPropertyTracesSuccess()
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
                var propertyTraceToSave = new PropertyTrace { Name = "Name Property Trace Test", Tax = 10, DateSale = DateTime.Now.Date , IdUser = idUser };
                await _propertyTraceService.SavePropertyTraceAsync(propertyTraceToSave);
            }
            var propertyTraceServiceList = _propertyTraceService.GetPropertyTraces();
            Assert.IsNotNull(propertyTraceServiceList);
            Assert.AreEqual(10, propertyTraceServiceList.Count());
        }
        [Test]
        public async Task GetPropertyTraceAsyncSuccess()
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
            var propertyTraceServiceList = _propertyTraceService.GetPropertyTraces();
            Assert.IsNotNull(propertyTraceServiceList);
            Assert.AreEqual(10, propertyTraceServiceList.Count());
        }
        [Test]
        public async Task SavePropertyTraceAsyncSuccess()
        {
            var userMock = CreateUserMock(1, "userTest01", RoleType.User);
            int idUser = int.Parse(userMock.Claims.FirstOrDefault(claim => claim.Type.Contains("IdUser")).Value);
            DateTime birthday = DateTime.Now.AddYears(new Random().Next(MIN_BIRTHDAY_YEAR, MAX_BIRTHDAY_YEAR)).Date;
            var owner = new Owner { Name = "Name Owner Test", Address = "Address test", Photo = "Url Photo Test", Birthday = birthday, Status = StatusType.Active, IdUser = idUser };
            await _ownerService.SaveOwnerAsync(owner);
            double price = new Random().Next(MIN_PRICE, MAX_PRICE);
            var propertyToSave = new Property { Name = "Name Test", Address = "Address test", Year = DateTime.Now.Year, Status = StatusType.Active, Price = price, IdOwner = owner.Id, IdUser = idUser };
            var property = await _propertyService.SavePropertyAsync(propertyToSave);
            var propertyTraceToSave = new PropertyTrace { Name = "Name Property Trace Test", Tax = 10, DateSale = DateTime.Now.Date, IdUser = idUser, IdProperty = property.Id };
            await _propertyTraceService.SavePropertyTraceAsync(propertyTraceToSave);
            var propertyTraceService = await _propertyTraceService.GetPropertyTraceAsync(1);
            Assert.IsNotNull(propertyTraceService);
            Assert.AreEqual(1, propertyTraceService.IdProperty);
            Assert.AreEqual(propertyTraceToSave.Name, propertyTraceService.Name);
            Assert.AreEqual(propertyTraceToSave.Status, propertyTraceService.Status);
            Assert.AreEqual(propertyTraceToSave.Tax, propertyTraceService.Tax);
        }
        [Test]
        public async Task DeletePropertyTraceAsyncSuccess()
        {
            var userMock = CreateUserMock(1, "userTest01", RoleType.User);
            int idUser = int.Parse(userMock.Claims.FirstOrDefault(claim => claim.Type.Contains("IdUser")).Value);
            DateTime birthday = DateTime.Now.AddYears(new Random().Next(MIN_BIRTHDAY_YEAR, MAX_BIRTHDAY_YEAR)).Date;
            var owner = new Owner { Name = "Name Owner Test", Address = "Address test", Photo = "Url Photo Test", Birthday = birthday, Status = StatusType.Active, IdUser = idUser };
            await _ownerService.SaveOwnerAsync(owner);
            double price = new Random().Next(MIN_PRICE, MAX_PRICE);
            var propertyToSave = new Property { Name = "Name Test", Address = "Address test", Year = DateTime.Now.Year, Status = StatusType.Active, Price = price, IdOwner = owner.Id, IdUser = idUser };
            var property = await _propertyService.SavePropertyAsync(propertyToSave);
            var propertyTraceToSave = new PropertyTrace { Name = "Name Property Trace Test", Tax = 10, DateSale = DateTime.Now.Date, IdUser = idUser, IdProperty = property.Id };
            await _propertyTraceService.SavePropertyTraceAsync(propertyTraceToSave);
            _unitOfWork.ChangeTrackerClear();
            var propertyTraceService = await _propertyTraceService.GetPropertyTraceAsync(1);
            Assert.IsNotNull(propertyTraceService);
            Assert.AreEqual(1, propertyTraceService.IdProperty);
            bool propertyTraceDeleteValidate = await _propertyTraceService.DeletePropertyTraceAsync(1);
            Assert.IsNotNull(propertyTraceDeleteValidate);
            Assert.AreEqual(true, propertyTraceDeleteValidate);
        }

        [Test]
        public async Task UpdatePropertyTraceAsynSuccess()
        {
            var userMock = CreateUserMock(1, "userTest01", RoleType.User);
            int idUser = int.Parse(userMock.Claims.FirstOrDefault(claim => claim.Type.Contains("IdUser")).Value);
            DateTime birthday = DateTime.Now.AddYears(new Random().Next(MIN_BIRTHDAY_YEAR, MAX_BIRTHDAY_YEAR)).Date;
            var owner = new Owner { Name = "Name Owner Test", Address = "Address test", Photo = "Url Photo Test", Birthday = birthday, Status = StatusType.Active, IdUser = idUser };
            await _ownerService.SaveOwnerAsync(owner);
            double price = new Random().Next(MIN_PRICE, MAX_PRICE);
            var propertyToSave = new Property { Name = "Name Test", Address = "Address test", Year = DateTime.Now.Year, Status = StatusType.Active, Price = price, IdOwner = owner.Id, IdUser = idUser };
            var property = await _propertyService.SavePropertyAsync(propertyToSave);
            var propertyTraceToSave = new PropertyTrace { Name = "Name Property Trace Test", Tax = 10, DateSale = DateTime.Now.Date, IdUser = idUser, IdProperty = property.Id };
            await _propertyTraceService.SavePropertyTraceAsync(propertyTraceToSave);
            _unitOfWork.ChangeTrackerClear();
            var propertyTraceService = await _propertyTraceService.GetPropertyTraceAsync(1);
            propertyTraceService.Name = "New Name Test";
            propertyTraceService.DateSale = DateTime.Now.AddYears(-1).Date;
            propertyTraceService.Tax = 0;
            propertyTraceService.Value = 100; 
            var propertyTraceServiceChanged = await _propertyTraceService.UpdatePropertyTraceAsyn(propertyTraceService);
            Assert.IsNotNull(propertyTraceServiceChanged);
            Assert.AreEqual("New Name Test", propertyTraceServiceChanged.Name);
            Assert.AreEqual(0, propertyTraceServiceChanged.Tax);
            Assert.AreEqual(DateTime.Now.AddYears(-1).Date, propertyTraceServiceChanged.DateSale);
            Assert.AreEqual(100, propertyTraceServiceChanged.Value);
        }
    }
}
