using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookyPets.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ProgressSwap : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookIds",
                table: "Readers");

            migrationBuilder.AddColumn<string>(
                name: "ProgressIds",
                table: "Readers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProgressIds",
                table: "Readers");

            migrationBuilder.AddColumn<string>(
                name: "BookIds",
                table: "Readers",
                type: "TEXT",
                nullable: true);
        }
    }
}
