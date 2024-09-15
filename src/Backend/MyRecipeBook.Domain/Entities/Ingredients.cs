namespace MyRecipeBook.Domain.Entities;

public abstract class Ingredients : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Quantity { get; set; } = string.Empty;
    public long RecipeId { get; set; }
}