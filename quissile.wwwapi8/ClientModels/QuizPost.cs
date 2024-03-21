namespace quissile.wwwapi8.ClientModels
{
    public class QuizPost
    {
        public string Title { get; set; }
        public List<QuestionNullableId>? Questions { get; set; }
    }
}
