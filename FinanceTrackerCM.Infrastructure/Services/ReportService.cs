using System.Globalization;
using FinanceTrackerCM.Application.Interfaces;
using FinanceTrackerCM.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using ClosedXML.Excel;

namespace FinanceTrackerCM.Infrastructure.Services
{
    public class ReportService : IReportService
    {
        private readonly AppDbContext _db;

        public ReportService(AppDbContext db)
        {
            _db = db;
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public async Task<byte[]> GenerateMonthlyReportPdfAsync(Guid tenantId, int month, int year)
        {
            var transactions = await _db.Transacoes
                .AsNoTracking()
                .Where(t => t.TenantId == tenantId && t.DataTransacao.Month == month && t.DataTransacao.Year == year)
                .Include(t => t.Categoria)
                .Include(t => t.Conta)
                .ToListAsync();

            var title = $"Relatório Mensal - {CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month)} {year}";

            var doc = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(20);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    page.Header().Text(title).SemiBold().FontSize(18).AlignCenter();

                    page.Content().Column(column =>
                    {
                        column.Spacing(5);
                        column.Item().Element(ctx =>
                        {
                            ctx.Container().Padding(5).Row(row =>
                            {
                                row.RelativeItem().Text("Data").Bold();
                                row.RelativeItem().Text("Descrição").Bold();
                                row.RelativeItem().Text("Categoria").Bold();
                                row.RelativeItem().Text("Conta").Bold();
                                row.RelativeItem().Text("Valor").Bold();
                            });
                        });

                        foreach (var t in transactions)
                        {
                            column.Item().Element(ctx =>
                            {
                                ctx.Container().Padding(5).Row(row =>
                                {
                                    row.RelativeItem().Text(t.DataTransacao.ToString("yyyy-MM-dd"));
                                    row.RelativeItem().Text(t.Descricao);
                                    row.RelativeItem().Text(t.Categoria?.NomeCategoria ?? string.Empty);
                                    row.RelativeItem().Text(t.Conta?.NomeConta ?? string.Empty);
                                    row.RelativeItem().Text(t.Valor.ToString("C"));
                                });
                            });
                        }
                    });

                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.Span("Gerado em ");
                        x.Span(DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));
                    });
                });
            });

            return doc.GeneratePdf();
        }

        public async Task<byte[]> ExportTransactionsToExcelAsync(Guid tenantId, int month, int year)
        {
            var transactions = await _db.Transacoes
                .AsNoTracking()
                .Where(t => t.TenantId == tenantId && t.DataTransacao.Month == month && t.DataTransacao.Year == year)
                .Include(t => t.Categoria)
                .Include(t => t.Conta)
                .ToListAsync();

            using var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("Transacoes");
            ws.Cell(1, 1).Value = "Data";
            ws.Cell(1, 2).Value = "Descrição";
            ws.Cell(1, 3).Value = "Categoria";
            ws.Cell(1, 4).Value = "Conta";
            ws.Cell(1, 5).Value = "Valor";

            var row = 2;
            foreach (var t in transactions)
            {
                ws.Cell(row, 1).Value = t.DataTransacao;
                ws.Cell(row, 2).Value = t.Descricao;
                ws.Cell(row, 3).Value = t.Categoria?.NomeCategoria ?? string.Empty;
                ws.Cell(row, 4).Value = t.Conta?.NomeConta ?? string.Empty;
                ws.Cell(row, 5).Value = t.Valor;
                row++;
            }

            ws.Columns().AdjustToContents();

            using var ms = new MemoryStream();
            wb.SaveAs(ms);
            return ms.ToArray();
        }
    }
}
