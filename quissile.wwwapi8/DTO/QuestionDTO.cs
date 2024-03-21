using quissile.wwwapi8.Models;

namespace quissile.wwwapi8.DTO
{
    public class QuestionDTO
    {
        public QuestionDTO(Question question)
        {
            Id = question.Id;   
            Text = question.Text;
            QuizId = question.QuizId;
            if (question.Alternatives != null)
            {
                foreach (var alternative in question.Alternatives)
                {
                    Alternatives.Add(new AlternativeDTO(alternative));
                }
            }
        }
        public int Id { get; set; }
        public string Text { get; set; }
        public int? QuizId { get; set; }
        public ICollection<AlternativeDTO>? Alternatives { get; set; } = new List<AlternativeDTO>();
    }
}
