using PropertyBuilding.Core.DTOs;
using PropertyBuilding.Core.Entities;
using PropertyBuilding.Core.Exceptions;
using PropertyBuilding.Core.Interfaces;
using PropertyBuilding.Core.Options;
using PropertyBuilding.Core.QueryFilters;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyBuilding.Core.Const.ErrorMessages;

namespace PropertyBuilding.Core.Services
{
    public class PropertyService : IPropertyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOwnerService _ownerService;
        private readonly PaginationOptions _paginationOptions;
        public PropertyService(IUnitOfWork unitOfWork, IOptions<PaginationOptions> paginationOptions, IOwnerService ownerService)
        {
            _unitOfWork = unitOfWork;
            _paginationOptions = (PaginationOptions)paginationOptions.Value;
            _ownerService = ownerService;
        }
        public PagedListDto<Property> GetProperties(PropertyQueryFilter propertyQueryFilter)
        {
            var properties = _unitOfWork.PropertyRepository.GetList();
            if (propertyQueryFilter.Name != null)
            {
                properties = properties.Where(item => item.Name.ToLower().Contains(propertyQueryFilter.Name.ToLower()));
            }
            if (propertyQueryFilter.Address != null)
            {
                properties = properties.Where(item => item.Address.ToLower().Contains(propertyQueryFilter.Address.ToLower()));
            }
            if (propertyQueryFilter.CodeInternal != null)
            {
                properties = properties.Where(item => item.CodeInternal.ToLower() == propertyQueryFilter.CodeInternal.ToLower());
            }
            if (propertyQueryFilter.IdOwner != null)
            {
                properties = properties.Where(item => item.IdOwner == propertyQueryFilter.IdOwner);
            }
            properties = PriceFilter(properties, propertyQueryFilter);
            properties = YearFilter(properties, propertyQueryFilter);
            var propertiesPagedList = PaginationFilter(properties, propertyQueryFilter);
            return propertiesPagedList;
        }

        public Task<Property> GetPropertyByIdAsync(int id)
        {
            var property = _unitOfWork.PropertyRepository.GetByIdAsync(id);
            return property;
        }

        public async Task<Property> ChangePricePropertyAsync(Property changePriceProperty)
        {
            var propertyToUpdate = await _unitOfWork.PropertyRepository.GetByIdAsync(changePriceProperty.Id);
            propertyToUpdate.Price = changePriceProperty.Price;
            _unitOfWork.PropertyRepository.Update(propertyToUpdate);
            await _unitOfWork.SaveChangesAsync();

            return propertyToUpdate;
        }
        private async Task ValidateOwner(int id)
        {
            if (!await _ownerService.ValidateOwner(id))
            {
                throw new BusinessException(OwnerErrorMessages.OwnerDoesNotExist);
            }
        }

        public async Task<Property> UpdatePropertyAsync(Property property)
        {
            await ValidateOwner(property.IdOwner);
            var propertyOld = await _unitOfWork.PropertyRepository.GetByIdAsync(property.Id);
            if (propertyOld == null) throw new BusinessException(PropertyErrorMessages.PropertyDoesNotExist);
            if (propertyOld.IdUser != property.IdUser)
            {
                throw new BusinessException(PropertyErrorMessages.PropertyUpdateNotAllowed);
            }
            property.Status = propertyOld.Status;
            _unitOfWork.PropertyRepository.Update(property);
            await _unitOfWork.SaveChangesAsync();
            return property;
        }
        public async Task<Property> SavePropertyAsync(Property property)
        {
            await ValidateOwner(property.IdOwner);
            if (property.Id.Equals(0))
            {
                await _unitOfWork.PropertyRepository.AddAsync(property);
            }
            else
            {
                var propertyOld = await _unitOfWork.PropertyRepository.GetByIdAsync(property.Id);
                property.IdUser = propertyOld.IdUser;
                _unitOfWork.PropertyRepository.Update(property);
            }
            await _unitOfWork.SaveChangesAsync();
            return property;
        }

        private static IEnumerable<Property> PriceFilter(IEnumerable<Property> properties, PropertyQueryFilter propertyQueryFilter)
        {
            if (propertyQueryFilter.Price != null)
            {
                properties = properties.Where(item => item.Price == propertyQueryFilter.Price);
            }
            else if (propertyQueryFilter.PriceInitial != null && propertyQueryFilter.PriceFinal != null)
            {
                properties = properties.Where(item => item.Price >= propertyQueryFilter.PriceInitial && item.Price <= propertyQueryFilter.PriceFinal);
            }

            return properties;
        }

        private static IEnumerable<Property> YearFilter(IEnumerable<Property> properties, PropertyQueryFilter propertyQueryFilter)
        {
            if (propertyQueryFilter.Year != null)
            {
                properties = properties.Where(item => item.Year == propertyQueryFilter.Year);
            }
            else if (propertyQueryFilter.YearInitial != null && propertyQueryFilter.YearInitial != null)
            {
                properties = properties.Where(item => item.Year >= propertyQueryFilter.YearInitial && item.Price <= propertyQueryFilter.YearInitial);
            }

            return properties;
        }

        private PagedListDto<Property> PaginationFilter(IEnumerable<Property> properties, PropertyQueryFilter propertyQueryFilter)
        {
            var propertiesPagedList = PagedListDto<Property>.Pagination(properties, propertyQueryFilter.page, propertyQueryFilter.rows, _paginationOptions);

            return propertiesPagedList;
        }

        public async Task<bool> ValidatePropertyAsync(int id)
        {
            return (await _unitOfWork.PropertyRepository.GetByIdAsync(id) != null);
        }
    }
}
