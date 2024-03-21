using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace quissile.wwwapi8.Migrations
{
    /// <inheritdoc />
    public partial class nullable_quiz_id : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_question_quiz_quiz_id",
                table: "question");

            migrationBuilder.AlterColumn<int>(
                name: "quiz_id",
                table: "question",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_question_quiz_quiz_id",
                table: "question",
                column: "quiz_id",
                principalTable: "quiz",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_question_quiz_quiz_id",
                table: "question");

            migrationBuilder.AlterColumn<int>(
                name: "quiz_id",
                table: "question",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_question_quiz_quiz_id",
                table: "question",
                column: "quiz_id",
                principalTable: "quiz",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
