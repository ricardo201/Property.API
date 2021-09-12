﻿using FluentValidation;
using PropertyBuilding.Core.Const.ErrorMessages;
using PropertyBuilding.Core.DTOs;
using PropertyBuilding.Core.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace PropertyBuilding.Infrastructure.Validators
{
    public class SignInValidator : AbstractValidator<SignInDto>
    {
        private readonly IUserService _userService;
        public SignInValidator(IUserService userService)
        {
            _userService = userService;
            RuleFor(signInDto => signInDto.Password)
                .NotNull().WithMessage(SignInErrorMessages.PasswordCanNotNull)
                .MinimumLength(10).WithMessage(SignInErrorMessages.PasswordCanNotLessThan10)
                .MaximumLength(50).WithMessage(SignInErrorMessages.PasswordCanNotGreaterThan50)
                .Matches("[A-Z]").WithMessage(SignInErrorMessages.PasswordUppercaseLetter)
                .Matches("[a-z]").WithMessage(SignInErrorMessages.PasswordLowercaseLetter)
                .Matches("[0-9]").WithMessage(SignInErrorMessages.PasswordDigit)
                .Matches("[^a-zA-Z0-9]").WithMessage(SignInErrorMessages.PasswordSpecialCharacter);

            RuleFor(signInDto => signInDto.UserName)
                .NotNull().WithMessage(SignInErrorMessages.UserNameCanNotNull)
                .MinimumLength(10).WithMessage(SignInErrorMessages.UserNameCanNotLessThan10)
                .MaximumLength(50).WithMessage(SignInErrorMessages.UserNameCanNotGreaterThan50)
                .MustAsync(UserNotExistValidation).WithMessage(SignInErrorMessages.UserNameExist); ;
        }

        private async Task<bool> UserNotExistValidation(string userName, CancellationToken cancellationToken)
        {
            var exist = await _userService.NotExistUserName(userName);
            return exist;
        }
    }
}