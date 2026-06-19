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
        private readonly ICurrentUserResolver _currentUser;

        public ReportsController(IReportService reportService, ICurrentUserResolver currentUser)
        {
            _reportService = reportService;
            _currentUser = currentUser;
        }

        [HttpGet("monthly/pdf")]
        public async Task<IActionResult> MonthlyPdf([FromQuery] int month, [FromQuery] int year)
        {
            var tenantId = _currentUser.TenantId;
            if (tenantId == Guid.Empty)
                return Unauthorized();

            var bytes = await _reportService.GenerateMonthlyReportPdfAsync(tenantId, month, year);
            var fileName = $"relatorio_{year}_{month:00}.pdf";
            return File(bytes, "application/pdf", fileName);
        }

        [HttpGet("monthly/excel")]
        public async Task<IActionResult> MonthlyExcel([FromQuery] int month, [FromQuery] int year)
        {
            var tenantId = _currentUser.TenantId;
            if (tenantId == Guid.Empty)
                return Unauthorized();

            var bytes = await _reportService.ExportTransactionsToExcelAsync(tenantId, month, year);
            var fileName = $"relatorio_{year}_{month:00}.xlsx";
            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
    }
}
