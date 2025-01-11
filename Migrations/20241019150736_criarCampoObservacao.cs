using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace projFront.Migrations
{
    public partial class criarCampoObservacao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Observacoes",
                table: "NotaFiscal",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Observacoes",
                table: "NotaFiscal");
        }
    }
}
