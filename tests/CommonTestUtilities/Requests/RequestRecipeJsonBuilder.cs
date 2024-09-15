using Bogus;
using MyRecipeBook.Communication.Enums;
using MyRecipeBook.Communication.Requests.Recipe;


namespace CommonTestUtilities.Requests;

public class RequestRecipeJsonBuilder
{
    public static RequestRecipeJson Build()
    {
        var step = 1;
        return new Faker<RequestRecipeJson>()
            .RuleFor(r => r.Title, f => f.Lorem.Sentence())
            .RuleFor(r => r.CookingTime, f => f.PickRandom<CookingTime>())
            .RuleFor(r => r.Difficulty, f => f.PickRandom<Difficulty>())
            .RuleFor(r => r.DishTypes, f => f.Make(3, f.PickRandom<DishTypes>))
            .RuleFor(r => r.Instructions, f => f.Make(3, () => new RequestInstructionJson()
            {
                Step = step++,
                Text = f.Lorem.Sentence()
            }))
            .RuleFor(r => r.Ingredients, f => f.Make(3, () => new RequestIngredientJson()
            {
                Name = f.Lorem.Sentence(),
                Quantity = f.Random.Decimal(1, 100).ToString()
            }));

    }
}