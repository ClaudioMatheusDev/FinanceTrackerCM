using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FinanceTrackerCM.Application.Interfaces;

namespace FinanceTrackerCM.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("monthly/pdf")]
        public async Task<IActionResult> MonthlyPdf([FromQuery] Guid tenantId, [FromQuery] int month, [FromQuery] int year)
        {
            var bytes = await _reportService.GenerateMonthlyReportPdfAsync(tenantId, month, year);
            var fileName = $"relatorio_{tenantId}_{year}_{month:00}.pdf";
            return File(bytes, "application/pdf", fileName);
        }

        [HttpGet("monthly/excel")]
        public async Task<IActionResult> MonthlyExcel([FromQuery] Guid tenantId, [FromQuery] int month, [FromQuery] int year)
        {
            var bytes = await _reportService.ExportTransactionsToExcelAsync(tenantId, month, year);
            var fileName = $"relatorio_{tenantId}_{year}_{month:00}.xlsx";
            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
    }
}
