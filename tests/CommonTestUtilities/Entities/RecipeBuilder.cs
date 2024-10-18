using Bogus;
using MyRecipeBook.Communication.Enums;
using MyRecipeBook.Domain.Entities;
using CookingTime = MyRecipeBook.Domain.Enums.CookingTime;
using Difficulty = MyRecipeBook.Domain.Enums.Difficulty;
using DishTypes = MyRecipeBook.Domain.Entities.DishTypes;

namespace CommonTestUtilities.Entities;

public class RecipeBuilder
{
    private List<Recipe?> recipes = new List<Recipe?>();
    
    public List<Recipe?> Collection(User user, int count = 2)
    {
        if (count <= 0)
            count = 1;
        int recipeId = 1;
        for (int i = 0; i < count; i++)
        {
            var fakeRecipe = Builder(user);
            fakeRecipe.Id = recipeId++;
            recipes.Add(fakeRecipe);
        }

        return recipes;
    }

    public static Recipe? Builder(User user)
    {
        return new Faker<Recipe>()
            .RuleFor(r => r.Id, () => 1)
            .RuleFor(r => r.Title, (f) => f.Lorem.Word())
            .RuleFor(r => r.CookingTime, (f) => f.PickRandom<CookingTime>())
            .RuleFor(r => r.Difficulty, (f) => f.PickRandom<Difficulty>())
            .RuleFor(r => r.Ingredients, (f) => f.Make(1, () => new Ingredient
            {
                Id = 1,
                Item = f.Commerce.ProductName()
            }))
            .RuleFor(r => r.Instructions, (f) => f.Make(1, () => new Instruction
            {
                Id = 1,
                Step = 1,
                Text = f.Lorem.Paragraph()
            }))
            .RuleFor(u => u.DishTypes, (f) => f.Make(1, () => new MyRecipeBook.Domain.Entities.DishTypes
            {
                Id = 1,
                Type = f.PickRandom<MyRecipeBook.Domain.Enums.DishTypes>()
            }))
            .RuleFor(r => r.UserId, _ => user.Id);
    }
    
    
}