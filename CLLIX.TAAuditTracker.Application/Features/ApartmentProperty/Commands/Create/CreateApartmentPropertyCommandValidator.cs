using FluentValidation;

namespace CLLIX.TAAuditTracker.Application.Features.ApartmentProperty.Commands.Create
{
    public class CreateApartmentPropertyCommandValidator : AbstractValidator<CreateApartmentPropertyCommand>
    {
        public CreateApartmentPropertyCommandValidator()
        {
            RuleFor(x => x.ApartmentName)
               .NotEmpty().WithMessage("ApartmentName is required.");
        }
    }
}
