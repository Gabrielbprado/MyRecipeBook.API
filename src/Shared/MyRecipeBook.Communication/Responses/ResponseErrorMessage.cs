namespace MyRecipeBook.Communication.Responses;

public class ResponseErrorMessage
{
    public IList<string> ErrorMessage { get; set; } 
    public ResponseErrorMessage(IList<string> errorMessage)
    {
        ErrorMessage = errorMessage;
    }
    public ResponseErrorMessage(string errorMessage)
    {
        ErrorMessage = new List<string>();
        ErrorMessage.Add(errorMessage);

    }
}