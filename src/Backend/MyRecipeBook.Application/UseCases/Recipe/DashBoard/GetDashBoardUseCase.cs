using AutoMapper;
using MyRecipeBook.Communication.Responses.Recipe;
using MyRecipeBook.Domain.Repositories.Recipe;
using MyRecipeBook.Domain.Services.LoggedUser;

namespace MyRecipeBook.Application.UseCases.Recipe.DashBoard;

public class GetDashBoardUseCase(IRecipeReadOnlyRepository repository,ILoggedUser loggedUser,IMapper mapper) : IGetDashBoardUseCase
{
    private readonly IRecipeReadOnlyRepository _repository = repository;
    private readonly ILoggedUser _loggedUser = loggedUser;
    private readonly IMapper _mapper = mapper;
    public async Task<ResponseRecipesJson> Execute()
    {
        var user = await _loggedUser.User();
        var recipes =  await _repository.GetForDashBoard(user);
        return new ResponseRecipesJson()
        {
            Recipes = _mapper.Map<IList<ResponseShortRecipeJson>>(recipes)
        };

    }
}