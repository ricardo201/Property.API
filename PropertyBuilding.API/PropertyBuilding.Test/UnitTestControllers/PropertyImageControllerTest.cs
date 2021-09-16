using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using PropertyBuilding.API.Controllers;
using PropertyBuilding.API.Responses;
using PropertyBuilding.Core.Const.PropertyImages;
using PropertyBuilding.Core.DTOs;
using PropertyBuilding.Core.Entities;
using PropertyBuilding.Core.Enumerations;
using PropertyBuilding.Core.Interfaces;
using PropertyBuilding.Core.Options;
using PropertyBuilding.Core.Services;
using PropertyBuilding.Infrastructure.Interfaces;
using PropertyBuilding.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyBuilding.Test.UnitTestControllers
{
    [TestFixture]
    class PropertyImageControllerTest: TestBase
    {
        private IPropertyImageService _propertyImageService;
        private IPropertyService _propertyService;
        private IOwnerService _ownerService;
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        private IOptions<PaginationOptions> _paginationOptions;
        private IFileService _fileService;
        const int MIN_BIRTHDAY_YEAR = -90;
        const int MAX_BIRTHDAY_YEAR = -22;
        const int MAX_PRICE = 50000;
        const int MIN_PRICE = 1000;
        [SetUp]
        public void Setup()
        {
            _paginationOptions = Options.Create(new PaginationOptions() { DefaultPageNumber = 5, DefaultPageSize = 1 });
            var dataBaseName = Guid.NewGuid().ToString();
            _unitOfWork = CreateUnitOfwork(dataBaseName);
            _ownerService = new OwnerService(_unitOfWork);
            _propertyService = new PropertyService(_unitOfWork, _paginationOptions, _ownerService);
            _propertyImageService = new PropertyImageService(_unitOfWork);
            _mapper = ConfigAutoMapper();
            var mockEnvironment = new Mock<IWebHostEnvironment>();            
            mockEnvironment                
                .Setup(option => option.WebRootPath)
                .Returns(Directory.GetCurrentDirectory());
            _fileService = new FileService(mockEnvironment.Object);
        }
        [Test]
        public async Task PostSuccess()
        {
            var userMock = CreateUserMock(1, "userTest01", RoleType.User);
            int idUser = int.Parse(userMock.Claims.FirstOrDefault(claim => claim.Type.Contains("IdUser")).Value);
            DateTime birthday = DateTime.Now.AddYears(new Random().Next(MIN_BIRTHDAY_YEAR, MAX_BIRTHDAY_YEAR)).Date;
            var owner = new Owner { Name = "Name Owner Test", Address = "Address test", Photo = "Url Photo Test", Birthday = birthday, Status = StatusType.Active, IdUser = idUser };
            await _ownerService.SaveOwnerAsync(owner);
            double price = new Random().Next(MIN_PRICE, MAX_PRICE);
            var propertyToSave = new Property { Name = "Name Test", Address = "Address test", Year = DateTime.Now.Year, Status = StatusType.Active, Price = price, IdOwner = owner.Id, IdUser = idUser };
            var property = await _propertyService.SavePropertyAsync(propertyToSave);
            IFormFile imageFileMock = CreateMockImageFile("testFile.Jpeg", PropertyImagesLimits.SizeFileLimitInBytes, PropertyImagesContentTypes.Jpeg);
            var propertyImageDto = new PropertyImageDto { IdProperty = property.Id, BlobFile = imageFileMock, Enabled = true };
            var propertyImageController = new PropertyImageController(_fileService, _propertyImageService, _mapper);
            propertyImageController.ControllerContext = new ControllerContext();
            propertyImageController.ControllerContext.HttpContext = new DefaultHttpContext { User = userMock };
            var propertyImageResponse = await propertyImageController.Post(propertyImageDto) as OkObjectResult;
            Assert.IsNotNull(propertyImageResponse);
            Assert.AreEqual(200, propertyImageResponse.StatusCode);
            var standardResponse = (StandardResponse<PropertyImageDto>)propertyImageResponse.Value;
            var propertyImage = standardResponse.Data;
            Assert.AreEqual(propertyImageDto.IdProperty, propertyImage.IdProperty);
            Assert.AreEqual(true, propertyImage.Enabled);
        }
        [Test]
        public async Task GetSuccess()
        {
            
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
            var propertyImageController = new PropertyImageController(_fileService, _propertyImageService, _mapper);
            propertyImageController.ControllerContext = new ControllerContext();
            propertyImageController.ControllerContext.HttpContext = new DefaultHttpContext { User = userMock };
            var propertyImageResponse = await propertyImageController.Get(propertyImageSaved.Id) as OkObjectResult;
            Assert.IsNotNull(propertyImageResponse);
            Assert.AreEqual(200, propertyImageResponse.StatusCode);
            var standardResponse = (StandardResponse<PropertyImageDto>)propertyImageResponse.Value;
            var propertyImage = standardResponse.Data;
            Assert.AreEqual(propertyImageSaved.File, propertyImage.File);
            Assert.AreEqual(propertyImageSaved.Id, propertyImage.Id);
        }
        [Test]
        public async Task DownloadSuccess()
        {
            var userMock = CreateUserMock(1, "userTest01", RoleType.User);
            int idUser = int.Parse(userMock.Claims.FirstOrDefault(claim => claim.Type.Contains("IdUser")).Value);
            DateTime birthday = DateTime.Now.AddYears(new Random().Next(MIN_BIRTHDAY_YEAR, MAX_BIRTHDAY_YEAR)).Date;
            var owner = new Owner { Name = "Name Owner Test", Address = "Address test", Photo = "Url Photo Test", Birthday = birthday, Status = StatusType.Active, IdUser = idUser };
            await _ownerService.SaveOwnerAsync(owner);
            double price = new Random().Next(MIN_PRICE, MAX_PRICE);
            var propertyToSave = new Property { Name = "Name Test", Address = "Address test", Year = DateTime.Now.Year, Status = StatusType.Active, Price = price, IdOwner = owner.Id, IdUser = idUser };
            var property = await _propertyService.SavePropertyAsync(propertyToSave);
            IFormFile imageFileMock = CreateMockImageFile("testFile.Jpeg", PropertyImagesLimits.SizeFileLimitInBytes, PropertyImagesContentTypes.Jpeg);
            var urlFile = await _fileService.SaveImageAsync(imageFileMock, idUser, (int)property.Id);
            var propertyImageToSave = new PropertyImage { Enabled = true, File = urlFile, IdProperty = property.Id, Status = StatusType.Active, IdUser = idUser };
            var propertyImageSaved = await _propertyImageService.SavePropertyImageAsync(propertyImageToSave);
            var propertyImageController = new PropertyImageController(_fileService, _propertyImageService, _mapper);
            propertyImageController.ControllerContext = new ControllerContext();
            propertyImageController.ControllerContext.HttpContext = new DefaultHttpContext { User = userMock };
            var propertyImageResponse = await propertyImageController.Download(urlFile) as FileContentResult;
            Assert.IsNotNull(propertyImageResponse);
            Assert.AreEqual("application/octet-stream", propertyImageResponse.ContentType);           
        }
        [Test]
        public async Task ChangeEnabledSuccess()
        {            
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
            _unitOfWork.ChangeTrackerClear();
            var propertyImageSavedToChange = await _propertyImageService.GetPropertyImageByIdAsync(1, idUser);
            propertyImageSavedToChange.Enabled = false;
            var propertyImageSavedToChangeDto = _mapper.Map<PropertyImageChangeEnabledDto>(propertyImageSavedToChange);
            var propertyImageController = new PropertyImageController(_fileService, _propertyImageService, _mapper);
            propertyImageController.ControllerContext = new ControllerContext();
            propertyImageController.ControllerContext.HttpContext = new DefaultHttpContext { User = userMock };
            var propertyImageResponse = await propertyImageController.ChangeEnabled(propertyImageSavedToChangeDto) as OkObjectResult;
            Assert.IsNotNull(propertyImageResponse);
            Assert.AreEqual(200, propertyImageResponse.StatusCode);
            var standardResponse = (StandardResponse<PropertyImageDto>)propertyImageResponse.Value;
            var propertyImage = standardResponse.Data;
            Assert.AreEqual(propertyImageSaved.File, propertyImage.File);
            Assert.AreEqual(propertyImageSaved.Id, propertyImage.Id);
            Assert.IsFalse(propertyImage.Enabled);
        }
    }
}
