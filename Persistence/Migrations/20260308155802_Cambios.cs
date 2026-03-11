using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Cambios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CotizacionDolars_TipoDolars_TipoDolarId",
                table: "CotizacionDolars");

            migrationBuilder.DropForeignKey(
                name: "FK_CuotaServicios_CotizacionDolars_CotizacionDolarId",
                table: "CuotaServicios");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TipoDolars",
                table: "TipoDolars");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CotizacionDolars",
                table: "CotizacionDolars");

            migrationBuilder.RenameTable(
                name: "TipoDolars",
                newName: "TiposDolar");

            migrationBuilder.RenameTable(
                name: "CotizacionDolars",
                newName: "CotizacionesDolar");

            migrationBuilder.RenameIndex(
                name: "IX_CotizacionDolars_TipoDolarId",
                table: "CotizacionesDolar",
                newName: "IX_CotizacionesDolar_TipoDolarId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TiposDolar",
                table: "TiposDolar",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CotizacionesDolar",
                table: "CotizacionesDolar",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CotizacionesDolar_TiposDolar_TipoDolarId",
                table: "CotizacionesDolar",
                column: "TipoDolarId",
                principalTable: "TiposDolar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CuotaServicios_CotizacionesDolar_CotizacionDolarId",
                table: "CuotaServicios",
                column: "CotizacionDolarId",
                principalTable: "CotizacionesDolar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CotizacionesDolar_TiposDolar_TipoDolarId",
                table: "CotizacionesDolar");

            migrationBuilder.DropForeignKey(
                name: "FK_CuotaServicios_CotizacionesDolar_CotizacionDolarId",
                table: "CuotaServicios");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TiposDolar",
                table: "TiposDolar");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CotizacionesDolar",
                table: "CotizacionesDolar");

            migrationBuilder.RenameTable(
                name: "TiposDolar",
                newName: "TipoDolars");

            migrationBuilder.RenameTable(
                name: "CotizacionesDolar",
                newName: "CotizacionDolars");

            migrationBuilder.RenameIndex(
                name: "IX_CotizacionesDolar_TipoDolarId",
                table: "CotizacionDolars",
                newName: "IX_CotizacionDolars_TipoDolarId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TipoDolars",
                table: "TipoDolars",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CotizacionDolars",
                table: "CotizacionDolars",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CotizacionDolars_TipoDolars_TipoDolarId",
                table: "CotizacionDolars",
                column: "TipoDolarId",
                principalTable: "TipoDolars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CuotaServicios_CotizacionDolars_CotizacionDolarId",
                table: "CuotaServicios",
                column: "CotizacionDolarId",
                principalTable: "CotizacionDolars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
