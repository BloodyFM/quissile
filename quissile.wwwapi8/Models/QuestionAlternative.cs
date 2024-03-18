using System.ComponentModel.DataAnnotations.Schema;

namespace quissile.wwwapi8.Models
{
    [Table("questions_alternatives")]
    public class QuestionAlternative
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("fk_question_id")]
        [ForeignKey("questions")]
        public int QuestionId { get; set; }
        [Column("fk_alternative_id")]
        [ForeignKey("alternatives")]
        public int AlternativeId { get; set; }

    }
}
