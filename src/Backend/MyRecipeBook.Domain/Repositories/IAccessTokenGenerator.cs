namespace MyRecipeBook.Domain.Repositories;

public interface IAccessTokenGenerator
{
    public string Generate(Guid userIdentifier);
}