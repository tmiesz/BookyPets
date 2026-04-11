using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookyPets.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class GenericUpgrade2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuestIds",
                table: "Readers");

            migrationBuilder.AddColumn<string>(
                name: "BookIds",
                table: "Readers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookIds",
                table: "Readers");

            migrationBuilder.AddColumn<string>(
                name: "QuestIds",
                table: "Readers",
                type: "TEXT",
                nullable: true);
        }
    }
}
