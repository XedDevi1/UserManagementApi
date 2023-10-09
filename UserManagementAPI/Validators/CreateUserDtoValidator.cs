using FluentValidation;
using UserManagementAPI.Dto;

namespace UserManagementAPI.Validators
{
    public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
    {
        public CreateUserDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Age).GreaterThan(0);
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
        }
    }
}
