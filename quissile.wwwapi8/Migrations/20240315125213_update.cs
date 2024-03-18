using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace quissile.wwwapi8.Migrations
{
    /// <inheritdoc />
    public partial class update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "questions_alternatives",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "quizes",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "quizes_questions",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.AlterColumn<int>(
                name: "fk_question_id",
                table: "alternatives",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateIndex(
                name: "IX_alternatives_fk_question_id",
                table: "alternatives",
                column: "fk_question_id");

            migrationBuilder.AddForeignKey(
                name: "FK_alternatives_questions_fk_question_id",
                table: "alternatives",
                column: "fk_question_id",
                principalTable: "questions",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_alternatives_questions_fk_question_id",
                table: "alternatives");

            migrationBuilder.DropIndex(
                name: "IX_alternatives_fk_question_id",
                table: "alternatives");

            migrationBuilder.AlterColumn<int>(
                name: "fk_question_id",
                table: "alternatives",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "questions_alternatives",
                columns: new[] { "id", "fk_alternative_id", "fk_question_id" },
                values: new object[] { 1, 0, 0 });

            migrationBuilder.InsertData(
                table: "quizes",
                columns: new[] { "id", "title" },
                values: new object[] { 1, "Progge - quiz" });

            migrationBuilder.InsertData(
                table: "quizes_questions",
                columns: new[] { "id", "fk_questions_alternatives_id", "fk_quiz_id" },
                values: new object[] { 1, 1, 1 });
        }
    }
}
