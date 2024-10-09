using MyRecipeBook.Communication.Responses.Recipe;

namespace MyRecipeBook.Application.UseCases.Recipe.GetById;

public interface IRecipeGetByIdUseCase
{
    Task<ResponseRecipeJson> ExecuteAsync(long recipeId);
}