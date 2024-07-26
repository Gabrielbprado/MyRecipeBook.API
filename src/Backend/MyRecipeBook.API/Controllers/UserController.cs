using Microsoft.AspNetCore.Mvc;
using MyRecipeBook.Application.UseCases.User.Register;
using MyRecipeBook.Communication.Requests.User;

namespace MyRecipeBook.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Get([FromBody] RequestRegisterUserJson request,[FromServices] IRegisterUserUseCase useCase)
    {
        var result = await useCase.Execute(request);
        return Created(string.Empty, result);
    }
}