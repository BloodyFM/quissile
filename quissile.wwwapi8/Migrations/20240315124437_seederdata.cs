using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace quissile.wwwapi8.Migrations
{
    /// <inheritdoc />
    public partial class seederdata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "alternatives",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    text = table.Column<string>(type: "text", nullable: false),
                    answer = table.Column<bool>(type: "boolean", nullable: false),
                    fk_question_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_alternatives", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "questions",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    text = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_questions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "questions_alternatives",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    fk_question_id = table.Column<int>(type: "integer", nullable: false),
                    fk_alternative_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_questions_alternatives", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "quizes",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_quizes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "quizes_questions",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    fk_questions_alternatives_id = table.Column<int>(type: "integer", nullable: false),
                    fk_quiz_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_quizes_questions", x => x.id);
                });

            migrationBuilder.InsertData(
                table: "alternatives",
                columns: new[] { "id", "fk_question_id", "text", "answer" },
                values: new object[,]
                {
                    { 1, 1, "Application Programming Interface", true },
                    { 2, 1, "Application Project Interface", false }
                });

            migrationBuilder.InsertData(
                table: "questions",
                columns: new[] { "id", "text" },
                values: new object[] { 1, "Hva står API for?" });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "alternatives");

            migrationBuilder.DropTable(
                name: "questions");

            migrationBuilder.DropTable(
                name: "questions_alternatives");

            migrationBuilder.DropTable(
                name: "quizes");

            migrationBuilder.DropTable(
                name: "quizes_questions");
        }
    }
}
