using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.Recipe;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.BaseException;

namespace MyRecipeBook.Application.UseCases.Recipe.Delete;

public class RecipeDeleteUseCase(IRecipeReadOnlyRepository readOnlyRepository,IRecipeWriteOnlyRepository recipeWriteOnlyRepository,ILoggedUser loggedUser,IUnityOfWork unityOfWork) : IRecipeDeleteUseCase
{
    private readonly IRecipeReadOnlyRepository _readOnlyRepository = readOnlyRepository;
    private readonly IRecipeWriteOnlyRepository _recipeWriteOnlyRepository = recipeWriteOnlyRepository;
    private readonly ILoggedUser _loggedUser = loggedUser;
    private readonly IUnityOfWork _unityOfWork = unityOfWork; 
    public async Task Execute(long id)
    {
        var user = await _loggedUser.User();
        var recipe = await _readOnlyRepository.GetById(user!,id);
        if (recipe is null)
            throw new NotFoundException(ResourceLanguage.RECIPE_NOT_FOUND);
        await _recipeWriteOnlyRepository.Delete(id);
        await _unityOfWork.Commit();
    }
}