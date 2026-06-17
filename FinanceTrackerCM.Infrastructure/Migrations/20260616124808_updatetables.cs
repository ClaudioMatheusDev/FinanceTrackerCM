using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceTrackerCM.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatetables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdCategoria",
                table: "Transacoes");

            migrationBuilder.DropColumn(
                name: "IdConta",
                table: "Transacoes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "IdCategoria",
                table: "Transacoes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "IdConta",
                table: "Transacoes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
