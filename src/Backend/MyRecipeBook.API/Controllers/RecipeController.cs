using FluentMigrator.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using MyRecipeBook.API.Attributes;
using MyRecipeBook.API.Binders;
using MyRecipeBook.Application.UseCases.Recipe;
using MyRecipeBook.Application.UseCases.Recipe.Filter;
using MyRecipeBook.Application.UseCases.Recipe.GetById;
using MyRecipeBook.Communication.Requests.Recipe;
using MyRecipeBook.Communication.Responses.Recipe;

namespace MyRecipeBook.API.Controllers;

public class RecipeController : MyRecipeBookControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisteredRecipeJson),StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorMessages),StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorMessages),StatusCodes.Status401Unauthorized)]
    [AuthenticatedUser]
    public async Task<IActionResult> Register([FromServices] IRegisterRecipeUseCase useCase,[FromBody] RequestRecipeJson request)
    {
        var result = await useCase.Execute(request);
        return Created(string.Empty, result);
    }

    [HttpPost("filter")]
    [ProducesResponseType(typeof(ResponseRecipesJson),StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorMessages),StatusCodes.Status401Unauthorized)]
    [AuthenticatedUser]
    public async Task<IActionResult> Filter([FromServices] IFilterRecipeUseCase useCase,[FromBody] RequestFilterRecipeJson request)
    {
        var result = await useCase.Execute(request);
        if (result.Recipes.Any())
            return Ok(result);
        return NoContent();
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ResponseRegisteredRecipeJson),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorMessages),StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorMessages),StatusCodes.Status401Unauthorized)]
    [AuthenticatedUser]
    public async Task<IActionResult> GetById([FromServices] IRecipeGetByIdUseCase useCase,[FromRoute] [ModelBinder(typeof(MyRecipeBookIdBinder))] long id)
    {
        var result = await useCase.ExecuteAsync(id);
            return Ok(result);
    }
}