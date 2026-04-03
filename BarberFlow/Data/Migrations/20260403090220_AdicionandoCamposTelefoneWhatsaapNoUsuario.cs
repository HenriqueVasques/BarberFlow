using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BarberFlow.API.Migrations
{
    /// <inheritdoc />
    public partial class AdicionandoCamposTelefoneWhatsaapNoUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bloqueio_Horarios_Empresas_EmpresaId",
                table: "Bloqueio_Horarios");

            migrationBuilder.DropForeignKey(
                name: "FK_Bloqueio_Horarios_Profissionais_ProfissionalId",
                table: "Bloqueio_Horarios");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Bloqueio_Horarios",
                table: "Bloqueio_Horarios");

            migrationBuilder.DropColumn(
                name: "Telefone",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "Whatsapp",
                table: "Clientes");

            migrationBuilder.RenameTable(
                name: "Bloqueio_Horarios",
                newName: "BloqueioHorarios");

            migrationBuilder.RenameIndex(
                name: "IX_Bloqueio_Horarios_ProfissionalId",
                table: "BloqueioHorarios",
                newName: "IX_BloqueioHorarios_ProfissionalId");

            migrationBuilder.RenameIndex(
                name: "IX_Bloqueio_Horarios_EmpresaId",
                table: "BloqueioHorarios",
                newName: "IX_BloqueioHorarios_EmpresaId");

            migrationBuilder.AddColumn<string>(
                name: "Telefone",
                table: "Usuarios",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Whatsapp",
                table: "Usuarios",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<TimeOnly>(
                name: "HoraFechamento",
                table: "HorarioFuncionamentoEmpresas",
                type: "time without time zone",
                nullable: true,
                oldClrType: typeof(TimeOnly),
                oldType: "time without time zone");

            migrationBuilder.AlterColumn<TimeOnly>(
                name: "HoraAbertura",
                table: "HorarioFuncionamentoEmpresas",
                type: "time without time zone",
                nullable: true,
                oldClrType: typeof(TimeOnly),
                oldType: "time without time zone");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BloqueioHorarios",
                table: "BloqueioHorarios",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BloqueioHorarios_Empresas_EmpresaId",
                table: "BloqueioHorarios",
                column: "EmpresaId",
                principalTable: "Empresas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BloqueioHorarios_Profissionais_ProfissionalId",
                table: "BloqueioHorarios",
                column: "ProfissionalId",
                principalTable: "Profissionais",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BloqueioHorarios_Empresas_EmpresaId",
                table: "BloqueioHorarios");

            migrationBuilder.DropForeignKey(
                name: "FK_BloqueioHorarios_Profissionais_ProfissionalId",
                table: "BloqueioHorarios");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BloqueioHorarios",
                table: "BloqueioHorarios");

            migrationBuilder.DropColumn(
                name: "Telefone",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "Whatsapp",
                table: "Usuarios");

            migrationBuilder.RenameTable(
                name: "BloqueioHorarios",
                newName: "Bloqueio_Horarios");

            migrationBuilder.RenameIndex(
                name: "IX_BloqueioHorarios_ProfissionalId",
                table: "Bloqueio_Horarios",
                newName: "IX_Bloqueio_Horarios_ProfissionalId");

            migrationBuilder.RenameIndex(
                name: "IX_BloqueioHorarios_EmpresaId",
                table: "Bloqueio_Horarios",
                newName: "IX_Bloqueio_Horarios_EmpresaId");

            migrationBuilder.AlterColumn<TimeOnly>(
                name: "HoraFechamento",
                table: "HorarioFuncionamentoEmpresas",
                type: "time without time zone",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0),
                oldClrType: typeof(TimeOnly),
                oldType: "time without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<TimeOnly>(
                name: "HoraAbertura",
                table: "HorarioFuncionamentoEmpresas",
                type: "time without time zone",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0),
                oldClrType: typeof(TimeOnly),
                oldType: "time without time zone",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Telefone",
                table: "Clientes",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Whatsapp",
                table: "Clientes",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Bloqueio_Horarios",
                table: "Bloqueio_Horarios",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bloqueio_Horarios_Empresas_EmpresaId",
                table: "Bloqueio_Horarios",
                column: "EmpresaId",
                principalTable: "Empresas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bloqueio_Horarios_Profissionais_ProfissionalId",
                table: "Bloqueio_Horarios",
                column: "ProfissionalId",
                principalTable: "Profissionais",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
