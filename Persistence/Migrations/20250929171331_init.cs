using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlanAutos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CantidadCuotas = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanAutos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TipoDolars",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoDolars", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CotizacionDolars",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Compra = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Venta = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TipoDolarId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CotizacionDolars", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CotizacionDolars_TipoDolars_TipoDolarId",
                        column: x => x.TipoDolarId,
                        principalTable: "TipoDolars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CuotasAuto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumeroCuota = table.Column<int>(type: "int", nullable: false),
                    FechaPago = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MontoARS = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PlanAutoId = table.Column<int>(type: "int", nullable: false),
                    CotizacionDolarId = table.Column<int>(type: "int", nullable: false),
                    FueFeriado = table.Column<bool>(type: "bit", nullable: false),
                    Observaciones = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CuotasAuto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CuotasAuto_CotizacionDolars_CotizacionDolarId",
                        column: x => x.CotizacionDolarId,
                        principalTable: "CotizacionDolars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CuotasAuto_PlanAutos_PlanAutoId",
                        column: x => x.PlanAutoId,
                        principalTable: "PlanAutos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CotizacionDolars_TipoDolarId",
                table: "CotizacionDolars",
                column: "TipoDolarId");

            migrationBuilder.CreateIndex(
                name: "IX_CuotasAuto_CotizacionDolarId",
                table: "CuotasAuto",
                column: "CotizacionDolarId");

            migrationBuilder.CreateIndex(
                name: "IX_CuotasAuto_PlanAutoId",
                table: "CuotasAuto",
                column: "PlanAutoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CuotasAuto");

            migrationBuilder.DropTable(
                name: "CotizacionDolars");

            migrationBuilder.DropTable(
                name: "PlanAutos");

            migrationBuilder.DropTable(
                name: "TipoDolars");
        }
    }
}
