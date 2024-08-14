using Microsoft.AspNetCore.Mvc;
using MyRecipeBook.Application.UseCases.Login;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses.User;

namespace MyRecipeBook.API.Controllers;
public class LoginController : MyRecipeBookControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisterUserJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseRegisterUserJson), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromServices] IDoLoginUseCase useCase, [FromBody] RequestLoginUseCase request)
    {
       var response = await useCase.Execute(request);
        return Ok(response);
    }
}