using PropertyBuilding.Core.Const.ErrorMessages;
using PropertyBuilding.Core.Entities;
using PropertyBuilding.Core.Exceptions;
using PropertyBuilding.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PropertyBuilding.Core.Services
{
    public class PropertyImageService : IPropertyImageService
    {
        private readonly IUnitOfWork _unitOfWork;
        public PropertyImageService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IEnumerable<PropertyImage> GetPropertyImages()
        {
            return _unitOfWork.PropertyImageRepository.GetList().ToList();
        }
        public async Task<PropertyImage> SavePropertyImageAsync(PropertyImage PropertyImage)
        {
            await _unitOfWork.PropertyImageRepository.AddAsync(PropertyImage);
            await _unitOfWork.SaveChangesAsync();
            return PropertyImage;
        }

        public async Task<Boolean> DeletePropertyImageAsync(int id)
        {
            await _unitOfWork.PropertyImageRepository.DeleteByIdAsync(id);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<PropertyImage> GetPropertyImageByIdAsync(int id, int idUser)
        {
            var propertyImage = await _unitOfWork.PropertyImageRepository.GetByIdAsync(id);
            if (propertyImage == null) return null;
            if (propertyImage.IdUser != idUser) return null;
            return propertyImage;
        }

        public PropertyImage GetPropertyImageByFileName(string fileName, int idUser)
        {
            var propertyImage = _unitOfWork.PropertyImageRepository.GetPropertyImageByName(fileName, idUser);
            if (propertyImage == null) throw new BusinessException(PropertyImageErrorMessages.ImageNotFound);
            return propertyImage;
        }

        public async Task<PropertyImage> ChangeEnabledPropertyImageAsyn(PropertyImage changeStatePropertyImage, int idUser)
        {
            var propertyImageToUpdate = await _unitOfWork.PropertyImageRepository.GetByIdAsync(changeStatePropertyImage.Id);
            if (propertyImageToUpdate.IdUser != idUser) throw new BusinessException(PropertyImageErrorMessages.ImageNotFound);
            propertyImageToUpdate.Enabled = changeStatePropertyImage.Enabled;
            _unitOfWork.PropertyImageRepository.Update(propertyImageToUpdate);
            await _unitOfWork.SaveChangesAsync();

            return propertyImageToUpdate;
        }
    }
}
