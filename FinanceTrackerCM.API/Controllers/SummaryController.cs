using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FinanceTrackerCM.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using FinanceTrackerCM.Application.DTOs;
using FinanceTrackerCM.Domain.Enums;

namespace FinanceTrackerCM.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class SummaryController : ControllerBase
    {
        private readonly IAppDbContext _db;
        private readonly ICurrentUserResolver _currentUser;

        public SummaryController(IAppDbContext db, ICurrentUserResolver currentUser)
        {
            _db = db;
            _currentUser = currentUser;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int? month, [FromQuery] int? year)
        {
            var tenantId = _currentUser.TenantId;
            if (tenantId == Guid.Empty)
                return Unauthorized();

            var trans = _db.Transacoes.AsQueryable();

            // aplicar filtro pelo tenant
            trans = trans.Where(t => t.TenantId == tenantId);

            if (month.HasValue && year.HasValue)
                trans = trans.Where(t => t.DataTransacao.Month == month.Value && t.DataTransacao.Year == year.Value);

            var receitas = await trans.Where(t => t.Tipo == TipoTransacao.Receita).SumAsync(t => (decimal?)t.Valor) ?? 0m;
            var despesas = await trans.Where(t => t.Tipo == TipoTransacao.Despesa).SumAsync(t => (decimal?)t.Valor) ?? 0m;
            var saldoGeral = receitas - despesas;

            var dto = new SummaryDto
            {
                SaldoGeral = saldoGeral,
                Receitas = receitas,
                Despesas = despesas
            };

            return Ok(dto);
        }
    }
}
