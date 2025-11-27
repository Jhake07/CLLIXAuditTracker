using CLLIX.TAAuditTracker.Application.Features.BookingReservation.Commands.Upload;

namespace CLLIX.TAAuditTracker.Application.ContractInterface
{
    public interface IExcelBookingParser
    {
        List<CreateBookingReservationFromUploadCommand> ParseSheet(Stream fileStream, string sheetName, out List<int> backfilledRowNumbers);
    }
}
