using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DaisyStudy.Migrations
{
    /// <inheritdoc />
    public partial class AddHomeworkImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HomeworkImages",
                columns: table => new
                {
                    ImageID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HomeworkID = table.Column<int>(type: "int", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ImageFileSize = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomeworkImages", x => x.ImageID);
                    table.ForeignKey(
                        name: "FK_HomeworkImages_Homeworks_HomeworkID",
                        column: x => x.HomeworkID,
                        principalTable: "Homeworks",
                        principalColumn: "HomeworkID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HomeworkImages_HomeworkID",
                table: "HomeworkImages",
                column: "HomeworkID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HomeworkImages");
        }
    }
}
