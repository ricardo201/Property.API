using PropertyBuilding.Core.Const.ErrorMessages;
using PropertyBuilding.Core.Entities;
using PropertyBuilding.Core.Exceptions;
using PropertyBuilding.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyBuilding.Core.Services
{
    public class PropertyTraceService : IPropertyTraceService
    {
        private readonly IUnitOfWork _unitOfWork;
        public PropertyTraceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IEnumerable<PropertyTrace> GetPropertyTraces()
        {
            return _unitOfWork.PropertyTraceRepository.GetList().ToList();
        }

        public async Task<PropertyTrace> GetPropertyTraceAsync(int id)
        {
            return await _unitOfWork.PropertyTraceRepository.GetByIdAsync(id);
        }

        public async Task<PropertyTrace> SavePropertyTraceAsync(PropertyTrace PropertyTrace)
        {
            await _unitOfWork.PropertyTraceRepository.AddAsync(PropertyTrace);
            await _unitOfWork.SaveChangesAsync();
            return PropertyTrace;
        }

        public async Task<Boolean> DeletePropertyTraceAsync(int id)
        {
            try
            {
                await _unitOfWork.PropertyTraceRepository.DeleteByIdAsync(id);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch
            {
                ///TODO: implements logger
                return false;
            }
        }

        public async Task<PropertyTrace> UpdatePropertyTraceAsyn(PropertyTrace propertyTrace)
        {
            var propertyOld = await _unitOfWork.PropertyTraceRepository.GetByIdAsync(propertyTrace.Id);            
            if (propertyOld == null) throw new BusinessException(PropertyTraceErrorMessages.PropertyTraceDoesNotExist);
            if (propertyOld.IdUser != propertyTrace.IdUser)
            {
                throw new BusinessException(PropertyErrorMessages.PropertyUpdateNotAllowed);
            }
            propertyTrace.Status = propertyOld.Status;
            _unitOfWork.PropertyTraceRepository.Update(propertyTrace);
            await _unitOfWork.SaveChangesAsync();
            return propertyTrace;
        }
    }
}
