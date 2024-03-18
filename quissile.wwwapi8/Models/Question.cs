using System.ComponentModel.DataAnnotations.Schema;

namespace quissile.wwwapi8.Models
{
    [Table("questions")]
    public class Question
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("text")]
        public string Text { get; set; }
        public ICollection<Alternative> Alternatives { get; } = new List<Alternative>();
    }
}
