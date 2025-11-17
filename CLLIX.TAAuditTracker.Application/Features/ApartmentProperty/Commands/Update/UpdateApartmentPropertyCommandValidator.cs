using CLLIX.TAAuditTracker.Application.ContractInterface;
using FluentValidation;

namespace CLLIX.TAAuditTracker.Application.Features.ApartmentProperty.Commands.Update
{
    public class UpdateApartmentPropertyCommandValidator : AbstractValidator<UpdateApartmentPropertyCommand>
    {
        public UpdateApartmentPropertyCommandValidator(IApartmentPropertyRepository repository)
        {
            RuleFor(x => x.NewApartmentName)
                .NotEmpty().WithMessage("Apartment name is required.")
                .MustAsync(async (command, name, cancellation) =>
                {
                    var existing = await repository.CheckExistingApartmentName(name);

                    // If no match, it's safe to proceed
                    if (existing == null)
                        return true;

                    // If match is the same record, it's safe to proceed
                    return existing.Id == command.Id;
                })
                .WithMessage("An apartment with the same name already exists.");

        }
    }
}
