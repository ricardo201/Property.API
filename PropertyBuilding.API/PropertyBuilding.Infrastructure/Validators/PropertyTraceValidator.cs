using PropertyBuilding.Core.DTOs;
using FluentValidation;
using System;
using PropertyBuilding.Core.Const.ErrorMessages;
using PropertyBuilding.Core.Interfaces;
using System.Threading.Tasks;
using System.Threading;

namespace PropertyBuilding.Infrastructure.Validators
{
    public class PropertyTraceValidator : AbstractValidator<PropertyTraceDto>
    {
        private readonly IPropertyService _propertyService;
        public PropertyTraceValidator(IPropertyService propertyService)
        {
            _propertyService = propertyService;
            RuleFor(propertyTraceDto => propertyTraceDto.IdProperty)
                .NotNull().WithMessage(PropertyErrorMessages.PropertyCannotNull)
                .GreaterThan(0).WithMessage(PropertyErrorMessages.PropertyCannotZero)
                .MustAsync(ExistProperty).WithMessage(PropertyErrorMessages.PropertyDoesNotExist);
            RuleFor(propertyTraceDto => propertyTraceDto.DateSale)
                .LessThan(DateTime.Now.Date).WithMessage(PropertyTraceErrorMessages.DateSaleCannotThanGreaterToday);
        }
        private async Task<bool> ExistProperty(int? idProperty, CancellationToken cancellationToken)
        {
            if (idProperty == null) return false;
            return await _propertyService.ValidatePropertyAsync((int)idProperty);
        }
    }
}
