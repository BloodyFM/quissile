using quissile.wwwapi8.Models;

namespace quissile.wwwapi8.DTO
{
    public class QuizDTO
    {
        public QuizDTO(Quiz quiz)
        {
            Id = quiz.Id;
            Title = quiz.Title;
            Questions = quiz.Questions?.Select(q => new QuestionDTO(q)).ToList();
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public List<QuestionDTO>? Questions { get; set; }
    }
}
