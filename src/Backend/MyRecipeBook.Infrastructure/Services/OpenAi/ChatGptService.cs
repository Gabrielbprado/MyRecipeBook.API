using MyRecipeBook.Domain.Dtos;
using MyRecipeBook.Domain.Repositories.Recipe;

namespace MyRecipeBook.Infrastructure.Services.OpenAi;

public class ChatGptService : IGenerateRecipeAi
{
    public Task<GenerateRecipeDto> Generate(IList<string> ingredients)
    {
        throw new NotImplementedException();
    }
}