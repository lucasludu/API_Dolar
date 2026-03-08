using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Modificando : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CuotasAuto_CotizacionDolars_CotizacionDolarId",
                table: "CuotasAuto");

            migrationBuilder.DropForeignKey(
                name: "FK_CuotasAuto_PlanAutos_ServicioId",
                table: "CuotasAuto");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlanAutos",
                table: "PlanAutos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CuotasAuto",
                table: "CuotasAuto");

            migrationBuilder.RenameTable(
                name: "PlanAutos",
                newName: "Servicios");

            migrationBuilder.RenameTable(
                name: "CuotasAuto",
                newName: "CuotaServicios");

            migrationBuilder.RenameIndex(
                name: "IX_CuotasAuto_ServicioId",
                table: "CuotaServicios",
                newName: "IX_CuotaServicios_ServicioId");

            migrationBuilder.RenameIndex(
                name: "IX_CuotasAuto_CotizacionDolarId",
                table: "CuotaServicios",
                newName: "IX_CuotaServicios_CotizacionDolarId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Servicios",
                table: "Servicios",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CuotaServicios",
                table: "CuotaServicios",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CuotaServicios_CotizacionDolars_CotizacionDolarId",
                table: "CuotaServicios",
                column: "CotizacionDolarId",
                principalTable: "CotizacionDolars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CuotaServicios_Servicios_ServicioId",
                table: "CuotaServicios",
                column: "ServicioId",
                principalTable: "Servicios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CuotaServicios_CotizacionDolars_CotizacionDolarId",
                table: "CuotaServicios");

            migrationBuilder.DropForeignKey(
                name: "FK_CuotaServicios_Servicios_ServicioId",
                table: "CuotaServicios");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Servicios",
                table: "Servicios");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CuotaServicios",
                table: "CuotaServicios");

            migrationBuilder.RenameTable(
                name: "Servicios",
                newName: "PlanAutos");

            migrationBuilder.RenameTable(
                name: "CuotaServicios",
                newName: "CuotasAuto");

            migrationBuilder.RenameIndex(
                name: "IX_CuotaServicios_ServicioId",
                table: "CuotasAuto",
                newName: "IX_CuotasAuto_ServicioId");

            migrationBuilder.RenameIndex(
                name: "IX_CuotaServicios_CotizacionDolarId",
                table: "CuotasAuto",
                newName: "IX_CuotasAuto_CotizacionDolarId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlanAutos",
                table: "PlanAutos",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CuotasAuto",
                table: "CuotasAuto",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CuotasAuto_CotizacionDolars_CotizacionDolarId",
                table: "CuotasAuto",
                column: "CotizacionDolarId",
                principalTable: "CotizacionDolars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CuotasAuto_PlanAutos_ServicioId",
                table: "CuotasAuto",
                column: "ServicioId",
                principalTable: "PlanAutos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
