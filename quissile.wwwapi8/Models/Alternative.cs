using System.ComponentModel.DataAnnotations.Schema;

namespace quissile.wwwapi8.Models
{
    [Table("alternatives")]
    public class Alternative
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("text")]
        public string Text { get; set; }
        [Column("answer")]
        public bool Answer { get; set; }
        [Column("fk_question_id")]
        [ForeignKey("questions")]
        public int? QuestionId { get; set; }
        public Question? Question { get; set; }
    }
}
