using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BarberFlow.API.Migrations
{
    /// <inheritdoc />
    public partial class AjustandoRelacionamentoProfissionalHorario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Agendamentos_Profissionais_ProfissionalId",
                table: "Agendamentos");

            migrationBuilder.DropForeignKey(
                name: "FK_Agendamentos_Servicos_ServicoId",
                table: "Agendamentos");

            migrationBuilder.RenameColumn(
                name: "ServicoId",
                table: "Agendamentos",
                newName: "ProfissionalServicoId");

            migrationBuilder.RenameIndex(
                name: "IX_Agendamentos_ServicoId",
                table: "Agendamentos",
                newName: "IX_Agendamentos_ProfissionalServicoId");

            migrationBuilder.AlterColumn<long>(
                name: "ProfissionalId",
                table: "Agendamentos",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_Agendamentos_Profissionais_ProfissionalId",
                table: "Agendamentos",
                column: "ProfissionalId",
                principalTable: "Profissionais",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Agendamentos_ProfissionalServicos_ProfissionalServicoId",
                table: "Agendamentos",
                column: "ProfissionalServicoId",
                principalTable: "ProfissionalServicos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Agendamentos_Profissionais_ProfissionalId",
                table: "Agendamentos");

            migrationBuilder.DropForeignKey(
                name: "FK_Agendamentos_ProfissionalServicos_ProfissionalServicoId",
                table: "Agendamentos");

            migrationBuilder.RenameColumn(
                name: "ProfissionalServicoId",
                table: "Agendamentos",
                newName: "ServicoId");

            migrationBuilder.RenameIndex(
                name: "IX_Agendamentos_ProfissionalServicoId",
                table: "Agendamentos",
                newName: "IX_Agendamentos_ServicoId");

            migrationBuilder.AlterColumn<long>(
                name: "ProfissionalId",
                table: "Agendamentos",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Agendamentos_Profissionais_ProfissionalId",
                table: "Agendamentos",
                column: "ProfissionalId",
                principalTable: "Profissionais",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Agendamentos_Servicos_ServicoId",
                table: "Agendamentos",
                column: "ServicoId",
                principalTable: "Servicos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
