using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pousada.API.Migrations
{
    /// <inheritdoc />
    public partial class RemocaoColunaAtivoItens : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ativo",
                table: "Itens");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Ativo",
                table: "Itens",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
