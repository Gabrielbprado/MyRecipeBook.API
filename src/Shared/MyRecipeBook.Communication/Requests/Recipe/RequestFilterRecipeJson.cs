using MyRecipeBook.Communication.Enums;

namespace MyRecipeBook.Communication.Requests.Recipe;

public class RequestFilterRecipeJson
{
    public string? RecipeName_Ingredient { get; set; } = string.Empty;
    public IList<CookingTime>? CookingTime { get; set; } = [];
    public IList<Difficulty>? Difficulty { get; set; } = [];
    public IList<DishTypes>? DishTypes { get; set; } = [];
}