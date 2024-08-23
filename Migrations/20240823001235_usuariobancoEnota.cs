using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace projFront.Migrations
{
    public partial class usuariobancoEnota : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "NotaFiscal",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Bancos",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "NotaFiscal");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Bancos");
        }
    }
}
