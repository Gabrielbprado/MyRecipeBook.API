using MyRecipeBook.Communication.Responses.Recipe;

namespace MyRecipeBook.Application.UseCases.Recipe.DashBoard;

public interface IGetDashBoardUseCase
{
    Task<ResponseRecipesJson> Execute();
}