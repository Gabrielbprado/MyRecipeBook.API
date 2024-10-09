using MyRecipeBook.Communication.Requests.Recipe;
using MyRecipeBook.Communication.Responses.Recipe;

namespace MyRecipeBook.Application.UseCases.Recipe;

public interface IRegisterRecipeUseCase
{
    Task<ResponseRegisteredRecipeJson> Execute(RequestRecipeJson request);
}