using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BarberFlow.API.Migrations
{
    /// <inheritdoc />
    public partial class AtualizandoCamposNoHorariosFuncionamento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HoraFim",
                table: "HorarioFuncionamentoEmpresas");

            migrationBuilder.DropColumn(
                name: "HoraInicio",
                table: "HorarioFuncionamentoEmpresas");

            migrationBuilder.AddColumn<bool>(
                name: "EstaFechado",
                table: "HorarioFuncionamentoEmpresas",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "HoraAbertura",
                table: "HorarioFuncionamentoEmpresas",
                type: "interval",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "HoraFechamento",
                table: "HorarioFuncionamentoEmpresas",
                type: "interval",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EstaFechado",
                table: "HorarioFuncionamentoEmpresas");

            migrationBuilder.DropColumn(
                name: "HoraAbertura",
                table: "HorarioFuncionamentoEmpresas");

            migrationBuilder.DropColumn(
                name: "HoraFechamento",
                table: "HorarioFuncionamentoEmpresas");

            migrationBuilder.AddColumn<TimeOnly>(
                name: "HoraFim",
                table: "HorarioFuncionamentoEmpresas",
                type: "time without time zone",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.AddColumn<TimeOnly>(
                name: "HoraInicio",
                table: "HorarioFuncionamentoEmpresas",
                type: "time without time zone",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));
        }
    }
}
