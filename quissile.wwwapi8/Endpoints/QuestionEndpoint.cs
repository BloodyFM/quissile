using Microsoft.AspNetCore.Mvc;
using quissile.wwwapi8.ClientModels;
using quissile.wwwapi8.DTO;
using quissile.wwwapi8.Models;
using quissile.wwwapi8.Repositories;

namespace quissile.wwwapi8.Endpoints
{
    public static class QuestionEndpoint
    {
        public static void ConfigureQuestionEndpoint(this WebApplication app)
        {
            var questionGroup = app.MapGroup("questions");
            questionGroup.MapPost("/", CreateQuestion);
            questionGroup.MapGet("/", GetQuestions);
            questionGroup.MapGet("/{id}", GetQuestionById);
            questionGroup.MapPut("/{id}", UpdateQuestionById);
            questionGroup.MapDelete("/{id}", DeleteQuestionById);
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> DeleteQuestionById(IRepository<Question> repository, int id)
        {
            var response = await repository.DeleteById(id);
            if (response != null) {
                return TypedResults.Ok(new Payload<QuestionDTO> { Data = new QuestionDTO(response) });
            }
            return TypedResults.NotFound(new Payload<string> { Status = "Failure", Data = "Question not found" });
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> UpdateQuestionById(IRepository<Question> repository, IRepository<Quiz> quizRepository, IRepository<Alternative> altRepository, int id, QuestionPost question)
        {
            var originalQuestion = await repository.GetById(id);
            if (originalQuestion == null)
            {
                return TypedResults.NotFound(new Payload<string> { Status = "Failure", Data = "Question not found" });
            }

            if (question.QuizId != null)
            {
                var quiz = await quizRepository.GetById((int)question.QuizId);
                if (quiz == null)
                {
                    return TypedResults.NotFound(new Payload<string> { Status = "Failure", Data = "Quiz not found" });
                }
            }

            originalQuestion.Text = (question.Text != "string" && question.Text.Length > 0) ? question.Text : originalQuestion.Text;
            originalQuestion.QuizId = question.QuizId;

            
            if (question.Alternatives != null)
            {
                List<Alternative> alternatives = new List<Alternative>();
                foreach (var alternative in question.Alternatives)
                {
                    if (alternative.Id != null)
                    {
                        var alt = await altRepository.GetById((int)alternative.Id);
                        if (alt == null)
                        {
                            return TypedResults.NotFound(new Payload<string> { Status = "Failure", Data = "Alternative not found" });
                        }
                        alt.Text = (alternative.Text != "string" && alternative.Text.Length > 0) ? alternative.Text : alt.Text;
                        alt.IsAnswer = alternative.IsAnswer;
                        alt.QuestionId = originalQuestion.Id;
                        alternatives.Add(alt);
                    }
                    else
                    {
                        var alt = new Alternative
                        {
                            Text = alternative.Text,
                            IsAnswer = alternative.IsAnswer,
                            QuestionId = originalQuestion.Id
                        };
                        alternatives.Add(alt);
                    }
                }
                originalQuestion.Alternatives = alternatives;
            }

            var response = await repository.Update(originalQuestion);
            if (response != null)
            {
                return TypedResults.Created("", new Payload<QuestionDTO> { Data = new QuestionDTO(response) });
            }
            else return TypedResults.BadRequest(new Payload<string> { Status = "Failure", Data = "Invalid input" });
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> GetQuestionById(IRepository<Question> repository, int id)
        {
            var response = await repository.GetById(id);
            if (response != null) {
                return TypedResults.Ok(new Payload<QuestionDTO> { Data = new QuestionDTO(response) });
            }
            return TypedResults.NotFound(new Payload<string> { Status = "Failure", Data = "Question not found" });
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> GetQuestions(IRepository<Question> repository)
        {
            var response = await repository.GetAll();
            List<QuestionDTO> result = new List<QuestionDTO>();
            if (response != null) {
                foreach (var item in response)
                {
                    result.Add(new QuestionDTO(item));
                }
                return TypedResults.Ok(new Payload<List<QuestionDTO>> { Data = result });
            }
            return TypedResults.NotFound(new Payload<string> { Status = "Failure", Data = "Failed to get all questions" });
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public static async Task<IResult> CreateQuestion(IRepository<Question> repository, IRepository<Quiz> quizRepository, QuestionPost questionPost)
        {
            if (questionPost.QuizId != null)
            {
                var quiz = await quizRepository.GetById((int)questionPost.QuizId);
                if (quiz == null)
                {
                    return TypedResults.NotFound(new Payload<string> { Status = "Failure", Data = "Quiz not found" });
                }
            }

            var question = new Question
            {
                Text = questionPost.Text,
                QuizId = questionPost.QuizId
            };

            List<Alternative> alternatives = new List<Alternative>();
            if (questionPost.Alternatives != null)
            {
                foreach (var alternative in questionPost.Alternatives)
                {
                    var alt = new Alternative
                    {
                        Text = alternative.Text,
                        IsAnswer = alternative.IsAnswer,
                        QuestionId = question.Id
                    };
                    alternatives.Add(alt);
                }
            }
            question.Alternatives = alternatives;

            var response = await repository.Insert(question);
            if (response != null)
            {
                return TypedResults.Created("", new Payload<QuestionDTO> { Data = new QuestionDTO(response) });
            }
            return TypedResults.BadRequest(new Payload<string> { Status = "Failure", Data = "Invalid input" });
        }
    }
}
