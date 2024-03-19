using System.ComponentModel.DataAnnotations.Schema;

namespace quissile.wwwapi8.Models
{
    [Table("question")]
    public class Question
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("text")]
        public string Text { get; set; }
        [Column("quiz_id")]
        [ForeignKey("quiz")]
        public int QuizId { get; set; }
        public ICollection<Alternative> Alternatives { get; set; } = new List<Alternative>();
    }
}
