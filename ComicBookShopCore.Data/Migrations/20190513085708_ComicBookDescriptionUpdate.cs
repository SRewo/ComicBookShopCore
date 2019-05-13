using Microsoft.EntityFrameworkCore.Migrations;

namespace ComicBookShopCore.Data.Migrations
{
    public partial class ComicBookDescriptionUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "ComicBooks",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShortDescription",
                table: "ComicBooks",
                maxLength: 60,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "ComicBooks");

            migrationBuilder.DropColumn(
                name: "ShortDescription",
                table: "ComicBooks");
        }
    }
}
