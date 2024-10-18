using MyRecipeBook.Domain.Dtos;

namespace MyRecipeBook.Domain.Repositories.Recipe;

public interface IGenerateRecipeAi
{
    Task<GenerateRecipeDto> Generate(IList<string> ingredients);
}