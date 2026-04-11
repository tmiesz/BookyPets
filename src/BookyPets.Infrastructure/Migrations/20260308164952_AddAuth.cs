using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookyPets.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAuth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Readers",
                newName: "PasswordHash");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Readers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Readers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Readers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Readers");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Readers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Readers");

            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "Readers",
                newName: "Name");
        }
    }
}
