using System.ComponentModel.DataAnnotations.Schema;

namespace quissile.wwwapi8.Models
{
    [Table("quizes_questions")]
    public class QuizQuestion
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("fk_questions_alternatives_id")]
        [ForeignKey("questions_alternatives")]
        public int QuestionAlternativeId { get; set; }
        [Column("fk_quiz_id")]
        [ForeignKey("quizes")]
        public int QuizId { get; set; }
    }
}
