using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace projFront.Migrations
{
    public partial class retirandoObrigatorio : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NotaFiscal_Bancos_BancoIdBanco",
                table: "NotaFiscal");

            migrationBuilder.DropForeignKey(
                name: "FK_NotaFiscal_Empresas_EmpresaIdEmpresa",
                table: "NotaFiscal");

            migrationBuilder.DropIndex(
                name: "IX_NotaFiscal_BancoIdBanco",
                table: "NotaFiscal");

            migrationBuilder.DropIndex(
                name: "IX_NotaFiscal_EmpresaIdEmpresa",
                table: "NotaFiscal");

            migrationBuilder.DropColumn(
                name: "BancoIdBanco",
                table: "NotaFiscal");

            migrationBuilder.DropColumn(
                name: "EmpresaIdEmpresa",
                table: "NotaFiscal");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BancoIdBanco",
                table: "NotaFiscal",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EmpresaIdEmpresa",
                table: "NotaFiscal",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_NotaFiscal_BancoIdBanco",
                table: "NotaFiscal",
                column: "BancoIdBanco");

            migrationBuilder.CreateIndex(
                name: "IX_NotaFiscal_EmpresaIdEmpresa",
                table: "NotaFiscal",
                column: "EmpresaIdEmpresa");

            migrationBuilder.AddForeignKey(
                name: "FK_NotaFiscal_Bancos_BancoIdBanco",
                table: "NotaFiscal",
                column: "BancoIdBanco",
                principalTable: "Bancos",
                principalColumn: "IdBanco",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NotaFiscal_Empresas_EmpresaIdEmpresa",
                table: "NotaFiscal",
                column: "EmpresaIdEmpresa",
                principalTable: "Empresas",
                principalColumn: "IdEmpresa",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
