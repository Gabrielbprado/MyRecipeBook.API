using Microsoft.AspNetCore.Mvc;
using MyRecipeBook.API.Attributes;
using MyRecipeBook.Application.UseCases.User.ChangePassword;
using MyRecipeBook.Application.UseCases.User.Profile;
using MyRecipeBook.Application.UseCases.User.Register;
using MyRecipeBook.Application.UseCases.User.Update;
using MyRecipeBook.Communication.Requests.ChangePassword;
using MyRecipeBook.Communication.Requests.User;
using MyRecipeBook.Communication.Responses;
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
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorMessage), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [AuthenticatedUser]
    public async Task<IActionResult> Update([FromServices] IUpdateUserUseCase useCase,[FromBody] RequestUpdateUserJson request)
    {
        await useCase.Execute(request);
        return NoContent();
    }
    
    [HttpPut("change-password")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorMessage), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [AuthenticatedUser]
    public async Task<IActionResult> ChangePassword([FromServices] IChangePasswordUseCase useCase,[FromBody] RequestChangePasswordJson request)
    {
        await useCase.Execute(request);
        return NoContent();
    }
}