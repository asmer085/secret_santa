using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace secret_santa.Migrations
{
    /// <inheritdoc />
    public partial class EditWishlistTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ItemDescription",
                table: "Wishlists");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ItemDescription",
                table: "Wishlists",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
