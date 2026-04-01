using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BarberFlow.API.Migrations
{
    /// <inheritdoc />
    public partial class AdicionandoCamposHorarioProfissionalTrocandoTiposPropiedadeHorarioFuncionamentoEmpresa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HorarioFuncionamentoEmpresas_Empresas_EmpresaId",
                table: "HorarioFuncionamentoEmpresas");

            migrationBuilder.AlterColumn<TimeOnly>(
                name: "HoraInicioAlmoco",
                table: "HorarioProfissionais",
                type: "time without time zone",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0),
                oldClrType: typeof(TimeOnly),
                oldType: "time without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<TimeOnly>(
                name: "HoraFimAlmoco",
                table: "HorarioProfissionais",
                type: "time without time zone",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0),
                oldClrType: typeof(TimeOnly),
                oldType: "time without time zone",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataAtualizacao",
                table: "HorarioProfissionais",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DataCriacao",
                table: "HorarioProfissionais",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<TimeOnly>(
                name: "HoraFechamento",
                table: "HorarioFuncionamentoEmpresas",
                type: "time without time zone",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0),
                oldClrType: typeof(TimeSpan),
                oldType: "interval",
                oldNullable: true);

            migrationBuilder.AlterColumn<TimeOnly>(
                name: "HoraAbertura",
                table: "HorarioFuncionamentoEmpresas",
                type: "time without time zone",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0),
                oldClrType: typeof(TimeSpan),
                oldType: "interval",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_HorarioFuncionamentoEmpresas_Empresas_EmpresaId",
                table: "HorarioFuncionamentoEmpresas",
                column: "EmpresaId",
                principalTable: "Empresas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HorarioFuncionamentoEmpresas_Empresas_EmpresaId",
                table: "HorarioFuncionamentoEmpresas");

            migrationBuilder.DropColumn(
                name: "DataAtualizacao",
                table: "HorarioProfissionais");

            migrationBuilder.DropColumn(
                name: "DataCriacao",
                table: "HorarioProfissionais");

            migrationBuilder.AlterColumn<TimeOnly>(
                name: "HoraInicioAlmoco",
                table: "HorarioProfissionais",
                type: "time without time zone",
                nullable: true,
                oldClrType: typeof(TimeOnly),
                oldType: "time without time zone");

            migrationBuilder.AlterColumn<TimeOnly>(
                name: "HoraFimAlmoco",
                table: "HorarioProfissionais",
                type: "time without time zone",
                nullable: true,
                oldClrType: typeof(TimeOnly),
                oldType: "time without time zone");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "HoraFechamento",
                table: "HorarioFuncionamentoEmpresas",
                type: "interval",
                nullable: true,
                oldClrType: typeof(TimeOnly),
                oldType: "time without time zone");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "HoraAbertura",
                table: "HorarioFuncionamentoEmpresas",
                type: "interval",
                nullable: true,
                oldClrType: typeof(TimeOnly),
                oldType: "time without time zone");

            migrationBuilder.AddForeignKey(
                name: "FK_HorarioFuncionamentoEmpresas_Empresas_EmpresaId",
                table: "HorarioFuncionamentoEmpresas",
                column: "EmpresaId",
                principalTable: "Empresas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
