using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookyPets.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UniqueReaderEmail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Readers_Email",
                table: "Readers",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Readers_Email",
                table: "Readers");
        }
    }
}
