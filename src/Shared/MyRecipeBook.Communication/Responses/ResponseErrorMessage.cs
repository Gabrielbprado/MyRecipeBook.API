namespace MyRecipeBook.Communication.Responses;

public class ResponseErrorMessage
{
    public IList<string> Errors { get; set; } 
    public ResponseErrorMessage(IList<string> errors)
    {
        Errors = errors;
    }
    public ResponseErrorMessage(string errorMessage)
    {
        Errors = new List<string>();
        Errors.Add(errorMessage);

    }
}