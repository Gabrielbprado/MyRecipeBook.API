using MyRecipeBook.Communication.Enums;

namespace MyRecipeBook.Communication.Responses.Recipe;

public class ResponseRecipeJson
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public IList<ResponseIngredientJson> Ingredients { get; set; } = [];
    public IList<ResponseInstructionsJson> Instructions { get; set; } = [];
    public IList<DishTypes> DishTypes { get; set; } = [];
    public CookingTime? CookingTime { get; set; }
    public Difficulty? Difficulty { get; set; }
}