using System.ComponentModel.DataAnnotations.Schema;

namespace quissile.wwwapi8.Models
{
    [Table("quizes")]
    public class Quiz
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("title")]
        public string Title { get; set; }
    }
}
