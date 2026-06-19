namespace FinanceTrackerCM.Application.Interfaces
{
    public interface IReportService
    {
        Task<byte[]> GenerateMonthlyReportPdfAsync(Guid tenantId, int month, int year);
        Task<byte[]> ExportTransactionsToExcelAsync(Guid tenantId, int month, int year);
    }
}
