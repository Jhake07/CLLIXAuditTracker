using CLLIX.TAAuditTracker.Application.ContractInterface;
using CLLIX.TAAuditTracker.Application.Features.BookingReservation.Commands.Upload;
using OfficeOpenXml;

namespace CLLIX.TAAuditTracker.Infrastructure.Repositories
{
    public class ExcelBookingParserRepository : IExcelBookingParser
    {
        public List<CreateBookingReservationFromUploadCommand> ParseSheet(Stream fileStream, string sheetName, out List<int> backfilledRowNumbers)
        {
            using var package = new ExcelPackage(fileStream);
            var worksheet = package.Workbook.Worksheets[sheetName];

            if (worksheet == null)
                throw new Exception($"Sheet '{sheetName}' not found.");

            if (worksheet.Dimension == null)
                throw new Exception($"Sheet '{sheetName}' is empty or has no data.");

            var reservations = new List<CreateBookingReservationFromUploadCommand>();
            var pendingRows = new List<CreateBookingReservationFromUploadCommand>();
            var backfilledRowLog = new List<(int RowNumber, string? InvoiceNumber)>();

            for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
            {
                var invoice = worksheet.Cells[row, 1].Text?.Trim();
                var billing = worksheet.Cells[row, 3].Text?.Trim();
                var stat = worksheet.Cells[row, 24].Text?.Trim();
                var remark = worksheet.Cells[row, 25].Text?.Trim();

                var command = new CreateBookingReservationFromUploadCommand
                {
                    RowNumber = row,
                    ApartmentPropertyName = worksheet.ToString(),
                    InvoiceNumber = invoice,
                    StatementNumber = worksheet.Cells[row, 2].Text,
                    TravelAgentBilling = billing,
                    ReservationNumber = worksheet.Cells[row, 4].Text,
                    ConfirmationNumber = worksheet.Cells[row, 5].Text,
                    CheckInDate = DateTime.TryParse(worksheet.Cells[row, 6].Text, out var checkIn) ? checkIn : null,
                    CheckOutDate = DateTime.TryParse(worksheet.Cells[row, 7].Text, out var checkOut) ? checkOut : null,
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
                };

                bool hasInvoice = !string.IsNullOrWhiteSpace(invoice);

                if (!hasInvoice)
                {
                    pendingRows.Add(command);
                    continue;
                }

                foreach (var pending in pendingRows)
                {
                    pending.InvoiceNumber = invoice;
                    pending.TravelAgentBilling = billing;
                    pending.Status = stat;
                    pending.Remarks = remark;
                    reservations.Add(pending);
                    backfilledRowLog.Add((pending.RowNumber, invoice!));
                }

                pendingRows.Clear();
                // Skip anchor row
                continue;
            }

            backfilledRowNumbers = backfilledRowLog.Select(x => x.RowNumber).ToList();
            return reservations;
        }

    }
}