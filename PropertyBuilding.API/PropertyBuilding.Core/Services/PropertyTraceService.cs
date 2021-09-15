using PropertyBuilding.Core.Entities;
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
            await _unitOfWork.PropertyTraceRepository.DeleteByIdAsync(id);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
