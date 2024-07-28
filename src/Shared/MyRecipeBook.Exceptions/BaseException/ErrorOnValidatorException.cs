namespace MyRecipeBook.Exceptions.BaseException;

public class ErrorOnValidatorException(IList<string> errorMessage) : MyRecipeBookException
{
       public IList<string> ErrorMessage = errorMessage;
}