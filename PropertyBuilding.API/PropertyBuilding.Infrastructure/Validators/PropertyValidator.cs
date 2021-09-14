using FluentValidation;
using PropertyBuilding.Core.Const.ErrorMessages;
using PropertyBuilding.Core.DTOs;

namespace PropertyBuilding.Infrastructure.Validators
{
    public class PropertyValidator : AbstractValidator<PropertyDto>
    {
        public PropertyValidator()
        {
            RuleFor(propertyDto => propertyDto.IdOwner)
                .NotNull().WithMessage(OwnerErrorMessages.IdOwnerCannotNull)
                .GreaterThan(0).WithMessage(OwnerErrorMessages.IdOwnerCannotZero);

            RuleFor(propertyDto => propertyDto.Year)
                .NotNull().WithMessage(PropertyErrorMessages.YearCannotNull)
                .GreaterThan(1900).WithMessage(PropertyErrorMessages.YearLessThan1900);

            RuleFor(propertyDto => propertyDto.Name)
                .NotNull().WithMessage(PropertyErrorMessages.NameCannotNull)
                .Length(3, 100).WithMessage(PropertyErrorMessages.NameLength);
        }
    }
}