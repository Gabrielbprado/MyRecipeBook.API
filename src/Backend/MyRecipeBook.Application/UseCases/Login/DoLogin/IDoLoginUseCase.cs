using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses.User;

namespace MyRecipeBook.Application.UseCases.Login;

public interface IDoLoginUseCase
{
    Task<ResponseRegisterUserJson> Execute(RequestLoginUseCase request);
}