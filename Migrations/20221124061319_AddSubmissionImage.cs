using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DaisyStudy.Migrations
{
    /// <inheritdoc />
    public partial class AddSubmissionImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Submissions_AppUsers_StudentID",
                table: "Submissions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Submissions",
                table: "Submissions");

            migrationBuilder.AlterColumn<string>(
                name: "StudentID",
                table: "Submissions",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "SubmissionID",
                table: "Submissions",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Submissions",
                table: "Submissions",
                column: "SubmissionID");

            migrationBuilder.CreateTable(
                name: "SubmissionImages",
                columns: table => new
                {
                    ImageID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubmissionID = table.Column<int>(type: "int", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ImageFileSize = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubmissionImages", x => x.ImageID);
                    table.ForeignKey(
                        name: "FK_SubmissionImages_Submissions_SubmissionID",
                        column: x => x.SubmissionID,
                        principalTable: "Submissions",
                        principalColumn: "SubmissionID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Submissions_HomeworkID",
                table: "Submissions",
                column: "HomeworkID");

            migrationBuilder.CreateIndex(
                name: "IX_SubmissionImages_SubmissionID",
                table: "SubmissionImages",
                column: "SubmissionID");

            migrationBuilder.AddForeignKey(
                name: "FK_Submissions_AppUsers_StudentID",
                table: "Submissions",
                column: "StudentID",
                principalTable: "AppUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Submissions_AppUsers_StudentID",
                table: "Submissions");

            migrationBuilder.DropTable(
                name: "SubmissionImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Submissions",
                table: "Submissions");

            migrationBuilder.DropIndex(
                name: "IX_Submissions_HomeworkID",
                table: "Submissions");

            migrationBuilder.DropColumn(
                name: "SubmissionID",
                table: "Submissions");

            migrationBuilder.AlterColumn<string>(
                name: "StudentID",
                table: "Submissions",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Submissions",
                table: "Submissions",
                columns: new[] { "HomeworkID", "StudentID" });

            migrationBuilder.AddForeignKey(
                name: "FK_Submissions_AppUsers_StudentID",
                table: "Submissions",
                column: "StudentID",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
