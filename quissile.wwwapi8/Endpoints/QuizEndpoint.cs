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
        public static async Task<IResult> CreateQuiz(IRepository<Quiz> repository, IRepository<Question> questionRepository, IRepository<Alternative> altRepository, QuizPost quizPost)
        {
            var quiz = new Quiz
            {
                Title = quizPost.Title
            };

            if (quizPost.Questions != null)
            {
                List<Question> questions = new List<Question>();
                foreach (var q in quizPost.Questions)
                {
                    Question currentQuestion;
                    if (q.Id != null)
                    {
                        currentQuestion = await questionRepository.GetById((int)q.Id);
                        if (currentQuestion == null)
                        {
                            return TypedResults.BadRequest(new Payload<string> { Status = "Failure", Data = "Invalid input" });
                        }
                        currentQuestion.Text = q.Text;
                        currentQuestion.QuizId = quiz.Id;
                    }
                    else
                    {
                        currentQuestion = new Question
                        {
                            Text = q.Text,
                            QuizId = quiz.Id
                        };
                    }

                    // Include alternatives
                    List<Alternative> alternatives = new List<Alternative>();
                    if (q.Alternatives != null)
                    {
                        foreach (var a in q.Alternatives)
                        {
                            Alternative currentAlternative;
                            if (a.Id != null)
                            {
                                currentAlternative = await altRepository.GetById((int)a.Id);
                                if (currentAlternative == null)
                                {
                                    return TypedResults.NotFound(new Payload<string> { Status = "Failure", Data = "Alternative not found" });
                                }
                                currentAlternative.Text = a.Text;
                                currentAlternative.IsAnswer = a.IsAnswer;
                                currentAlternative.QuestionId = currentQuestion.Id;
                            }
                            else
                            {
                                currentAlternative = new Alternative
                                {
                                    Text = a.Text,
                                    IsAnswer = a.IsAnswer,
                                    QuestionId = currentQuestion.Id
                                };
                            }
                            alternatives.Add(currentAlternative);
                        }
                    }
                    currentQuestion.Alternatives = alternatives;
                    questions.Add(currentQuestion);
                };
                quiz.Questions = questions;
            }

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
        public static async Task<IResult> DeleteQuizById(IRepository<Quiz> repository, IRepository<Question> questionRepository, int id)
        {
            var quiz = await repository.GetById(id);
            var response = await repository.DeleteById(id);
            if (response != null)
            {
                var questions = quiz.Questions;
                foreach (var q in questions)
                {
                    await questionRepository.DeleteById(q.Id);
                }
                
                return TypedResults.Ok(new Payload<QuizDTO> { Data = new QuizDTO(response) });
            }
            return TypedResults.NotFound(new Payload<string> { Status = "Failure", Data = "Quiz not found" });
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> UpdateQuizById(IRepository<Quiz> repository, IRepository<Question> questionRepository, IRepository<Alternative> altRepository, int id, QuizPost quizPut)
        {
            var originalQuiz = await repository.GetById(id);
            if (originalQuiz == null)
            {
                return TypedResults.NotFound(new Payload<string> { Status = "Failure", Data = "Quiz not found" });
            }
            originalQuiz.Title = (quizPut.Title != "string" && quizPut.Title != null) ? quizPut.Title : originalQuiz.Title;

            if (quizPut.Questions != null)
            {
                List<Question> questions = new List<Question>();
                foreach (var q in quizPut.Questions)
                {
                    Question currentQuestion;
                    if (q.Id != null)
                    {
                        currentQuestion = await questionRepository.GetById((int)q.Id);
                        if (currentQuestion == null)
                        {
                            return TypedResults.BadRequest(new Payload<string> { Status = "Failure", Data = "Invalid input" });
                        }
                        currentQuestion.Text = q.Text;
                        currentQuestion.QuizId = originalQuiz.Id;
                    }
                    else
                    {
                        currentQuestion = new Question
                        {
                            Text = q.Text,
                            QuizId = originalQuiz.Id
                        };
                    }

                    // Include alternatives
                    List<Alternative> alternatives = new List<Alternative>();
                    if (q.Alternatives != null)
                    {
                        foreach (var a in q.Alternatives)
                        {
                            Alternative currentAlternative;
                            if (a.Id != null)
                            {
                                currentAlternative = await altRepository.GetById((int)a.Id);
                                if (currentAlternative == null)
                                {
                                    return TypedResults.NotFound(new Payload<string> { Status = "Failure", Data = "Alternative not found" });
                                }
                                currentAlternative.Text = a.Text;
                                currentAlternative.IsAnswer = a.IsAnswer;
                                currentAlternative.QuestionId = currentQuestion.Id;
                            }
                            else
                            {
                                currentAlternative = new Alternative
                                {
                                    Text = a.Text,
                                    IsAnswer = a.IsAnswer,
                                    QuestionId = currentQuestion.Id
                                };
                            }
                            alternatives.Add(currentAlternative);
                        }
                    }
                    currentQuestion.Alternatives = alternatives;
                    questions.Add(currentQuestion);
                };
                originalQuiz.Questions = questions;
            }

            var response = await repository.Update(originalQuiz);
            if (response != null)
            {
                return TypedResults.Created("", new Payload<QuizDTO> { Data = new QuizDTO(response) });
            }
            else return TypedResults.BadRequest(new Payload<string> { Status = "Failure", Data = "Invalid input" });
        }
    }
}
