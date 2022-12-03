using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DaisyStudy.Migrations
{
    /// <inheritdoc />
    public partial class Update1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Submissions_AppUsers_StudentID",
                table: "Submissions");

            migrationBuilder.AddForeignKey(
                name: "FK_Submissions_AppUsers_StudentID",
                table: "Submissions",
                column: "StudentID",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Submissions_AppUsers_StudentID",
                table: "Submissions");

            migrationBuilder.AddForeignKey(
                name: "FK_Submissions_AppUsers_StudentID",
                table: "Submissions",
                column: "StudentID",
                principalTable: "AppUsers",
                principalColumn: "Id");
        }
    }
}
