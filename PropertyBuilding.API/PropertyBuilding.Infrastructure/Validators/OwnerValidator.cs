using FluentValidation;
using PropertyBuilding.Core.Const.ErrorMessages;
using PropertyBuilding.Core.DTOs;
using System;

namespace PropertyBuilding.Infrastructure.Validators
{
    public class OwnerValidator : AbstractValidator<OwnerDto>
    {
        public OwnerValidator()
        {
            RuleFor(ownerDto => ownerDto.Name)
                .NotNull().WithMessage(OwnerErrorMessages.NameCannotNull);

            RuleFor(ownerDto => ownerDto.Address)
                .NotNull().WithMessage(OwnerErrorMessages.AddressCannotNull);

            RuleFor(ownerDto => ownerDto.Birthday)
                .NotNull().WithMessage(OwnerErrorMessages.BirthdayCannotNull)
                .GreaterThan(DateTime.Now.AddYears(-100).Date).WithMessage(OwnerErrorMessages.BirthdayGreaterThan100Years)
                .LessThan(DateTime.Now.AddYears(-21).Date).WithMessage(OwnerErrorMessages.BirthdayLessThan21Years);
        }
    }
}
