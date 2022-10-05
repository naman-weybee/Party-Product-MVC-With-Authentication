using Microsoft.EntityFrameworkCore.Migrations;

namespace PartyProduct_Exercise_03.Migrations
{
    public partial class UpdatedcolumninInvoice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PartyName",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "ProductName",
                table: "Invoice");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PartyName",
                table: "Invoice",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductName",
                table: "Invoice",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
