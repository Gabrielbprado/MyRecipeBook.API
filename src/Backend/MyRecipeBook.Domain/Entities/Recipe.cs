using MyRecipeBook.Domain.Enums;

namespace MyRecipeBook.Domain.Entities;

public class Recipe : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public CookingTime CookingTime { get; set; }
    public Difficulty Difficulty { get; set; }
    public IList<Ingredients> Ingredients { get; set; } = [];
    public IList<Instructions> Instructions { get; set; } = [];
    public IList<DishTypes> DishTypes { get; set; } = [];
    public long UserId { get; set; }
}