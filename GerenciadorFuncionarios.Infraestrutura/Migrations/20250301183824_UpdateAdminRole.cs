using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GerenciadorFuncionarios.Infraestrutura.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAdminRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                UPDATE Funcionarios 
                SET Role = 'Admin' 
                WHERE Email = 'admin@email.com'
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                UPDATE Funcionarios 
                SET Role = NULL 
                WHERE Email = 'admin@email.com'
            ");
        }
    }
}
