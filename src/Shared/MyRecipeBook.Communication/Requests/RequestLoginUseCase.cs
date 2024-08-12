namespace MyRecipeBook.Communication.Requests;

public class RequestLoginUseCase
{
    public string Password { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}