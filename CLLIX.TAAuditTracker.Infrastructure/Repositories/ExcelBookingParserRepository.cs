using CLLIX.TAAuditTracker.Application.ContractInterface;
using CLLIX.TAAuditTracker.Application.Features.BookingReservation.Commands.Create;
using OfficeOpenXml;

namespace CLLIX.TAAuditTracker.Infrastructure.Repositories
{
    public class ExcelBookingParserRepository : IExcelBookingParser
    {
        public List<CreateBookingReservationCommand> ParseSheet(Stream fileStream, string sheetName)
        {
            using var package = new ExcelPackage(fileStream);
            var worksheet = package.Workbook.Worksheets[sheetName];

            if (worksheet == null)
                throw new Exception($"Sheet '{sheetName}' not found.");

            if (worksheet.Dimension == null)
                throw new Exception($"Sheet '{sheetName}' is empty or has no data.");

            var reservations = new List<CreateBookingReservationCommand>();

            for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
            {
                try
                {
                    DateTime? checkInDate = DateTime.TryParse(worksheet.Cells[row, 6].Text, out var parsedCheckIn)
                        ? parsedCheckIn
                        : null;

                    DateTime? checkOutDate = DateTime.TryParse(worksheet.Cells[row, 7].Text, out var parsedCheckOut)
                        ? parsedCheckOut
                        : null;

                    var command = new CreateBookingReservationCommand
                    {
                        ApartmentPropertyName = worksheet.ToString(),
                        InvoiceNumber = worksheet.Cells[row, 1].Text,
                        StatementNumber = worksheet.Cells[row, 2].Text,
                        TravelAgentBilling = worksheet.Cells[row, 3].Text,
                        ReservationNumber = worksheet.Cells[row, 4].Text,
                        ConfirmationNumber = worksheet.Cells[row, 5].Text,
                        CheckInDate = checkInDate,
                        CheckOutDate = checkOutDate,
                        Nights = int.TryParse(worksheet.Cells[row, 8].Text, out var nights) ? nights : 0,
                        GuestName = worksheet.Cells[row, 9].Text,
                        TravelAgentName = worksheet.Cells[row, 10].Text,
                        BookingSource = worksheet.Cells[row, 11].Text,
                        CommissionRate = worksheet.Cells[row, 12].Text,
                        DailyTariff = decimal.TryParse(worksheet.Cells[row, 13].Text, out var dailyTariff) ? dailyTariff : 0,
                        TotalTariff = decimal.TryParse(worksheet.Cells[row, 14].Text, out var totalTariff) ? totalTariff : 0,
                        TotalCommission = decimal.TryParse(worksheet.Cells[row, 15].Text, out var totalCommission) ? totalCommission : 0,
                        AmountInTAInvoice = decimal.TryParse(worksheet.Cells[row, 16].Text, out var amountInTAInvoice) ? amountInTAInvoice : 0,
                        IsInvoiceMatched = worksheet.Cells[row, 17].Text.Trim().ToUpper() == "TRUE",
                        InvoiceRemarks = worksheet.Cells[row, 18].Text,
                        WeekNumber = int.TryParse(worksheet.Cells[row, 19].Text, out var weekNumber) ? weekNumber : null,
                        InvoiceReceivedDate = DateTime.TryParse(worksheet.Cells[row, 20].Text, out var invoiceReceivedDate) ? invoiceReceivedDate : null,
                        InvoiceProcessDate = DateTime.TryParse(worksheet.Cells[row, 21].Text, out var invoiceProcessDate) ? invoiceProcessDate : null,
                        DueDate = DateTime.TryParse(worksheet.Cells[row, 22].Text, out var dueDate) ? dueDate : null,
                        RemittanceDate = DateTime.TryParse(worksheet.Cells[row, 23].Text, out var remittanceDate) ? remittanceDate : null,
                        Status = worksheet.Cells[row, 24].Text,
                        Remarks = worksheet.Cells[row, 25].Text,
                        CreatedBy = "System",
                        CreatedDate = DateTime.UtcNow,
                        //ModifiedBy = "System",
                        //ModifiedDate = DateTime.UtcNow
                    };

                    reservations.Add(command);
                }
                catch (Exception ex)
                {
                    // Optionally log or collect row-level errors
                    throw new Exception($"Error parsing row {row}: {ex.Message}", ex);
                }
            }

            return reservations;
        }
    }
}