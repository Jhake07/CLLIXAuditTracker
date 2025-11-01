using FluentValidation;

namespace CLLIX.TAAuditTracker.Application.Features.TravelAgencyAgent.Commands.Create
{
    public class CreateTravelAgencyAgentCommandValidator : AbstractValidator<CreateTravelAgencyAgentCommand>
    {
        public CreateTravelAgencyAgentCommandValidator()
        {
            RuleFor(x => x.AgentName)
                          .NotEmpty().WithMessage("Agency Code is required.");

            RuleFor(x => x.AgentCode)
               .NotEmpty().WithMessage("Agenc yName is required.");
        }
    }
}
