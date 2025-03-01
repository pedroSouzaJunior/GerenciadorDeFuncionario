using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GerenciadorFuncionarios.Infraestrutura.Migrations
{
    /// <inheritdoc />
    public partial class AddRoleToFuncionario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "Funcionarios",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "Funcionarios");
        }
    }
}
