using MyRecipeBook.Communication.Requests.Recipe;
using MyRecipeBook.Communication.Responses.Recipe;

namespace MyRecipeBook.Application.UseCases.Recipe.Recipe;

public interface IUpdateRecipeUseCase
{
    Task<ResponseRecipeJson> Execute(long recipeId,RequestRecipeJson request);
}