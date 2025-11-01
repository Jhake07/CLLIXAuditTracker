using CLLIX.TAAuditTracker.Application.Features.BookingReservation.Commands.Create;

namespace CLLIX.TAAuditTracker.Application.ContractInterface
{
    public interface IExcelBookingParser
    {
        List<CreateBookingReservationCommand> ParseSheet(Stream fileStream, string sheetName);
    }
}
