using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BarberFlow.API.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarSoftDeleteModelEmpresa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Slug",
                table: "Empresas",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(150)",
                oldMaxLength: 150);

            migrationBuilder.AddColumn<bool>(
                name: "Ativo",
                table: "Empresas",
                type: "boolean",
                maxLength: 150,
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Empresas",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Empresas_CNPJ",
                table: "Empresas",
                column: "CNPJ",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Empresas_CNPJ",
                table: "Empresas");

            migrationBuilder.DropColumn(
                name: "Ativo",
                table: "Empresas");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Empresas");

            migrationBuilder.AlterColumn<string>(
                name: "Slug",
                table: "Empresas",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
