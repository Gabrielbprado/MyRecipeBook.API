using MyRecipeBook.Communication.Requests.User;
using MyRecipeBook.Communication.Responses.User;

namespace MyRecipeBook.Application.UseCases.User.Register;

public interface IRegisterUserUseCase
{
    Task<ResponseRegisterUserJson> Execute(RequestRegisterUserJson request);
}