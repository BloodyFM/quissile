namespace quissile.wwwapi8.ClientModels
{
    public class QuestionPut
    {
        public string Text { get; set; }
        public int? QuizId { get; set; }
        public List<AlternativeNullableId>? Alternatives { get; set; }
    }
}
