using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DaisyStudy.Migrations
{
    /// <inheritdoc />
    public partial class UpdateQuestion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentExamDetails");

            migrationBuilder.DropTable(
                name: "Answers");

            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Questions");

            migrationBuilder.AddColumn<string>(
                name: "Option1",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Option2",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Option3",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Option4",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OptionCorrect",
                table: "Questions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Option1",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "Option2",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "Option3",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "Option4",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "OptionCorrect",
                table: "Questions");

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Questions",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Answers",
                columns: table => new
                {
                    AnswerID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionID = table.Column<int>(type: "int", nullable: false),
                    AnswerString = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answers", x => x.AnswerID);
                    table.ForeignKey(
                        name: "FK_Answers_Questions_QuestionID",
                        column: x => x.QuestionID,
                        principalTable: "Questions",
                        principalColumn: "QuestionID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentExamDetails",
                columns: table => new
                {
                    StudentExamDetailID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnswerID = table.Column<int>(type: "int", nullable: false),
                    StudentExamID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentExamDetails", x => x.StudentExamDetailID);
                    table.ForeignKey(
                        name: "FK_StudentExamDetails_Answers_AnswerID",
                        column: x => x.AnswerID,
                        principalTable: "Answers",
                        principalColumn: "AnswerID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentExamDetails_StudentExams_StudentExamID",
                        column: x => x.StudentExamID,
                        principalTable: "StudentExams",
                        principalColumn: "StudentExamID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Answers_QuestionID",
                table: "Answers",
                column: "QuestionID");

            migrationBuilder.CreateIndex(
                name: "IX_StudentExamDetails_AnswerID",
                table: "StudentExamDetails",
                column: "AnswerID");

            migrationBuilder.CreateIndex(
                name: "IX_StudentExamDetails_StudentExamID",
                table: "StudentExamDetails",
                column: "StudentExamID");
        }
    }
}
