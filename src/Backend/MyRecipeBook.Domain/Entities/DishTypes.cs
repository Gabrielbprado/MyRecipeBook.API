namespace MyRecipeBook.Domain.Entities;

public  class DishTypes : BaseEntity
{
    public Enums.DishTypes Type { get; set; }
    public long RecipeId { get; set; }
}