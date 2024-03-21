using System.ComponentModel.DataAnnotations.Schema;

namespace quissile.wwwapi8.Models
{
    [Table("quiz")]
    public class Quiz
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("title")]
        public string Title { get; set; }
        public ICollection<Question> Questions { get; set; } = new List<Question>();
    }
}
