using Microsoft.EntityFrameworkCore.Migrations;

namespace PartyProduct_Exercise_03.Migrations
{
    public partial class addedcolumninInvoice1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PartyName",
                table: "Invoice",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductName",
                table: "Invoice",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PartyName",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "ProductName",
                table: "Invoice");
        }
    }
}
