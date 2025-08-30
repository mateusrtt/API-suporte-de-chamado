using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api_sistema_de_chamado.Data.Migrations
{
    /// <inheritdoc />
    public partial class CriarTabelaAcessoLogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AcessoLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: true),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IpAcesso = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MetodoHttp = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Endpoint = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataHora = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcessoLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AcessoLogs_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AcessoLogs_UsuarioId",
                table: "AcessoLogs",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AcessoLogs");
        }
    }
}
