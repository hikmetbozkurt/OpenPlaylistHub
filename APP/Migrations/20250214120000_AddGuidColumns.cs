using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APP.Migrations
{
    /// <inheritdoc />
    public partial class AddGuidColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Guid",
                table: "Users",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Guid",
                table: "Tracks",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Guid",
                table: "Roles",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Guid",
                table: "Playlists",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Guid",
                table: "Groups",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Guid",
                table: "Artists",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Guid",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "Tracks");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "Playlists");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "Artists");
        }
    }
}
