using AutoMapper;
using Microsoft.Extensions.Logging;
using MyRecipeBook.Communication.Responses.Recipe;
using MyRecipeBook.Domain.Repositories.Recipe;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.BaseException;

namespace MyRecipeBook.Application.UseCases.Recipe.GetById;

public class RecipeGetByIdUseCase(IRecipeReadOnlyRepository readOnlyRepository,ILoggedUser loggedUser,IMapper mapper) : IRecipeGetByIdUseCase 
{
    private readonly IRecipeReadOnlyRepository _readOnlyRepository = readOnlyRepository;
    private readonly ILoggedUser _loggedUser = loggedUser;
    private readonly IMapper _mapper = mapper;
    public async Task<ResponseRecipeJson> ExecuteAsync(long recipeId)
    {
        var loggedUser = await _loggedUser.User();
        var recipe = await _readOnlyRepository.GetById(loggedUser!, recipeId);
        if (recipe is null)
            throw new NotFoundException(ResourceLanguage.RECIPE_NOT_FOUND);
        return _mapper.Map<ResponseRecipeJson>(recipe);
    }
}