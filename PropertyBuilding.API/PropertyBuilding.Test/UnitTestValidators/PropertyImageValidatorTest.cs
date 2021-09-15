using FluentValidation.TestHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using PropertyBuilding.Core.Const.ErrorMessages;
using PropertyBuilding.Core.Const.PropertyImages;
using PropertyBuilding.Core.DTOs;
using PropertyBuilding.Core.Entities;
using PropertyBuilding.Core.Enumerations;
using PropertyBuilding.Core.Interfaces;
using PropertyBuilding.Core.Options;
using PropertyBuilding.Core.Services;
using PropertyBuilding.Infrastructure.Validators;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyBuilding.Test.UnitTestValidators
{
    [TestFixture]
    public class PropertyImageValidatorTest : TestBase
    {
        private PropertyImageValidator _propertyImageValidator;
        private IPropertyService _propertyService;
        private IOwnerService _ownerService;
        private IUnitOfWork _unitOfWork;
        private IOptions<PaginationOptions> _paginationOptions;

        private IFormFile CreateMockImageFile(string nameFile, long fileSize, string contentType)
        {
            var fileMock = new Mock<IFormFile>();
            //Setup mock file using a memory stream
            Bitmap bitmap = new Bitmap(1000, 800, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
            Graphics graphics = Graphics.FromImage(bitmap);
            Pen pen = new Pen(Color.FromKnownColor(KnownColor.Blue), 2);
            graphics.DrawArc(pen, 0, 0, 700, 700, 0, 180);
            MemoryStream memoryStream = new MemoryStream();
            byte[] byteArray;
            bitmap.Save(memoryStream, ImageFormat.Jpeg);
            byteArray = memoryStream.ToArray();
            var fileName = nameFile;
            var ms = new MemoryStream();
            memoryStream.Position = 0;
            fileMock.Setup(_ => _.OpenReadStream()).Returns(memoryStream);
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.ContentType).Returns(contentType);
            fileMock.Setup(_ => _.Length).Returns(fileSize);            
            var file = fileMock.Object;
            return file;
        }
        [SetUp]
        public void Setup()
        {
            var dataBaseName = Guid.NewGuid().ToString();
            _unitOfWork = CreateUnitOfwork(dataBaseName);
            _paginationOptions = Options.Create(new PaginationOptions() { DefaultPageNumber = 5, DefaultPageSize = 1 });                        
            _ownerService = new OwnerService(_unitOfWork);
            _propertyService = new PropertyService(_unitOfWork, _paginationOptions, _ownerService);
            _propertyImageValidator = new PropertyImageValidator(_propertyService);
        }

        [Test]
        public void ShouldHaveErrorWhenPropertyIdIsNull()
        {
            IFormFile imageFileMock = CreateMockImageFile("testFile.Jpeg", PropertyImagesLimits.SizeFileLimitInBytes, PropertyImagesContentTypes.Jpeg);
            var propertyImageDto = new PropertyImageDto { Enabled = true, File = "Name file", BlobFile = imageFileMock };
            var result = _propertyImageValidator.TestValidate(propertyImageDto);
            result.ShouldHaveValidationErrorFor(propertyImageDto => propertyImageDto.IdProperty)
               .WithErrorMessage(PropertyErrorMessages.PropertyCannotNull);
        }

        [Test]
        public void ShouldHaveErrorWhenPropertyIdIsZero()
        {
            IFormFile imageFileMock = CreateMockImageFile("testFile.Jpeg", PropertyImagesLimits.SizeFileLimitInBytes, PropertyImagesContentTypes.Jpeg);
            var propertyImageDto = new PropertyImageDto { Enabled = true, File = "Name file", BlobFile = imageFileMock, IdProperty = 0 };
            var result = _propertyImageValidator.TestValidate(propertyImageDto);
            result.ShouldHaveValidationErrorFor(propertyImageDto => propertyImageDto.IdProperty)
               .WithErrorMessage(PropertyErrorMessages.PropertyCannotZero);
        }

        [Test]
        public void ShouldHaveErrorWhenPropertyDoesNotExist()
        {
            IFormFile imageFileMock = CreateMockImageFile("testFile.Jpeg", PropertyImagesLimits.SizeFileLimitInBytes, PropertyImagesContentTypes.Jpeg);
            var propertyImageDto = new PropertyImageDto { Enabled = true, File = "Name file", BlobFile = imageFileMock, IdProperty = 1 };
            var result = _propertyImageValidator.TestValidate(propertyImageDto);
            result.ShouldHaveValidationErrorFor(propertyImageDto => propertyImageDto.IdProperty)
               .WithErrorMessage(PropertyErrorMessages.PropertyDoesNotExist);
        }

        [Test]
        public async Task ShouldHaveErrorWhenEnableIsNull()
        {
            IFormFile imageFileMock = CreateMockImageFile("testFile.Jpeg", PropertyImagesLimits.SizeFileLimitInBytes, PropertyImagesContentTypes.Jpeg);
            var ownerToSave = new Owner { Name = "Name Owner Test", Address = "Address test", Photo = "Url Photo Test", Birthday = DateTime.Now.AddYears(-25), Status = StatusType.Active };
            var ownerSaved = await _ownerService.SaveOwnerAsync(ownerToSave);
            var propertyToSave = new Property { Name = "Name", Address = "Address", Year = DateTime.Now.Year, Status = StatusType.Active, Price = 1000, IdOwner = ownerSaved.Id };
            var propertySaved = await _propertyService.SavePropertyAsync(propertyToSave);
            var propertyImageDto = new PropertyImageDto { File = "Name file", BlobFile = imageFileMock, IdProperty = propertySaved.Id };
            var result = _propertyImageValidator.TestValidate(propertyImageDto);
            result.ShouldHaveValidationErrorFor(propertyImageDto => propertyImageDto.Enabled)
               .WithErrorMessage(PropertyImageErrorMessages.EnableIsRequired);
        }

        [Test]
        public async Task ShouldHaveErrorWhenBlobFileIsNull()
        {           
            var ownerToSave = new Owner { Name = "Name Owner Test", Address = "Address test", Photo = "Url Photo Test", Birthday = DateTime.Now.AddYears(-25), Status = StatusType.Active };
            var ownerSaved = await _ownerService.SaveOwnerAsync(ownerToSave);
            var propertyToSave = new Property { Name = "Name", Address = "Address", Year = DateTime.Now.Year, Status = StatusType.Active, Price = 1000, IdOwner = ownerSaved.Id };
            var propertySaved = await _propertyService.SavePropertyAsync(propertyToSave);
            var propertyImageDto = new PropertyImageDto { File = "Name file", Enabled = true, IdProperty = propertySaved.Id };
            var result = _propertyImageValidator.TestValidate(propertyImageDto);
            result.ShouldHaveValidationErrorFor(propertyImageDto => propertyImageDto.BlobFile)
               .WithErrorMessage(PropertyImageErrorMessages.FileIsRequired);
        }

        [Test]
        public async Task ShouldHaveErrorWhenFileExceedsSizeLimit()
        {
            IFormFile imageFileMock = CreateMockImageFile("testFile.Jpeg", PropertyImagesLimits.SizeFileLimitInBytes + 10000, PropertyImagesContentTypes.Jpeg);            
            var ownerToSave = new Owner { Name = "Name Owner Test", Address = "Address test", Photo = "Url Photo Test", Birthday = DateTime.Now.AddYears(-25), Status = StatusType.Active };
            var ownerSaved = await _ownerService.SaveOwnerAsync(ownerToSave);
            var propertyToSave = new Property { Name = "Name", Address = "Address", Year = DateTime.Now.Year, Status = StatusType.Active, Price = 1000, IdOwner = ownerSaved.Id };
            var propertySaved = await _propertyService.SavePropertyAsync(propertyToSave);
            var propertyImageDto = new PropertyImageDto { File = "Name file", BlobFile = imageFileMock, IdProperty = propertySaved.Id, Enabled = true  };
            var result = _propertyImageValidator.TestValidate(propertyImageDto);
            result.ShouldHaveValidationErrorFor(propertyImageDto => propertyImageDto.BlobFile.Length)
               .WithErrorMessage(PropertyImageErrorMessages.SizeLimit);
        }

        [Test]
        public async Task ShouldHaveErrorWhenContentTypeNotAllowed()
        {
            IFormFile imageFileMock = CreateMockImageFile("testFile.Jpeg", PropertyImagesLimits.SizeFileLimitInBytes, "pdf");
            var ownerToSave = new Owner { Name = "Name Owner Test", Address = "Address test", Photo = "Url Photo Test", Birthday = DateTime.Now.AddYears(-25), Status = StatusType.Active };
            var ownerSaved = await _ownerService.SaveOwnerAsync(ownerToSave);
            var propertyToSave = new Property { Name = "Name", Address = "Address", Year = DateTime.Now.Year, Status = StatusType.Active, Price = 1000, IdOwner = ownerSaved.Id };
            var propertySaved = await _propertyService.SavePropertyAsync(propertyToSave);
            var propertyImageDto = new PropertyImageDto { File = "Name file", BlobFile = imageFileMock, IdProperty = propertySaved.Id, Enabled = true };
            var result = _propertyImageValidator.TestValidate(propertyImageDto);
            result.ShouldHaveValidationErrorFor(propertyImageDto => propertyImageDto.BlobFile.ContentType)
               .WithErrorMessage(PropertyImageErrorMessages.ContentType);
        }
    }
}
