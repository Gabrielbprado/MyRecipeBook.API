namespace MyRecipeBook.Exceptions.BaseException;

public class NotFoundException(string message) : MyRecipeBookException(message)
{
    
}