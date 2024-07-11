using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace projFront.Migrations
{
    public partial class Script_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NotaFiscal",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", nullable: false),
                    Cnpj = table.Column<string>(type: "TEXT", nullable: false),
                    IncricaoEstadual = table.Column<string>(type: "TEXT", nullable: false),
                    Endereco = table.Column<string>(type: "TEXT", nullable: false),
                    Numero = table.Column<string>(type: "TEXT", nullable: false),
                    Bairro = table.Column<string>(type: "TEXT", nullable: false),
                    NomeCidade = table.Column<string>(type: "TEXT", nullable: false),
                    Uf = table.Column<string>(type: "TEXT", nullable: false),
                    Cep = table.Column<string>(type: "TEXT", nullable: false),
                    NumeroTelefone = table.Column<string>(type: "TEXT", nullable: false),
                    DescricaoServico = table.Column<string>(type: "TEXT", nullable: false),
                    ValorTotal = table.Column<decimal>(type: "TEXT", nullable: false),
                    Banco = table.Column<string>(type: "TEXT", nullable: false),
                    Agencia = table.Column<string>(type: "TEXT", nullable: false),
                    Conta = table.Column<string>(type: "TEXT", nullable: false),
                    PixChave = table.Column<string>(type: "TEXT", nullable: false),
                    PixNumero = table.Column<string>(type: "TEXT", nullable: false),
                    IdEmpresa = table.Column<int>(type: "INTEGER", nullable: false),
                    DataEmissao = table.Column<DateTime>(type: "TEXT", nullable: true),
                    FaturaSerie = table.Column<string>(type: "TEXT", nullable: false),
                    FaturaNumero = table.Column<int>(type: "INTEGER", nullable: false),
                    MensagemFisco = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotaFiscal", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NotaFiscal");
        }
    }
}
