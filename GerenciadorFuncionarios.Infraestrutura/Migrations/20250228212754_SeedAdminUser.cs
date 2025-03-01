using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GerenciadorFuncionarios.Infraestrutura.Migrations
{
    /// <inheritdoc />
    public partial class SeedAdminUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string senhaHash = BCrypt.Net.BCrypt.HashPassword("admin123");

            migrationBuilder.Sql($@"
            INSERT INTO Funcionarios (Nome, Sobrenome, Email, Documento, DataNascimento, GestorId, SenhaHash) 
            VALUES ('Admin', 'User', 'admin@email.com', '00000000000', '1990-01-01', NULL, '{senhaHash}')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Funcionarios WHERE Email = 'admin@email.com'");
        }
    }
}
