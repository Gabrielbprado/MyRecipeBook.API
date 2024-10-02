using FluentValidation;
using MyRecipeBook.Communication.Requests.Recipe;
using MyRecipeBook.Exceptions;

namespace MyRecipeBook.Application.UseCases.Recipe.Filter;

public class FilterRecipeValidator : AbstractValidator<RequestFilterRecipeJson>
{
    public FilterRecipeValidator()
    {

        RuleForEach(x => x.CookingTime)
            .IsInEnum()
            .WithMessage(ResourceLanguage.RECIPE_COOKING_TIME_INVALID);

        RuleForEach(x => x.Difficulty)
            .IsInEnum()
            .WithMessage(ResourceLanguage.RECIPE_DIFFICULTY_INVALID);

        RuleForEach(x => x.DishTypes)
            .IsInEnum()
            .WithMessage(ResourceLanguage.DISH_TYPE_INVALID);
    }
}