using System.Data;
using FluentValidation;
using MyRecipeBook.Communication.Requests.Recipe;
using MyRecipeBook.Domain.ValueObjects;
using MyRecipeBook.Exceptions;

namespace MyRecipeBook.Application.UseCases.Recipe.Generate;

public class GenerateRecipeValidator : AbstractValidator<RequestGenerateRecipeJson>
{
    public GenerateRecipeValidator()
    {
        var maxIngredients = MyRecipeBookRuleConstants.MAXIMUM_INGREDIENTS_GENERATE_RECIPE;
        RuleFor(r => r.Ingredients.Count).InclusiveBetween(1,maxIngredients).WithMessage(ResourceLanguage.INGREDIENT_NUMBER_INVALID);
        RuleFor(r => r.Ingredients).Must(ingredients => ingredients.Count == ingredients.Distinct().Count()).WithMessage(ResourceLanguage.DUPLICATED_INGREDIENT);
        RuleForEach(r => r.Ingredients).Custom((value, context) =>
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                context.AddFailure("Ingredient", ResourceLanguage.INGREDIENT_NAME_EMPTY);
            }
            else if (value.Count(c => c == ' ') > 3 || value.Count(c => c == '/') > 1)
            {
                context.AddFailure("Ingredient", ResourceLanguage.INGREDIENT_NAME_EMPTY);
            }
        });
    }
}