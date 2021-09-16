﻿using PropertyBuilding.Core.Const.ErrorMessages;
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
    public class OwnerService : IOwnerService
    {
        private readonly IUnitOfWork _unitOfWork;
        public OwnerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IEnumerable<Owner> GetOwners()
        {
            return _unitOfWork.OwnerRepository.GetList().ToList();
        }

        public async Task<Owner> GetOwnerAsync(int id)
        {
            return await _unitOfWork.OwnerRepository.GetByIdAsync(id);
        }

        public async Task<Owner> SaveOwnerAsync(Owner owner)
        {
            await _unitOfWork.OwnerRepository.AddAsync(owner);
            await _unitOfWork.SaveChangesAsync();
            return owner;
        }

        public async Task<Boolean> DeleteOwnerAsync(int id)
        {
            try
            {
                await _unitOfWork.OwnerRepository.DeleteByIdAsync(id);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch
            {
                ///TODO: Implements logger
                return false;
            }
        }
        public async Task<bool> ValidateOwner(int id)
        {
            var owner = await _unitOfWork.OwnerRepository.GetByIdAsync(id);

            return (owner != null);
        }

        public async Task<Owner> UpdateOwnerAsync(Owner owner, int idUser)
        {           
            var ownerOld = await _unitOfWork.OwnerRepository.GetByIdAsync(owner.Id);
            if (ownerOld == null) throw new BusinessException(OwnerErrorMessages.OwnerDoesNotExist);
            if (ownerOld.IdUser != owner.IdUser)
            {
                throw new BusinessException(OwnerErrorMessages.OwnerUpdateNotAllowed);
            }
            owner.Status = ownerOld.Status;
            _unitOfWork.OwnerRepository.Update(owner);
            await _unitOfWork.SaveChangesAsync();
            return owner;
        }
    }
}
