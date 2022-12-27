using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLayer.Migrations
{
    public partial class productComment_change : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsAccepted",
                table: "ProductComments",
                newName: "Approved");

            migrationBuilder.AddColumn<bool>(
                name: "Seen",
                table: "ProductComments",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Seen",
                table: "ProductComments");

            migrationBuilder.RenameColumn(
                name: "Approved",
                table: "ProductComments",
                newName: "IsAccepted");
        }
    }
}
