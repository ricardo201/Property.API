using FluentValidation;
using PropertyBuilding.Core.Const.PropertyImages;
using PropertyBuilding.Core.Const.ErrorMessages;
using PropertyBuilding.Core.DTOs;
using PropertyBuilding.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PropertyBuilding.Infrastructure.Validators
{
    public class PropertyImageValidator : AbstractValidator<PropertyImageDto>
    {
        private readonly IPropertyService _propertyService;
        public PropertyImageValidator(IPropertyService propertyService)
        {
            _propertyService = propertyService;
            RuleFor(propertyImageDto => propertyImageDto.IdProperty)
                .NotNull().WithMessage(PropertyErrorMessages.PropertyCannotNull)
                .GreaterThan(0).WithMessage(PropertyErrorMessages.PropertyCannotZero)
                .MustAsync(ExistProperty).WithMessage(PropertyErrorMessages.PropertyDoesNotExist);
            RuleFor(propertyImageDto => propertyImageDto.Enabled)
                .NotNull().WithMessage(PropertyImageErrorMessages.EnableIsRequired);
            RuleFor(propertyImageDto => propertyImageDto.BlobFile)
                .NotNull().WithMessage(PropertyImageErrorMessages.FileIsRequired)
                .DependentRules(() =>
                    {
                        RuleFor(propertyImageDto => propertyImageDto.BlobFile.Length)
                            .NotNull().WithMessage(PropertyImageErrorMessages.FileCannotNull)
                            .LessThanOrEqualTo(PropertyImagesLimits.SizeFileLimitInBytes).WithMessage(PropertyImageErrorMessages.SizeLimit);
                        RuleFor(propertyImageDto => propertyImageDto.BlobFile.ContentType)
                            .NotNull().WithMessage(PropertyImageErrorMessages.FileCannotNull)
                            .Must(x => x.Equals(PropertyImagesContentTypes.Jpeg) || x.Equals(PropertyImagesContentTypes.Jpg) || x.Equals(PropertyImagesContentTypes.Png))
                            .WithMessage(PropertyImageErrorMessages.ContentType);
                    });
        }

        private async Task<bool> ExistProperty(int? idProperty, CancellationToken cancellationToken)
        {
            if (idProperty == null) return false;
            return await _propertyService.ValidatePropertyAsync((int)idProperty);
        }
    }
}