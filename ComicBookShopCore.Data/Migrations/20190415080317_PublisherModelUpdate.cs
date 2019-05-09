using Microsoft.EntityFrameworkCore.Migrations;

namespace ComicBookShopCore.Data.Migrations
{
    public partial class PublisherModelUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Publishers",
                maxLength: 40,
                nullable: false,
                oldClrType: typeof(string));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Publishers",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 40);
        }
    }
}
