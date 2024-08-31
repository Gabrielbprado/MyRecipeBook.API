namespace MyRecipeBook.Exceptions.BaseException;

public class ErrorOnValidatorException(IList<string> errorMessage) : MyRecipeBookException(string.Empty)
{
       public IList<string> ErrorMessage = errorMessage;
}