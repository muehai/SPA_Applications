using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace MeetingSchema.API.ViewModels.Validations
{
    public class UserViewModelValidator : AbstractValidator<UserViewModel>
    {
        public UserViewModelValidator()
        {
            RuleFor(user => user.Name).NotEmpty().WithMessage("The name is not nullable");
            RuleFor(user => user.Profession).NotEmpty().WithMessage("The profession is nullable");
            RuleFor(user => user.Avatar).NotEmpty().WithMessage("The avator is nullable");
        }
    }
}
