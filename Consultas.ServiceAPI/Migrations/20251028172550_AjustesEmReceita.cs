using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Consultas.ServiceAPI.Migrations
{
    /// <inheritdoc />
    public partial class AjustesEmReceita : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Receita_consultas_ConsultaId",
                table: "Receita");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Receita",
                table: "Receita");

            migrationBuilder.RenameTable(
                name: "Receita",
                newName: "Receitas");

            migrationBuilder.RenameIndex(
                name: "IX_Receita_ConsultaId",
                table: "Receitas",
                newName: "IX_Receitas_ConsultaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Receitas",
                table: "Receitas",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Receitas_consultas_ConsultaId",
                table: "Receitas",
                column: "ConsultaId",
                principalTable: "consultas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Receitas_consultas_ConsultaId",
                table: "Receitas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Receitas",
                table: "Receitas");

            migrationBuilder.RenameTable(
                name: "Receitas",
                newName: "Receita");

            migrationBuilder.RenameIndex(
                name: "IX_Receitas_ConsultaId",
                table: "Receita",
                newName: "IX_Receita_ConsultaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Receita",
                table: "Receita",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Receita_consultas_ConsultaId",
                table: "Receita",
                column: "ConsultaId",
                principalTable: "consultas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
