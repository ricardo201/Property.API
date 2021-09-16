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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyBuilding.Test.UnitTestServices
{
    [TestFixture]
    public class PropertyImageServiceTest : TestBase
    {
        private IPropertyImageService _propertyImageService;
        private IPropertyService _propertyService;
        private IOwnerService _ownerService;
        private IUnitOfWork _unitOfWork;
        private IOptions<PaginationOptions> _paginationOptions;

        [SetUp]
        public void Setup()
        {
            _paginationOptions = Options.Create(new PaginationOptions() { DefaultPageNumber = 5, DefaultPageSize = 1 });
            var dataBaseName = Guid.NewGuid().ToString();
            _unitOfWork = CreateUnitOfwork(dataBaseName);
            _ownerService = new OwnerService(_unitOfWork);
            _propertyService = new PropertyService(_unitOfWork, _paginationOptions, _ownerService);
            _propertyImageService = new PropertyImageService(_unitOfWork);
        }

        [Test]
        public async Task GetPropertyImagesSucess()
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
            var property = await _propertyService.SavePropertyAsync(propertyToSave);
            IFormFile imageFileMock = CreateMockImageFile("testFile.Jpeg", PropertyImagesLimits.SizeFileLimitInBytes, PropertyImagesContentTypes.Jpeg);
            var propertyImageToSave = new PropertyImage { Enabled = true, File = "Name file", IdProperty = property.Id, Status = StatusType.Active  };
            var propertyImageSaved = await _propertyImageService.SavePropertyImageAsync(propertyImageToSave);
            var propertyImageList = _propertyImageService.GetPropertyImages();
            Assert.IsNotNull(propertyImageList);
            Assert.AreEqual(1, propertyImageList.Count());

        }
        [Test]
        public async Task SavePropertyImageAsyncSuccess()
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
            var property = await _propertyService.SavePropertyAsync(propertyToSave);
            IFormFile imageFileMock = CreateMockImageFile("testFile.Jpeg", PropertyImagesLimits.SizeFileLimitInBytes, PropertyImagesContentTypes.Jpeg);
            var propertyImage = new PropertyImage { Enabled = true, File = "Name file", IdProperty = property.Id, Status = StatusType.Active };
            var propertyImageSaved = await _propertyImageService.SavePropertyImageAsync(propertyImage);
            Assert.IsNotNull(propertyImageSaved);
            Assert.AreEqual(1, propertyImageSaved.Id);
        }
        [Test]
        public async Task DeletePropertyImageAsyncSuccess()
        {
            const int MIN_BIRTHDAY_YEAR = -90;
            const int MAX_BIRTHDAY_YEAR = -22;
            const int MAX_PRICE = 50000;
            const int MIN_PRICE = 1000;
            var userMock = CreateUserMock(1, "userTest01", RoleType.User);
            int idUser = int.Parse(userMock.Claims.FirstOrDefault(claim => claim.Type.Contains("IdUser")).Value);
            DateTime birthday = DateTime.Now.AddYears(new Random().Next(MIN_BIRTHDAY_YEAR, MAX_BIRTHDAY_YEAR)).Date;
            var owner = new Owner { Name = "Name Owner Test", Address = "Address test", Photo = "Url Photo Test", Birthday = birthday, Status = StatusType.Active, IdUser = idUser };
            await _ownerService.SaveOwnerAsync(owner);
            double price = new Random().Next(MIN_PRICE, MAX_PRICE);
            var propertyToSave = new Property { Name = "Name Test", Address = "Address test", Year = DateTime.Now.Year, Status = StatusType.Active, Price = price, IdOwner = owner.Id, IdUser = idUser };
            var property = await _propertyService.SavePropertyAsync(propertyToSave);
            IFormFile imageFileMock = CreateMockImageFile("testFile.Jpeg", PropertyImagesLimits.SizeFileLimitInBytes, PropertyImagesContentTypes.Jpeg);
            var propertyImageToSave = new PropertyImage { Enabled = true, File = "Name file", IdProperty = property.Id, Status = StatusType.Active, IdUser = idUser };
            var propertyImageSaved = await _propertyImageService.SavePropertyImageAsync(propertyImageToSave);
            var propertyImage = await _propertyImageService.GetPropertyImageByIdAsync(1, idUser);
            _unitOfWork.ChangeTrackerClear();
            bool propertyImageDeleteValidate = await _propertyImageService.DeletePropertyImageAsync(1);
            Assert.IsNotNull(propertyImageDeleteValidate);
            Assert.AreEqual(true, propertyImageDeleteValidate);            
        }
        [Test]
        public async Task GetPropertyImageByIdAsyncSuccess()
        {
            const int MIN_BIRTHDAY_YEAR = -90;
            const int MAX_BIRTHDAY_YEAR = -22;
            const int MAX_PRICE = 50000;
            const int MIN_PRICE = 1000;
            var userMock = CreateUserMock(1, "userTest01", RoleType.User);
            int idUser = int.Parse(userMock.Claims.FirstOrDefault(claim => claim.Type.Contains("IdUser")).Value);
            DateTime birthday = DateTime.Now.AddYears(new Random().Next(MIN_BIRTHDAY_YEAR, MAX_BIRTHDAY_YEAR)).Date;
            var owner = new Owner { Name = "Name Owner Test", Address = "Address test", Photo = "Url Photo Test", Birthday = birthday, Status = StatusType.Active, IdUser = idUser };
            await _ownerService.SaveOwnerAsync(owner);
            double price = new Random().Next(MIN_PRICE, MAX_PRICE);
            var propertyToSave = new Property { Name = "Name Test", Address = "Address test", Year = DateTime.Now.Year, Status = StatusType.Active, Price = price, IdOwner = owner.Id, IdUser = idUser };
            var property = await _propertyService.SavePropertyAsync(propertyToSave);
            IFormFile imageFileMock = CreateMockImageFile("testFile.Jpeg", PropertyImagesLimits.SizeFileLimitInBytes, PropertyImagesContentTypes.Jpeg);
            var propertyImageToSave = new PropertyImage { Enabled = true, File = "Name file", IdProperty = property.Id, Status = StatusType.Active, IdUser = idUser };
            var propertyImageSaved = await _propertyImageService.SavePropertyImageAsync(propertyImageToSave);
            var propertyImage = await _propertyImageService.GetPropertyImageByIdAsync(1, idUser);
            Assert.IsNotNull(propertyImage);
            Assert.AreEqual(1, propertyImage.Id);
            Assert.AreEqual(propertyImageToSave.Id, propertyImage.IdProperty);
            Assert.AreEqual(propertyImageToSave.IdUser, propertyImage.IdUser);
        }
        [Test]
        public async Task GetPropertyImageByFileNameSuccess()
        {
            const int MIN_BIRTHDAY_YEAR = -90;
            const int MAX_BIRTHDAY_YEAR = -22;
            const int MAX_PRICE = 50000;
            const int MIN_PRICE = 1000;
            var userMock = CreateUserMock(1, "userTest01", RoleType.User);
            int idUser = int.Parse(userMock.Claims.FirstOrDefault(claim => claim.Type.Contains("IdUser")).Value);
            DateTime birthday = DateTime.Now.AddYears(new Random().Next(MIN_BIRTHDAY_YEAR, MAX_BIRTHDAY_YEAR)).Date;
            var owner = new Owner { Name = "Name Owner Test", Address = "Address test", Photo = "Url Photo Test", Birthday = birthday, Status = StatusType.Active, IdUser = idUser };
            await _ownerService.SaveOwnerAsync(owner);
            double price = new Random().Next(MIN_PRICE, MAX_PRICE);
            var propertyToSave = new Property { Name = "Name Test", Address = "Address test", Year = DateTime.Now.Year, Status = StatusType.Active, Price = price, IdOwner = owner.Id, IdUser = idUser };
            var property = await _propertyService.SavePropertyAsync(propertyToSave);
            IFormFile imageFileMock = CreateMockImageFile("testFile.Jpeg", PropertyImagesLimits.SizeFileLimitInBytes, PropertyImagesContentTypes.Jpeg);
            var propertyImageToSave = new PropertyImage { Enabled = true, File = "Name file", IdProperty = property.Id, Status = StatusType.Active, IdUser = idUser };
            var propertyImageSaved = await _propertyImageService.SavePropertyImageAsync(propertyImageToSave);
            var propertyImage = _propertyImageService.GetPropertyImageByFileName("Name file", idUser);
            Assert.IsNotNull(propertyImage);
            Assert.AreEqual(1, propertyImage.Id);
            Assert.AreEqual(propertyImageToSave.Id, propertyImage.IdProperty);
            Assert.AreEqual(propertyImageToSave.IdUser, propertyImage.IdUser);
            Assert.AreEqual("Name file", propertyImage.File);
        }
        [Test]
        public async Task ChangeEnabledPropertyImageAsynSuccess()
        {
            const int MIN_BIRTHDAY_YEAR = -90;
            const int MAX_BIRTHDAY_YEAR = -22;
            const int MAX_PRICE = 50000;
            const int MIN_PRICE = 1000;
            var userMock = CreateUserMock(1, "userTest01", RoleType.User);
            int idUser = int.Parse(userMock.Claims.FirstOrDefault(claim => claim.Type.Contains("IdUser")).Value);
            DateTime birthday = DateTime.Now.AddYears(new Random().Next(MIN_BIRTHDAY_YEAR, MAX_BIRTHDAY_YEAR)).Date;
            var owner = new Owner { Name = "Name Owner Test", Address = "Address test", Photo = "Url Photo Test", Birthday = birthday, Status = StatusType.Active, IdUser = idUser };
            await _ownerService.SaveOwnerAsync(owner);
            double price = new Random().Next(MIN_PRICE, MAX_PRICE);
            var propertyToSave = new Property { Name = "Name Test", Address = "Address test", Year = DateTime.Now.Year, Status = StatusType.Active, Price = price, IdOwner = owner.Id, IdUser = idUser };
            var property = await _propertyService.SavePropertyAsync(propertyToSave);
            IFormFile imageFileMock = CreateMockImageFile("testFile.Jpeg", PropertyImagesLimits.SizeFileLimitInBytes, PropertyImagesContentTypes.Jpeg);
            var propertyImageToSave = new PropertyImage { Enabled = true, File = "Name file", IdProperty = property.Id, Status = StatusType.Active, IdUser = idUser };
            var propertyImageSaved = await _propertyImageService.SavePropertyImageAsync(propertyImageToSave);
            var propertyImage = await _propertyImageService.GetPropertyImageByIdAsync(1, idUser);
            _unitOfWork.ChangeTrackerClear();
            propertyImage.Enabled = false; 
            var propertyImageChanged = await _propertyImageService.ChangeEnabledPropertyImageAsyn(propertyImage,idUser);
            Assert.IsNotNull(propertyImageChanged);
            Assert.AreEqual(false, propertyImageChanged.Enabled);
        }
    }
}
