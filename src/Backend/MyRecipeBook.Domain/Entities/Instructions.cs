namespace MyRecipeBook.Domain.Entities;

public abstract class Instructions : BaseEntity
{
    public int Step { get; set; }
    public string Text { get; set; } = string.Empty;
    public long RecipeId { get; set; }
}