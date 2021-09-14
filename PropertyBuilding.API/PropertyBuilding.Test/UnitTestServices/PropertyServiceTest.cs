using Microsoft.Extensions.Options;
using NUnit.Framework;
using PropertyBuilding.Core.Entities;
using PropertyBuilding.Core.Enumerations;
using PropertyBuilding.Core.Interfaces;
using PropertyBuilding.Core.Options;
using PropertyBuilding.Core.QueryFilters;
using PropertyBuilding.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyBuilding.Test.UnitTestServices
{
    [TestFixture]
    public class PropertyServiceTest : TestBase
    {
        private IPropertyService _propertyService;
        private IOwnerService _ownerService;
        private IUnitOfWork _unitOfWork;
        private IOptions<PaginationOptions> _paginationOptions;
        private async Task<IEnumerable<Property>> MockProperties(int numberOfProperties, int numberOfOwners)
        {
            const int MAX_PRICE = 100000;
            const int MIN_PRICE = 1000;
            const int MIN_YEAR = 1900;
            for (int index = 1; index <= numberOfProperties; index++)
            {
                int idOwner = new Random().Next(1, numberOfOwners);
                int year = new Random().Next(MIN_YEAR, 2021);
                double price = new Random().Next(MIN_PRICE, MAX_PRICE);
                var propertyToSave = new Property { Name = "Name Test " + index, Address = "Address test", Year = year, Status = StatusType.Active, Price = price, IdOwner = idOwner };
                await _propertyService.SavePropertyAsync(propertyToSave);
            }
            PropertyQueryFilter propertyQueryFilter = new PropertyQueryFilter { rows = numberOfProperties, page = 1 };
            return _propertyService.GetProperties(propertyQueryFilter);
        }

        private async Task<IEnumerable<Owner>> MockOwners(int numberOfOwners)
        {            
            const int MIN_BIRTHDAY_YEAR = -90;
            const int MAX_BIRTHDAY_YEAR = -22;
            for (int index = 1; index <= numberOfOwners; index++)
            {
                DateTime birthday = DateTime.Now.AddYears(new Random().Next(MIN_BIRTHDAY_YEAR, MAX_BIRTHDAY_YEAR)).Date;
                var ownerToSave = new Owner { Name = "Name Owner Test " + index, Address = "Address test", Photo = "Url Photo Test", Birthday = birthday, Status = StatusType.Active };
                await _ownerService.SaveOwnerAsync(ownerToSave);
            }
            return _ownerService.GetOwners();            
        }

        [SetUp]
        public void Setup()
        {
            _paginationOptions = Options.Create(new PaginationOptions() { DefaultPageNumber = 5, DefaultPageSize = 1 });
            var dataBaseName = Guid.NewGuid().ToString();
            _unitOfWork = CreateUnitOfwork(dataBaseName);
            _ownerService = new OwnerService(_unitOfWork);
            _propertyService = new PropertyService(_unitOfWork, _paginationOptions, _ownerService);
        }
        
        [Test]
        public async Task GetPropertiesWithoutQueryFilterSucess()
        {
            var ownerList = await MockOwners(100);
            var propertyListMock = await MockProperties(100, ownerList.Count());
            PropertyQueryFilter propertyQueryFilter = new PropertyQueryFilter { rows = 100, page = 1 };
            var propertyList = _propertyService.GetProperties(propertyQueryFilter);
            Assert.AreEqual(100, propertyList.Count());
        }

        [Test]
        public async Task GetPropertyByIdAsyncSucess()
        {
            var ownerList = await MockOwners(20);
            var propertyListMock = await MockProperties(100, ownerList.Count());
            var idProperty = new Random().Next(1, 100);
            var property = await _propertyService.GetPropertyByIdAsync(idProperty);
            Assert.AreEqual(idProperty, property.Id);
            Assert.AreEqual("Name Test " + idProperty, property.Name);
        }


        [Test]
        public async Task ChangePricePropertyAsyncSuccess()
        {
            const int MIN_BIRTHDAY_YEAR = -90;
            const int MAX_BIRTHDAY_YEAR = -22;
            const int MAX_PRICE = 50000;
            const int MIN_PRICE = 1000;
            DateTime birthday = DateTime.Now.AddYears(new Random().Next(MIN_BIRTHDAY_YEAR, MAX_BIRTHDAY_YEAR)).Date;
            var owner = new Owner { Name = "Name Owner Test", Address = "Address test", Photo = "Url Photo Test", Birthday = birthday, Status = StatusType.Active };
            await _ownerService.SaveOwnerAsync(owner);            
            double price = new Random().Next(MIN_PRICE, MAX_PRICE);
            var propertyToSave = new Property { Name = "Name Test", Address = "Address test", Year = DateTime.Now.Year, Status = StatusType.Active, Price = price, IdOwner = owner.Id };
            var propertySaved = await _propertyService.SavePropertyAsync(propertyToSave);
            _unitOfWork.ChangeTrackerClear();
            double priceToChange = new Random().Next(60000, 100000);
            Assert.AreEqual(price, propertyToSave.Price);
            propertySaved.Price = priceToChange;
            var propertyPriceChanged = await _propertyService.ChangePricePropertyAsync(propertySaved);
            Assert.IsNotNull(propertyPriceChanged);
            Assert.AreEqual(propertySaved.Id, propertyPriceChanged.Id);
            Assert.AreEqual(propertySaved.Price, propertyPriceChanged.Price);
            Assert.AreEqual(priceToChange, propertyPriceChanged.Price);
            Assert.AreNotEqual(price, propertyPriceChanged.Price);
        }


        [Test]
        public async Task UpdatePropertyAsyncSuccess()
        {
            const int MIN_BIRTHDAY_YEAR = -90;
            const int MAX_BIRTHDAY_YEAR = -22;
            const int MAX_PRICE = 50000;
            const int MIN_PRICE = 1000;
            DateTime birthday = DateTime.Now.AddYears(new Random().Next(MIN_BIRTHDAY_YEAR, MAX_BIRTHDAY_YEAR)).Date;
            var owner = new Owner { Name = "Name Owner Test", Address = "Address test", Photo = "Url Photo Test", Birthday = birthday, Status = StatusType.Active };
            await _ownerService.SaveOwnerAsync(owner);
            double price = new Random().Next(MIN_PRICE, MAX_PRICE);
            var propertyToSave = new Property { Name = "Name Test", Address = "Address test", Year = DateTime.Now.Year, Status = StatusType.Active, Price = price, IdOwner = owner.Id };
            var propertySaved = await _propertyService.SavePropertyAsync(propertyToSave);
            _unitOfWork.ChangeTrackerClear();
            var propertyToChange = new Property { Name = "New Name", Address = "New Address test", Year = DateTime.Now.AddYears(-5).Year, Status = StatusType.Active, Price = price, IdOwner = owner.Id };
            propertyToChange.Id = propertySaved.Id;
            var propertyChanged = await _propertyService.UpdatePropertyAsync(propertyToChange);
            Assert.IsNotNull(propertyChanged);
            Assert.AreEqual(propertySaved.Id, propertyChanged.Id);
            Assert.AreNotEqual(propertySaved.Name, propertyChanged.Name);
            Assert.AreNotEqual(propertySaved.Address, propertyChanged.Address);
            Assert.AreNotEqual(propertySaved.Year, propertyChanged.Year);           
        }


        [Test]
        public async Task SavePropertyAsync()
        {
            const int MIN_BIRTHDAY_YEAR = -90;
            const int MAX_BIRTHDAY_YEAR = -22;
            const int MAX_PRICE = 50000;
            const int MIN_PRICE = 1000;
            const string NAME = "Name Test";
            const string ADDRESS = "Address Test";
            DateTime birthday = DateTime.Now.AddYears(new Random().Next(MIN_BIRTHDAY_YEAR, MAX_BIRTHDAY_YEAR)).Date;
            var owner = new Owner { Name = "Name Owner Test", Address = ADDRESS, Photo = "Url Photo Test", Birthday = birthday, Status = StatusType.Active };
            await _ownerService.SaveOwnerAsync(owner);
            double price = new Random().Next(MIN_PRICE, MAX_PRICE);
            var propertyToSave = new Property { Name = NAME, Address = ADDRESS, Year = DateTime.Now.Year, Status = StatusType.Active, Price = price, IdOwner = owner.Id };
            var propertySaved = await _propertyService.SavePropertyAsync(propertyToSave);
            Assert.IsNotNull(propertySaved);
            Assert.AreEqual(1, propertySaved.Id);
            Assert.AreEqual(NAME, propertySaved.Name);
            Assert.AreEqual(ADDRESS, propertySaved.Address);
            Assert.AreEqual(DateTime.Now.Year, propertySaved.Year);
            Assert.AreEqual(owner.Id, propertySaved.IdOwner);
        }
        [Test]
        public async Task ValidatePropertyAsyncSuccess()
        {
            const int MIN_BIRTHDAY_YEAR = -90;
            const int MAX_BIRTHDAY_YEAR = -22;
            const int MAX_PRICE = 50000;
            const int MIN_PRICE = 1000;
            const string NAME = "Name Test";
            const string ADDRESS = "Address Test";
            DateTime birthday = DateTime.Now.AddYears(new Random().Next(MIN_BIRTHDAY_YEAR, MAX_BIRTHDAY_YEAR)).Date;
            var owner = new Owner { Name = "Name Owner Test", Address = ADDRESS, Photo = "Url Photo Test", Birthday = birthday, Status = StatusType.Active };
            await _ownerService.SaveOwnerAsync(owner);
            double price = new Random().Next(MIN_PRICE, MAX_PRICE);
            var propertyToSave = new Property { Name = NAME, Address = ADDRESS, Year = DateTime.Now.Year, Status = StatusType.Active, Price = price, IdOwner = owner.Id };
            var propertySaved = await _propertyService.SavePropertyAsync(propertyToSave);
            bool propertyIsValid = await _propertyService.ValidatePropertyAsync(propertyToSave.Id);
            Assert.IsNotNull(propertyIsValid);
            Assert.AreEqual(true, propertyIsValid);            
        }
    }
}
