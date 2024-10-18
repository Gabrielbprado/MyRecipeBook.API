using AutoMapper;
using MyRecipeBook.Communication.Requests.Recipe;
using MyRecipeBook.Communication.Responses.Recipe;
using MyRecipeBook.Domain.Dtos;
using MyRecipeBook.Domain.Repositories.Recipe;
using MyRecipeBook.Exceptions.BaseException;

namespace MyRecipeBook.Application.UseCases.Recipe.Generate;

public class GenerateRecipeUseCase(IGenerateRecipeAi generateRecipeAi,IMapper mapper) : IGenerateRecipeUseCase
{
    private readonly IGenerateRecipeAi _generateRecipeAi = generateRecipeAi;
    private readonly IMapper _mapper = mapper;
    public async Task<ResponseGenerateRecipeJson> Execute(RequestGenerateRecipeJson request)
    {
        await Validate(request);
        var response = await _generateRecipeAi.Generate(request.Ingredients);
        return _mapper.Map<ResponseGenerateRecipeJson>(response);
    }

    private async Task Validate(RequestGenerateRecipeJson request)
    {
        var validator = new GenerateRecipeValidator();
        var validationResult = await validator.ValidateAsync(request);
        if (validationResult.IsValid is false)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ErrorOnValidatorException(errors);
        }
    }
}