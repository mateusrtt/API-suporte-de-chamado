using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api_sistema_de_chamado.Data.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarCampoPrioridadeEmChamado : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Prioridade",
                table: "Chamado",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Prioridade",
                table: "Chamado");
        }
    }
}
