using Microsoft.AspNetCore.Mvc;
using MyRecipeBook.API.Attributes;
using MyRecipeBook.Application.UseCases.User.Profile;
using MyRecipeBook.Application.UseCases.User.Register;
using MyRecipeBook.Communication.Requests.User;
using MyRecipeBook.Communication.Responses.User;

namespace MyRecipeBook.API.Controllers;

public class UserController : MyRecipeBookControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisterUserJson), StatusCodes.Status201Created)]
    public async Task<IActionResult> Register([FromBody] RequestRegisterUserJson request,[FromServices] IRegisterUserUseCase useCase)
    {
        var result = await useCase.Execute(request);
        return Created(string.Empty, result);
    } 
    [HttpGet]
    [ProducesResponseType(typeof(ResponseUserProfileJson), StatusCodes.Status200OK)]
    [AuthenticatedUser]
    public async Task<IActionResult> GetUserProfile([FromServices] IGetProfileUserUseCase useCase)
    {
        var result = await useCase.Execute();
        return Ok(result);
    }
}