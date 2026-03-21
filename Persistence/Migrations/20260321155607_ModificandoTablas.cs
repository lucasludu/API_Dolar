using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ModificandoTablas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CotizacionesDolar_TiposDolar_TipoDolarId",
                table: "CotizacionesDolar");

            migrationBuilder.DropTable(
                name: "TiposDolar");

            migrationBuilder.DropIndex(
                name: "IX_CotizacionesDolar_TipoDolarId",
                table: "CotizacionesDolar");

            migrationBuilder.DropColumn(
                name: "TipoDolarId",
                table: "CotizacionesDolar");

            migrationBuilder.AddColumn<string>(
                name: "TipoDolar",
                table: "CotizacionesDolar",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TipoDolar",
                table: "CotizacionesDolar");

            migrationBuilder.AddColumn<int>(
                name: "TipoDolarId",
                table: "CotizacionesDolar",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "TiposDolar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposDolar", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CotizacionesDolar_TipoDolarId",
                table: "CotizacionesDolar",
                column: "TipoDolarId");

            migrationBuilder.AddForeignKey(
                name: "FK_CotizacionesDolar_TiposDolar_TipoDolarId",
                table: "CotizacionesDolar",
                column: "TipoDolarId",
                principalTable: "TiposDolar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
