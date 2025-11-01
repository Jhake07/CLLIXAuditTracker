using FluentValidation;

namespace CLLIX.TAAuditTracker.Application.Features.TravelAgency.Commands.Create
{
    public class CreateTravelAgencyCommandValidator : AbstractValidator<CreateTravelAgencyCommand>
    {
        public CreateTravelAgencyCommandValidator()
        {
            RuleFor(x => x.AgencyCode)
               .NotEmpty().WithMessage("Agency Code is required.");

            RuleFor(x => x.AgencyName)
               .NotEmpty().WithMessage("Agenc yName is required.");
        }
    }
}
