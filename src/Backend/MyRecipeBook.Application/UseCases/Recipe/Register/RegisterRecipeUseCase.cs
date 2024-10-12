using AutoMapper;
using MyRecipeBook.Communication.Requests.Recipe;
using MyRecipeBook.Communication.Responses.Recipe;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.Recipe;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Exceptions.BaseException;

namespace MyRecipeBook.Application.UseCases.Recipe;

public class RegisterRecipeUseCase(IRecipeWriteOnlyRepository recipeWriteOnlyRepository,IMapper mapper,IUnityOfWork unityOfWork,ILoggedUser loggedUser) : IRegisterRecipeUseCase
{
    private readonly IRecipeWriteOnlyRepository _recipeWriteOnlyRepository = recipeWriteOnlyRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IUnityOfWork _unityOfWork = unityOfWork;
    private readonly ILoggedUser _loggedUser = loggedUser;
    public async Task<ResponseRegisteredRecipeJson> Execute(RequestRecipeJson request)
    {
        await Validate(request);
        var recipe = _mapper.Map<Domain.Entities.Recipe>(request);
        var user = _loggedUser.User();
        recipe.UserId = user.Id;
        recipe.Ingredients = _mapper.Map<IList<Domain.Entities.Ingredient>>(request.Ingredients);
        var instructions = request.Instructions.OrderBy(i => i.Step).ToList();
        for(var index = 0; index < instructions.Count; index++)
            instructions.ElementAt(index).Step = index + 1;
        recipe.Instructions = _mapper.Map<IList<Domain.Entities.Instruction>>(instructions);
        await _recipeWriteOnlyRepository.Add(recipe);
        await _unityOfWork.Commit();
        return _mapper.Map<ResponseRegisteredRecipeJson>(recipe);
    }

    private async Task Validate(RequestRecipeJson request)
    {
        var validator = new RecipeValidator();
        var validationResult = await validator.ValidateAsync(request);
        if (validationResult.IsValid is false)
        {
            var errorMessage = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ErrorOnValidatorException(errorMessage);
        }
    }
}