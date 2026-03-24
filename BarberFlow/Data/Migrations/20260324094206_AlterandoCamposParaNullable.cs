using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BarberFlow.API.Migrations
{
    /// <inheritdoc />
    public partial class AlterandoCamposParaNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<TimeSpan>(
                name: "HoraFechamento",
                table: "HorarioFuncionamentoEmpresas",
                type: "interval",
                nullable: true,
                oldClrType: typeof(TimeSpan),
                oldType: "interval");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "HoraAbertura",
                table: "HorarioFuncionamentoEmpresas",
                type: "interval",
                nullable: true,
                oldClrType: typeof(TimeSpan),
                oldType: "interval");

            migrationBuilder.AddColumn<bool>(
                name: "Ativo",
                table: "HorarioFuncionamentoEmpresas",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataAtualizacao",
                table: "HorarioFuncionamentoEmpresas",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DataCriacao",
                table: "HorarioFuncionamentoEmpresas",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "HorarioFuncionamentoEmpresas",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ativo",
                table: "HorarioFuncionamentoEmpresas");

            migrationBuilder.DropColumn(
                name: "DataAtualizacao",
                table: "HorarioFuncionamentoEmpresas");

            migrationBuilder.DropColumn(
                name: "DataCriacao",
                table: "HorarioFuncionamentoEmpresas");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "HorarioFuncionamentoEmpresas");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "HoraFechamento",
                table: "HorarioFuncionamentoEmpresas",
                type: "interval",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0),
                oldClrType: typeof(TimeSpan),
                oldType: "interval",
                oldNullable: true);

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "HoraAbertura",
                table: "HorarioFuncionamentoEmpresas",
                type: "interval",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0),
                oldClrType: typeof(TimeSpan),
                oldType: "interval",
                oldNullable: true);
        }
    }
}
