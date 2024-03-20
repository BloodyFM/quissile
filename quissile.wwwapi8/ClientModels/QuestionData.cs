namespace quissile.wwwapi8.ClientModels
{
    public class QuestionData
    {
        public string Text { get; set; }
        public int? QuizId { get; set; }
        public List<AlternativeData>? Alternatives { get; set; }
    }
}
