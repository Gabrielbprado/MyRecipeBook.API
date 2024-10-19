using Azure;
using Azure.AI.OpenAI;
using MyRecipeBook.Domain.Dtos;
using MyRecipeBook.Domain.Enums;
using MyRecipeBook.Domain.Services.OpenAi;


namespace MyRecipeBook.Infrastructure.Services.OpenAi;

public class ChatGptService(OpenAIClient  openAiApi) : IGenerateRecipeAi
{
    private readonly OpenAIClient _openAiApi = openAiApi;
    private const string CHAT_MODEL = "gpt-35-turbo";

    public async Task<GenerateRecipeDto> Generate(IList<string> ingredients)
    {
        ChatCompletionsOptions options = new ChatCompletionsOptions()
        {
            Messages = { new ChatMessage(ChatRole.System, ResourceOpenAi.DEFAULT_MESSAGE) },
            Temperature = (float)0.7,
            MaxTokens = 800,
            NucleusSamplingFactor = (float)0.95,
            FrequencyPenalty = 0,
            PresencePenalty = 0,
        };
        var ingredientsString = string.Join(";", ingredients);
        options.Messages.Add(new ChatMessage(ChatRole.User, ingredientsString));

        var response = await _openAiApi.GetChatCompletionsAsync(deploymentOrModelName: CHAT_MODEL, options);

        var completions = response.Value;
        var fullResponse = completions.Choices[0].Message.Content;
        var responseList = fullResponse
            .Split("\n")
            .Where(response => response.Trim().Equals(string.Empty) is false)
            .Select(item => item.Replace("[", "").Replace("]", ""))
            .ToList();
        
        var step = 1;
        var response22=  new GenerateRecipeDto()
        {
            Title = responseList[0],
            CookingTime = (CookingTime)Enum.Parse(typeof(CookingTime), responseList[1]),
            Ingredients = responseList[2].Split(";"),
            Instructions = responseList[3].Split("@").Select(instruction => new InstructionsDto
            {
                Text = instruction.Trim(),
                Step = step++
            }).ToList()
        };
        return response22;
    }
}