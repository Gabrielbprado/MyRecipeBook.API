using MyRecipeBook.Domain.Dtos;

namespace MyRecipeBook.Domain.Services.OpenAi;

public interface IGenerateRecipeAi
{
    Task<GenerateRecipeDto> Generate(IList<string> ingredients);
}