using MyRecipeBook.Domain.Enums;

namespace MyRecipeBook.Domain.Dtos;

public record FilterRecipesDto
{
    public string? RecipeName_Ingredient { get; init; } = string.Empty;
    public IList<CookingTime> CookingTime { get; init; } = [];
    public IList<Difficulty> Difficulty { get; init; } = [];
    public IList<DishTypes> DishTypes { get; init; } = [];
}