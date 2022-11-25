using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DaisyStudy.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSubmission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Delay",
                table: "Submissions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Delay",
                table: "Submissions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
