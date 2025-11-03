using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Consultas.ServiceAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddReceitas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Receita",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ConsultaId = table.Column<long>(type: "INTEGER", nullable: false),
                    PacienteId = table.Column<long>(type: "INTEGER", nullable: false),
                    PacienteNome = table.Column<string>(type: "TEXT", nullable: true),
                    MedicoId = table.Column<long>(type: "INTEGER", nullable: false),
                    MedicoNome = table.Column<string>(type: "TEXT", nullable: true),
                    CRM = table.Column<string>(type: "TEXT", nullable: true),
                    DataReceita = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Descricao = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Receita", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Receita_consultas_ConsultaId",
                        column: x => x.ConsultaId,
                        principalTable: "consultas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Receita_ConsultaId",
                table: "Receita",
                column: "ConsultaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Receita");
        }
    }
}
