using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace projFront.Migrations
{
    public partial class AdicionandoCampoUser1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumeroDaNotaAtual",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UltimoNumeroDaNota",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumeroDaNotaAtual",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UltimoNumeroDaNota",
                table: "AspNetUsers");
        }
    }
}
