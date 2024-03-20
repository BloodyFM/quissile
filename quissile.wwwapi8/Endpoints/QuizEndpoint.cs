using Microsoft.AspNetCore.Mvc;
using quissile.wwwapi8.ClientModels;
using quissile.wwwapi8.DTO;
using quissile.wwwapi8.Models;
using quissile.wwwapi8.Repositories;

namespace quissile.wwwapi8.Endpoints
{
    public static class QuizEndpoint
    {
        public static void ConfigureQuizEndpoint(this WebApplication app)
        {
            var quizGroup = app.MapGroup("quiz");
            quizGroup.MapPost("/", CreateQuiz);
            quizGroup.MapGet("/", GetQuizes);
            quizGroup.MapGet("/{id}", GetQuizById);
            quizGroup.MapPut("/{id}", UpdateQuizById);
            quizGroup.MapDelete("/{id}", DeleteQuizById);
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public static async Task<IResult> CreateQuiz(IRepository<Quiz> repository, IRepository<Question> questionRepository, QuizPost quizPost)
        {
            var quiz = new Quiz
            {
                Title = quizPost.Title
            };

            // Include questions
            List<Question> questions = new List<Question>();
            if (quizPost.Questions != null)
            {
                foreach (var question in quizPost.Questions)
                {
                    Question currentQuestion;
                    if (question.QuizId != null)
                    {
                        var existingQuestion = await questionRepository.GetById((int)question.QuizId);
                        if (existingQuestion == null)
                        {
                            return TypedResults.BadRequest(new Payload<string> { Status = "Failure", Data = "Invalid input" });
                        }
                        currentQuestion = existingQuestion;
                    }
                    else
                    {
                        var newQuestion = new Question
                        {
                            Text = question.Text,
                            QuizId = quiz.Id
                        };
                        currentQuestion = newQuestion;
                    }
                    // Include alternatives
                    List<Alternative> alternatives = new List<Alternative>();
                    if (question.Alternatives != null)
                    {
                        foreach (var alternative in question.Alternatives)
                        {
                            var alt = new Alternative
                            {
                                Text = alternative.Text,
                                IsAnswer = alternative.IsAnswer,
                                QuestionId = currentQuestion.Id
                            };
                            alternatives.Add(alt);
                        }
                    }
                    currentQuestion.Alternatives = alternatives;
                    questions.Add(currentQuestion);
                }
            }
            quiz.Questions = questions;

            var response = await repository.Insert(quiz);
            if (response != null)
            {
                return TypedResults.Created("", new Payload<QuizDTO> { Data = new QuizDTO(quiz) });
            }
            return TypedResults.BadRequest(new Payload<string> { Status = "Failure", Data = "Invalid input" });
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        public static async Task<IResult> GetQuizes(IRepository<Quiz> repository)
        {
            var response = await repository.GetAll();
            return TypedResults.Ok(new Payload<IEnumerable<QuizDTO>> { Data = response.Select(q => new QuizDTO(q)) });
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> GetQuizById(IRepository<Quiz> repository, int id)
        {
            var response = await repository.GetById(id);
            if (response != null)
            {
                return TypedResults.Ok(new Payload<QuizDTO> { Data = new QuizDTO(response) });
            }
            return TypedResults.NotFound(new Payload<string> { Status = "Failure", Data = "Quiz not found" });
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> DeleteQuizById(IRepository<Quiz> repository, int id)
        {
            var response = await repository.DeleteById(id);
            if (response != null)
            {
                return TypedResults.Ok(new Payload<QuizDTO> { Data = new QuizDTO(response) });
            }
            return TypedResults.NotFound(new Payload<string> { Status = "Failure", Data = "Quiz not found" });
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> UpdateQuizById(IRepository<Quiz> repository, int id, QuizPost quizPost)
        {
            var originalQuiz = await repository.GetById(id);
            if (originalQuiz == null)
            {
                return TypedResults.NotFound(new Payload<string> { Status = "Failure", Data = "Quiz not found" });
            }
            originalQuiz.Title = (quizPost.Title != "string" && quizPost.Title != null) ? quizPost.Title : originalQuiz.Title;

            var response = await repository.Update(originalQuiz);
            if (response != null)
            {
                return TypedResults.Created("", new Payload<QuizDTO> { Data = new QuizDTO(response) });
            }
            else return TypedResults.BadRequest(new Payload<string> { Status = "Failure", Data = "Invalid input" });
        }
    }
}
