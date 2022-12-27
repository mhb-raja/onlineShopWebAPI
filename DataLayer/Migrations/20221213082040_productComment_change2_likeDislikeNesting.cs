using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLayer.Migrations
{
    public partial class productComment_change2_likeDislikeNesting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Seen",
                table: "ProductComments",
                newName: "SeenByAdmin");

            migrationBuilder.AddColumn<int>(
                name: "DislikeCount",
                table: "ProductComments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LikeCount",
                table: "ProductComments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "ParentId",
                table: "ProductComments",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductComments_ParentId",
                table: "ProductComments",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductComments_ProductComments_ParentId",
                table: "ProductComments",
                column: "ParentId",
                principalTable: "ProductComments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductComments_ProductComments_ParentId",
                table: "ProductComments");

            migrationBuilder.DropIndex(
                name: "IX_ProductComments_ParentId",
                table: "ProductComments");

            migrationBuilder.DropColumn(
                name: "DislikeCount",
                table: "ProductComments");

            migrationBuilder.DropColumn(
                name: "LikeCount",
                table: "ProductComments");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "ProductComments");

            migrationBuilder.RenameColumn(
                name: "SeenByAdmin",
                table: "ProductComments",
                newName: "Seen");
        }
    }
}
