using FluentValidation;
using Library.Entities;
using Library.Models.Dto;

namespace Library.Models.Validators
{
    public class RentRoomDtoValidator : AbstractValidator<RentRoomDto>
    {
        public RentRoomDtoValidator(Context _context)
        {
            RuleFor(x => x.Hours)
                .Custom((value, context) =>
                {
                    if (value < 1 || value > 6)
                    {
                        context.AddFailure("Hours", "Wrong number of hours, please choose a number between 1 and 6");
                    }
                });

            RuleFor(x => x.RentTime)
                .Custom((value, context) =>
                {
                    if (value > DateTime.Now.AddMonths(2) || value < DateTime.Now)
                    {
                        context.AddFailure("RentTime", "Wrong date");
                    }
                });
        }
    }
}