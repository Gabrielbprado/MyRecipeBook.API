using AutoMapper;
using MyRecipeBook.Communication.Requests.Recipe;
using MyRecipeBook.Communication.Responses.Recipe;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.Recipe;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Exceptions.BaseException;

namespace MyRecipeBook.Application.UseCases.Recipe.Recipe;

public class UpdateRecipeUseCase(IRecipeUpdateOnlyRepository updateOnlyRepository,IUnityOfWork unityOfWork,IMapper mapper,ILoggedUser loggedUser) : IUpdateRecipeUseCase
{
    private readonly IRecipeUpdateOnlyRepository _updateOnlyRepository = updateOnlyRepository;
    private readonly IUnityOfWork _unityOfWork = unityOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly ILoggedUser _loggedUser = loggedUser;
    public async Task<ResponseRecipeJson> Execute(long recipeId, RequestRecipeJson request)
    {
        await Validate(request);
        var user = await _loggedUser.User();
        var recipe = await _updateOnlyRepository.GetById(user!,recipeId);
        recipe.Instructions.Clear();
        recipe.Ingredients.Clear();
        recipe.DishTypes.Clear();
        _mapper.Map(request, recipe);
        var instructions = request.Instructions.OrderBy(i => i.Step).ToList();
        for(var index = 0; index < instructions.Count; index++)
            instructions.ElementAt(index).Step = index + 1;
        recipe.Instructions = _mapper.Map<IList<Domain.Entities.Instruction>>(instructions);
        _updateOnlyRepository.Update(recipe);
        await _unityOfWork.Commit();
        return _mapper.Map<ResponseRecipeJson>(recipe);
    }

    private async Task Validate(RequestRecipeJson request)
    {
        var validate = new RecipeValidator();
        var response = await validate.ValidateAsync(request);
        if (response.IsValid is false)
        {
            var errors = response.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ErrorOnValidatorException(errors);
        }

    }
}