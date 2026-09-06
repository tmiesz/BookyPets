using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookyPets.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSpeciesToPet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Species",
                table: "Pets",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Species",
                table: "Pets");
        }
    }
}
