using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GerenciadorFuncionarios.Infraestrutura.Migrations
{
    /// <inheritdoc />
    public partial class SeedNormalUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string senhaUser = BCrypt.Net.BCrypt.HashPassword("user123");

            migrationBuilder.Sql($@"
            INSERT INTO Funcionarios (Nome, Sobrenome, Email, Documento, DataNascimento, GestorId, SenhaHash, Role) 
            VALUES ('Usuario', 'Comum', 'user@email.com', '40792326503', '1995-05-15', NULL, '{senhaUser}', 'Usuario')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Funcionarios WHERE Email IN ('user@email.com')");
        }
    }
}
