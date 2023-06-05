using FluentValidation;
using Library.Entities;

namespace Library.Models.Validators
{
    public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
    {
        public RegisterUserDtoValidator(Context dbContext)
        {
            RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

            RuleFor(x => x.Email)
                .Custom((value, context) =>
                {
                    if (dbContext.Users.Any(x => x.Email == value))
                    {
                        context.AddFailure("Email", "Email already used");
                    }
                });

            RuleFor(x => x.Password)
                .MinimumLength(5);
            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password);

            RuleFor(x => x.Name)
                .Custom((value, context) =>
                {
                    if (dbContext.Users.Any(x => x.Name == value))
                    {
                        context.AddFailure("Name", "Name already used");
                    }
                });
        }
    }
}