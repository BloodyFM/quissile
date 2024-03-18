using Microsoft.AspNetCore.Mvc;
using quissile.wwwapi8.ClientModels;
using quissile.wwwapi8.DTO;
using quissile.wwwapi8.Models;
using quissile.wwwapi8.Repositories;

namespace quissile.wwwapi8.Endpoints
{
    public static class AlternativeEndpoint
    {

        // I did a copy paste from question endpoint xd
        public static void ConfigureAlternativeEndpoint(this WebApplication app)
        {
            var questionGroup = app.MapGroup("alternative");
            questionGroup.MapPost("/", CreateAlternative);
            questionGroup.MapGet("/", GetAlternatives);
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> DeleteQuestionById(IRepository<Question> repository, int id)
        {
            var resposne = await repository.DeleteById(id);
            if (resposne != null)
            {
                return TypedResults.Ok(new Payload<QuestionDTO> { Data = new QuestionDTO(resposne) });
            }
            return TypedResults.NotFound(new Payload<string> { Status = "Failure", Data = "Question not found" });
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> UpdateQuestionById(IRepository<Question> repository, int id, QuestionPost question)
        {
            var originalQuestion = await repository.GetById(id);
            if (originalQuestion == null)
            {
                return TypedResults.NotFound(new Payload<string> { Status = "Failure", Data = "Question not found" });
            }
            originalQuestion.Text = (question.Text != "string" && question.Text != null) ? question.Text : originalQuestion.Text;

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
            if (response != null)
            {
                return TypedResults.Ok(new Payload<QuestionDTO> { Data = new QuestionDTO(response) });
            }
            return TypedResults.NotFound(new Payload<string> { Status = "Failure", Data = "Question not found" });
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> GetAlternatives(IRepository<Alternative> repository)
        {
            var response = await repository.GetAll();
            List<AlternativeDTO> result = new List<AlternativeDTO>();
            if (response != null)
            {
                foreach (var item in response)
                {
                    result.Add(new AlternativeDTO(item));
                }
                return TypedResults.Ok(response);
            }
            return TypedResults.NotFound(new Payload<string> { Status = "Failure", Data = "Failed to get all questions" });
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public static async Task<IResult> CreateAlternative(IRepository<Alternative> repository, AlternativePost alternativePost)
        {
            var alternative = new Alternative
            {
                Text = alternativePost.Text
            };

            var response = await repository.Insert(alternative);
            if (response != null)
            {
                return TypedResults.Created("", new Payload<AlternativeDTO> { Data = new AlternativeDTO(response) });
            }
            return TypedResults.BadRequest(new Payload<string> { Status = "Failure", Data = "Invalid input" });
        }
    }
}
