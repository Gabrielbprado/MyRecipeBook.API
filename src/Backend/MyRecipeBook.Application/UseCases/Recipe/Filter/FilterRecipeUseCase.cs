using AutoMapper;
using Microsoft.Extensions.Logging;
using MyRecipeBook.Communication.Requests.Recipe;
using MyRecipeBook.Communication.Responses.Recipe;
using MyRecipeBook.Domain.Dtos;
using MyRecipeBook.Domain.Repositories.Recipe;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Exceptions.BaseException;

namespace MyRecipeBook.Application.UseCases.Recipe.Filter;

public class FilterRecipeUseCase(IRecipeReadOnlyRepository repository,ILoggedUser user,IMapper mapper) : IFilterRecipeUseCase
{
    private readonly IRecipeReadOnlyRepository _repository = repository;
    private readonly ILoggedUser _loggedUser = user;
    private readonly IMapper _mapper = mapper;
    public async Task<ResponseRecipesJson> Execute(RequestFilterRecipeJson request)
    {
        await Validate(request);
        var user = await _loggedUser.User();
        var dto = _mapper.Map<FilterRecipesDto>(request);
        var recipes = await _repository.Filter(user, filter:dto);
        var shortRecipes = _mapper.Map<IList<ResponseShortRecipeJson>>(recipes);
        return new ResponseRecipesJson
        {
            Recipes = shortRecipes
        };

    }
    
    private async Task Validate(RequestFilterRecipeJson request)
    {
        var validate = new FilterRecipeValidator();
        var result = await validate.ValidateAsync(request);
        if (result.IsValid is false)
        {
            var errors = result.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ErrorOnValidatorException(errors);
        }
    }
}