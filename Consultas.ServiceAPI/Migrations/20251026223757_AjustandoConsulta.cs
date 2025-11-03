using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Consultas.ServiceAPI.Migrations
{
    /// <inheritdoc />
    public partial class AjustandoConsulta : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MedicoNome",
                table: "consultas",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PacienteCpf",
                table: "consultas",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PacienteNome",
                table: "consultas",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MedicoNome",
                table: "consultas");

            migrationBuilder.DropColumn(
                name: "PacienteCpf",
                table: "consultas");

            migrationBuilder.DropColumn(
                name: "PacienteNome",
                table: "consultas");
        }
    }
}
