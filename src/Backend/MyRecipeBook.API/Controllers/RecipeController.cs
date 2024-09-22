using Microsoft.AspNetCore.Mvc;
using MyRecipeBook.API.Attributes;
using MyRecipeBook.Application.UseCases.Recipe;
using MyRecipeBook.Communication.Requests.Recipe;

namespace MyRecipeBook.API.Controllers;

public class RecipeController : MyRecipeBookControllerBase
{
    [HttpPost]
    [AuthenticatedUser]
    public async Task<IActionResult> Register([FromServices] IRegisterRecipeUseCase useCase,[FromBody] RequestRecipeJson request)
    {
        var result = await useCase.Execute(request);
        return Created(string.Empty, result);
    }
}