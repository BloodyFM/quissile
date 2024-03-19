using System.ComponentModel.DataAnnotations.Schema;

namespace quissile.wwwapi8.Models
{
    [Table("alternative")]
    public class Alternative
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("text")]
        public string Text { get; set; }
        [Column("is_answer")]
        public bool IsAnswer { get; set; } = false;
        [Column("question_id")]
        [ForeignKey("question")]
        public int QuestionId { get; set; }
    }
}
