using MyRecipeBook.Communication.Enums;

namespace MyRecipeBook.Communication.Requests.Recipe;

public  class RequestRecipeJson
{
    public string Title { get; set; } = string.Empty;
    public CookingTime CookingTime { get; set; }
    public Difficulty Difficulty { get; set; }
    public IList<RequestIngredientJson> Ingredients { get; set; } = [];
    public IList<RequestInstructionJson> Instructions { get; set; } = [];
    public IList<DishTypes> DishTypes { get; set; } = [];
}