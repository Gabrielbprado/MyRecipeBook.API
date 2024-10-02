using MyRecipeBook.Communication.Requests.Recipe;
using MyRecipeBook.Communication.Responses.Recipe;

namespace MyRecipeBook.Application.UseCases.Recipe.Filter;

public interface IFilterRecipeUseCase
{
    Task<ResponseRecipesJson> Execute(RequestFilterRecipeJson request);
}