using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BarberFlow.API.Migrations
{
    /// <inheritdoc />
    public partial class AdicionandoCamposNoHorarioProfissionalTambemNoProfissionalServico : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Ativo",
                table: "ProfissionalServicos",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataAtualizacao",
                table: "ProfissionalServicos",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "DuracaoPersonalizadaMinutos",
                table: "ProfissionalServicos",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ProfissionalServicos",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Ativo",
                table: "HorarioProfissionais",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<TimeOnly>(
                name: "HoraFimAlmoco",
                table: "HorarioProfissionais",
                type: "time without time zone",
                nullable: true);

            migrationBuilder.AddColumn<TimeOnly>(
                name: "HoraInicioAlmoco",
                table: "HorarioProfissionais",
                type: "time without time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "HorarioProfissionais",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                name: "ProfissionalId1",
                table: "HorarioProfissionais",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HorarioProfissionais_ProfissionalId1",
                table: "HorarioProfissionais",
                column: "ProfissionalId1");

            migrationBuilder.AddForeignKey(
                name: "FK_HorarioProfissionais_Profissionais_ProfissionalId1",
                table: "HorarioProfissionais",
                column: "ProfissionalId1",
                principalTable: "Profissionais",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HorarioProfissionais_Profissionais_ProfissionalId1",
                table: "HorarioProfissionais");

            migrationBuilder.DropIndex(
                name: "IX_HorarioProfissionais_ProfissionalId1",
                table: "HorarioProfissionais");

            migrationBuilder.DropColumn(
                name: "Ativo",
                table: "ProfissionalServicos");

            migrationBuilder.DropColumn(
                name: "DataAtualizacao",
                table: "ProfissionalServicos");

            migrationBuilder.DropColumn(
                name: "DuracaoPersonalizadaMinutos",
                table: "ProfissionalServicos");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ProfissionalServicos");

            migrationBuilder.DropColumn(
                name: "Ativo",
                table: "HorarioProfissionais");

            migrationBuilder.DropColumn(
                name: "HoraFimAlmoco",
                table: "HorarioProfissionais");

            migrationBuilder.DropColumn(
                name: "HoraInicioAlmoco",
                table: "HorarioProfissionais");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "HorarioProfissionais");

            migrationBuilder.DropColumn(
                name: "ProfissionalId1",
                table: "HorarioProfissionais");
        }
    }
}
