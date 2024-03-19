using Microsoft.AspNetCore.Mvc;
using quissile.wwwapi8.ClientModels;
using quissile.wwwapi8.DTO;
using quissile.wwwapi8.Models;
using quissile.wwwapi8.Repositories;

namespace quissile.wwwapi8.Endpoints
{
    public static class AlternativeEndpoint
    {
        public static void ConfigureAlternativeEndpoint(this WebApplication app)
        {
            var alternativeGroup = app.MapGroup("alternative");
            alternativeGroup.MapPost("/", CreateAlternative);
            alternativeGroup.MapGet("/", GetAlternatives);
            alternativeGroup.MapPut("/{id}", UpdateAlternativeById);
            alternativeGroup.MapDelete("/{id}", DeleteAlternativeById);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> DeleteAlternativeById(IRepository<Alternative> repository, int id)
        {
            var response = await repository.DeleteById(id);
            if (response != null)
            {
                return TypedResults.Ok(new Payload<AlternativeDTO> { Data = new AlternativeDTO(response) });
            }
            return TypedResults.NotFound(new Payload<string> { Status = "Failure", Data = "Alternative not found" });
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> UpdateAlternativeById(IRepository<Alternative> repository, int id, AlternativePost alternative)
        {
            var originalAlternative = await repository.GetById(id);
            if (originalAlternative == null)
            {
                return TypedResults.NotFound(new Payload<string> { Status = "Failure", Data = "Alternative not found" });
            }
            originalAlternative.Text = (alternative.Text != "string" && alternative.Text.Length > 1) ? alternative.Text : originalAlternative.Text;
            originalAlternative.IsAnswer = alternative.IsAnswer;
            originalAlternative.QuestionId = alternative.QuestionId;

            var response = await repository.Update(originalAlternative);
            if (response != null)
            {
                return TypedResults.Created("", new Payload<AlternativeDTO> { Data = new AlternativeDTO(response) });
            }
            else return TypedResults.BadRequest(new Payload<string> { Status = "Failure", Data = "Invalid input" });
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
            return TypedResults.NotFound(new Payload<string> { Status = "Failure", Data = "Failed to get all alternatives" });
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public static async Task<IResult> CreateAlternative(IRepository<Alternative> repository, AlternativePost alternativePost)
        {
            var alternative = new Alternative
            {
                Text = alternativePost.Text,
                IsAnswer = alternativePost.IsAnswer,
                QuestionId = alternativePost.QuestionId
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
