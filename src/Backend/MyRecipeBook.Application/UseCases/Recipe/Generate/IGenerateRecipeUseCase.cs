using MyRecipeBook.Communication.Requests.Recipe;
using MyRecipeBook.Communication.Responses.Recipe;

namespace MyRecipeBook.Application.UseCases.Recipe.Generate;

public interface IGenerateRecipeUseCase
{
    Task<ResponseGenerateRecipeJson> Execute(RequestGenerateRecipeJson request);
}