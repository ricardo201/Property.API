using PropertyBuilding.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace PropertyBuilding.Infrastructure.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public FileService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<string> SaveImageAsync(IFormFile file, int idUser, int propertyId)
        {
            try
            {
                string fileName = NameFile(file.FileName);
                if (!Directory.Exists(_webHostEnvironment.WebRootPath + "\\uploads\\" + idUser + "\\" + propertyId + "\\"))
                {
                    Directory.CreateDirectory(_webHostEnvironment.WebRootPath + "\\uploads\\" + idUser + "\\" + propertyId + "\\");
                }
                using (FileStream fileStream = System.IO.File.Create(_webHostEnvironment.WebRootPath + "\\uploads\\" + idUser + "\\" + propertyId + "\\" + fileName))
                {
                    await file.CopyToAsync(fileStream);
                    fileStream.Flush();
                    return fileName;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string GetFilePath(string fileName, int idUser, int propertyId)
        {
            return _webHostEnvironment.WebRootPath + "\\uploads\\" + idUser + "\\" + propertyId + "\\" + fileName;
        }

        private string GetExtension(string file)
        {
            var extension = "." + file.Split('.')[file.Split('.').Length - 1];
            return extension;
        }
        private string NameFile(string file)
        {
            var extension = GetExtension(file);
            var fileName = DateTime.Now.Ticks + extension;
            return fileName;

        }
    }
}