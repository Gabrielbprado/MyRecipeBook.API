using Bogus;
using MyRecipeBook.Communication.Enums;
using MyRecipeBook.Communication.Requests.Recipe;

namespace CommonTestUtilities.Requests.Recipe.Filter;

public class RequestFilterRecipeJsonBuilder
{
    public static RequestFilterRecipeJson Builder()
    {
        return new Faker<RequestFilterRecipeJson>()
            .RuleFor(r => r.RecipeName_Ingredient, f => f.Lorem.Word())
            .RuleFor(r => r.CookingTime, f => f.Make(1, f.PickRandom<CookingTime>))
            .RuleFor(r => r.DishTypes, f => f.Make(1, f.PickRandom<DishTypes>))
            .RuleFor(r => r.Difficulty, f => f.Make(1, f.PickRandom<Difficulty>));
    }
}