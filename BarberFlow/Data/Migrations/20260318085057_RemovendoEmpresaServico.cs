using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BarberFlow.API.Migrations
{
    /// <inheritdoc />
    public partial class RemovendoEmpresaServico : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProfissionalServicos_EmpresaServico_EmpresaServicoId",
                table: "ProfissionalServicos");

            migrationBuilder.DropTable(
                name: "EmpresaServico");

            migrationBuilder.RenameColumn(
                name: "EmpresaServicoId",
                table: "ProfissionalServicos",
                newName: "ServicoId");

            migrationBuilder.RenameIndex(
                name: "IX_ProfissionalServicos_ProfissionalId_EmpresaServicoId",
                table: "ProfissionalServicos",
                newName: "IX_ProfissionalServicos_ProfissionalId_ServicoId");

            migrationBuilder.RenameIndex(
                name: "IX_ProfissionalServicos_EmpresaServicoId",
                table: "ProfissionalServicos",
                newName: "IX_ProfissionalServicos_ServicoId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProfissionalServicos_Servicos_ServicoId",
                table: "ProfissionalServicos",
                column: "ServicoId",
                principalTable: "Servicos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProfissionalServicos_Servicos_ServicoId",
                table: "ProfissionalServicos");

            migrationBuilder.RenameColumn(
                name: "ServicoId",
                table: "ProfissionalServicos",
                newName: "EmpresaServicoId");

            migrationBuilder.RenameIndex(
                name: "IX_ProfissionalServicos_ServicoId",
                table: "ProfissionalServicos",
                newName: "IX_ProfissionalServicos_EmpresaServicoId");

            migrationBuilder.RenameIndex(
                name: "IX_ProfissionalServicos_ProfissionalId_ServicoId",
                table: "ProfissionalServicos",
                newName: "IX_ProfissionalServicos_ProfissionalId_EmpresaServicoId");

            migrationBuilder.CreateTable(
                name: "EmpresaServico",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmpresaId = table.Column<long>(type: "bigint", nullable: false),
                    ServicoId = table.Column<long>(type: "bigint", nullable: false),
                    PrecoPadrao = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmpresaServico", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmpresaServico_Empresas_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmpresaServico_Servicos_ServicoId",
                        column: x => x.ServicoId,
                        principalTable: "Servicos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmpresaServico_EmpresaId_ServicoId",
                table: "EmpresaServico",
                columns: new[] { "EmpresaId", "ServicoId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmpresaServico_ServicoId",
                table: "EmpresaServico",
                column: "ServicoId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProfissionalServicos_EmpresaServico_EmpresaServicoId",
                table: "ProfissionalServicos",
                column: "EmpresaServicoId",
                principalTable: "EmpresaServico",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
