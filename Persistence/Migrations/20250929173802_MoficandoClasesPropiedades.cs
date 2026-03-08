using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class MoficandoClasesPropiedades : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CuotasAuto_PlanAutos_PlanAutoId",
                table: "CuotasAuto");

            migrationBuilder.DropColumn(
                name: "FueFeriado",
                table: "CuotasAuto");

            migrationBuilder.DropColumn(
                name: "Observaciones",
                table: "CuotasAuto");

            migrationBuilder.RenameColumn(
                name: "PlanAutoId",
                table: "CuotasAuto",
                newName: "ServicioId");

            migrationBuilder.RenameIndex(
                name: "IX_CuotasAuto_PlanAutoId",
                table: "CuotasAuto",
                newName: "IX_CuotasAuto_ServicioId");

            migrationBuilder.AlterColumn<int>(
                name: "CantidadCuotas",
                table: "PlanAutos",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<bool>(
                name: "Activo",
                table: "PlanAutos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_CuotasAuto_PlanAutos_ServicioId",
                table: "CuotasAuto",
                column: "ServicioId",
                principalTable: "PlanAutos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CuotasAuto_PlanAutos_ServicioId",
                table: "CuotasAuto");

            migrationBuilder.DropColumn(
                name: "Activo",
                table: "PlanAutos");

            migrationBuilder.RenameColumn(
                name: "ServicioId",
                table: "CuotasAuto",
                newName: "PlanAutoId");

            migrationBuilder.RenameIndex(
                name: "IX_CuotasAuto_ServicioId",
                table: "CuotasAuto",
                newName: "IX_CuotasAuto_PlanAutoId");

            migrationBuilder.AlterColumn<int>(
                name: "CantidadCuotas",
                table: "PlanAutos",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "FueFeriado",
                table: "CuotasAuto",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Observaciones",
                table: "CuotasAuto",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_CuotasAuto_PlanAutos_PlanAutoId",
                table: "CuotasAuto",
                column: "PlanAutoId",
                principalTable: "PlanAutos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
