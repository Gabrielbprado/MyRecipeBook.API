using FluentMigrator.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using MyRecipeBook.API.Attributes;
using MyRecipeBook.Application.UseCases.Recipe.DashBoard;
using MyRecipeBook.Communication.Responses.Recipe;

namespace MyRecipeBook.API.Controllers;

public class DashBoardController : MyRecipeBookControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(ResponseRecipesJson),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorMessages),StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorMessages),StatusCodes.Status401Unauthorized)]
    [AuthenticatedUser]
    public async Task<IActionResult> GetToDashBoard([FromServices] IGetDashBoardUseCase useCase)
    {
        var result = await useCase.Execute();
        if (result.Recipes.Any())
         return Ok(result);

        return NotFound();
    }
}