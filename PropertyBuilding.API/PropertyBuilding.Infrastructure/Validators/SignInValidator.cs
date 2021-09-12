using FluentValidation;
using PropertyBuilding.Core.Const.ErrorMessages;
using PropertyBuilding.Core.DTOs;
using PropertyBuilding.Core.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace PropertyBuilding.Infrastructure.Validators
{
    public class SignInValidator : AbstractValidator<SignInDto>
    {        
        public SignInValidator()
        {
            RuleFor(signInDto => signInDto.Password)
                .NotNull()
                .MinimumLength(10)
                .MaximumLength(50)
                .Matches("[A-Z]").WithMessage(SignInErrorMessages.PasswordUppercaseLetter)
                .Matches("[a-z]").WithMessage(SignInErrorMessages.PasswordLowercaseLetter)
                .Matches("[0-9]").WithMessage(SignInErrorMessages.PasswordDigit)
                .Matches("[^a-zA-Z0-9]").WithMessage(SignInErrorMessages.PasswordSpecialCharacter);

            RuleFor(signInDto => signInDto.UserName)
                .NotNull()
                .MinimumLength(10)
                .MaximumLength(50);
        }
    }
}