using FluentMigrator.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using MyRecipeBook.API.Attributes;
using MyRecipeBook.API.Binders;
using MyRecipeBook.Application.UseCases.Recipe;
using MyRecipeBook.Application.UseCases.Recipe.Delete;
using MyRecipeBook.Application.UseCases.Recipe.Filter;
using MyRecipeBook.Application.UseCases.Recipe.GetById;
using MyRecipeBook.Application.UseCases.Recipe.Recipe;
using MyRecipeBook.Communication.Requests.Recipe;
using MyRecipeBook.Communication.Responses.Recipe;
using MyRecipeBook.Exceptions.BaseException;

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
    
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorMessages),StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorMessages),StatusCodes.Status401Unauthorized)]
    [AuthenticatedUser]
    public async Task<IActionResult> Delete([FromServices] IRecipeDeleteUseCase useCase,[FromRoute] [ModelBinder(typeof(MyRecipeBookIdBinder))] long id)
    {
        await useCase.Execute(id);
            return NoContent();
    }
    
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorMessages),StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorMessages),StatusCodes.Status401Unauthorized)]
    [AuthenticatedUser]
    public async Task<IActionResult> Update([FromServices] IUpdateRecipeUseCase useCase,[FromRoute] [ModelBinder(typeof(MyRecipeBookIdBinder))] long id,[FromBody] RequestRecipeJson request)
    {
        await useCase.Execute(id,request);
            return NoContent();
    }
}