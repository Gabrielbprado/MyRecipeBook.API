namespace MyRecipeBook.Application.UseCases.Recipe.Delete;

public interface IRecipeDeleteUseCase
{
    Task Execute(long id);
}