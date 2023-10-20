using Microsoft.EntityFrameworkCore.Migrations;

namespace TP_PWEB2.Data.Migrations
{
    public partial class _1mig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Empresa_ID",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "pNome",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "uNome",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Empresa_ID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "pNome",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "uNome",
                table: "AspNetUsers");
        }
    }
}
