using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses.User;

namespace MyRecipeBook.Application.UseCases.Login.DoLogin;

public interface IDoLoginUseCase
{
    Task<ResponseRegisterUserJson> Execute(RequestLoginJson request);
}