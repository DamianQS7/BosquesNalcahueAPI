using BosquesNalcahue.Application.Models;
using FluentValidation;

namespace BosquesNalcahue.Application.Validators
{
    public class GetAllReportsOptionsValidator : AbstractValidator<GetAllReportsOptions>
    {
        public GetAllReportsOptionsValidator()
        {
            RuleFor(x => x.Page).GreaterThanOrEqualTo(1);
            RuleFor(x => x.PageSize).GreaterThanOrEqualTo(1);
        }
    }
}
