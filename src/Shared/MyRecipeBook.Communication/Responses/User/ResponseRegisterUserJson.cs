namespace MyRecipeBook.Communication.Responses.User;

public class ResponseRegisterUserJson
{
    public string Name { get; set; } = string.Empty;
    public ResponseTokensJson Tokens { get; set; } = null!;
}