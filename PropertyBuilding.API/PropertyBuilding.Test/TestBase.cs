using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using PropertyBuilding.Core.Enumerations;
using PropertyBuilding.Core.Interfaces;
using PropertyBuilding.Infrastructure.Data;
using PropertyBuilding.Infrastructure.Mappings;
using PropertyBuilding.Infrastructure.Repositories;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Security.Claims;

namespace PropertyBuilding.Test
{
    public class TestBase
    {
        protected IUnitOfWork CreateUnitOfwork(string dataBaseName)
        {
            var options = new DbContextOptionsBuilder<PropertyBuildingDataBaseContext>()
                .UseInMemoryDatabase(dataBaseName).Options;
            var dataBaseContext = new PropertyBuildingDataBaseContext(options);
            IUnitOfWork unitOfWork = new UnitOfWork(dataBaseContext);
            return unitOfWork;
        }

        protected IMapper ConfigAutoMapper()
        {
            var config = new MapperConfiguration(options =>
            {
                options.AddProfile(new AutoMapperProfile());
            });

            return config.CreateMapper();
        }

        protected IConfiguration CreateConfiguration()
        {
            var authenticationConfiguration = new Dictionary<string, string>
            {
                {"Authentication:SecretKey", "9zQobBuKXo5yzUC-jb-t2nN4tfHCY80xE6mJEOvVdEk$hKJaid"},
                {"Authentication:ValidIssuer", "https://localhost:44373/"},
                {"Authentication:ValidAudience", "https://localhost:44373/"},
                {"Authentication:LifetimeInMinutes", "25"}
            };
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(authenticationConfiguration)
                .Build();

            return configuration;
        }

        protected ClaimsPrincipal CreateUserMock(int idUser, string userName, RoleType roleType)
        {
            var claims = new[]
            {
                new Claim("IdUser", idUser.ToString()),
                new Claim("UserName", userName),
                new Claim(ClaimTypes.Role, roleType.ToString())
            };
            var userMock = new ClaimsPrincipal(new ClaimsIdentity(claims, "TestAuthentication"));
            return userMock;
        }

        protected IFormFile CreateMockImageFile(string nameFile, long fileSize, string contentType)
        {
            var fileMock = new Mock<IFormFile>();            
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
    }
}
