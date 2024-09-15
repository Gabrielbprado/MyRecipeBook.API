using FluentValidation;
using MyRecipeBook.Communication.Requests.Recipe;
using MyRecipeBook.Exceptions;

namespace MyRecipeBook.Application.UseCases.Recipe;

public class RecipeValidator : AbstractValidator<RequestRecipeJson>
{
    public RecipeValidator()
    {
        CascadeMode = CascadeMode.Stop;
        RuleFor(r => r.Title).NotEmpty().WithMessage(ResourceLanguage.RECIPE_TITLE_EMPTY);
        RuleFor(r => r.CookingTime).IsInEnum().WithMessage(ResourceLanguage.RECIPE_COOKING_TIME_INVALID);
        RuleFor(r => r.Difficulty).IsInEnum().WithMessage(ResourceLanguage.RECIPE_DIFFICULTY_INVALID);
        RuleFor(r => r.DishTypes.Count).GreaterThan(0).WithMessage(ResourceLanguage.DISH_TYPE_INVALID);
        RuleFor(r => r.Ingredients.Count).GreaterThan(0).WithMessage(ResourceLanguage.AT_LEAST_ONE_INGREDIENT);
        RuleFor(r => r.Instructions.Count).GreaterThan(0).WithMessage(ResourceLanguage.AT_LEAST_ONE_INSCTRUCTION);
        RuleForEach(r => r.DishTypes).IsInEnum().WithMessage(ResourceLanguage.DISH_TYPE_INVALID);
            
        RuleForEach(r => r.Instructions).ChildRules(i =>
        {
            i.RuleFor(instruction => instruction.Step).NotEmpty().WithMessage(ResourceLanguage.INSTRUCTION_STEP_EMPTY)
                .GreaterThan(0).WithMessage(ResourceLanguage.STEP_MUST_BE_GREATER_THAN_ZERO);
            i.RuleFor(instruction => instruction.Text).NotEmpty().WithMessage(ResourceLanguage.INSTRUCTION_TEXT_EMPTY)
                .MaximumLength(2000).WithMessage(ResourceLanguage.INSTRUCTION_TEXT_MAX_LENGTH);
        });
        
        RuleForEach(r => r.Ingredients).ChildRules(i =>
        {
            i.RuleFor(ingredient => ingredient.Name).NotEmpty().WithMessage(ResourceLanguage.INGREDIENT_NAME_EMPTY);
            i.RuleFor(ingredient => ingredient.Quantity).NotEmpty().WithMessage(ResourceLanguage.INGREDIENT_QUANTITY_EMPTY);
        });
        
        RuleFor(r => r.Instructions).Must(i => i.Count == i.Select(x => x.Step).Distinct().Count())
            .WithMessage(ResourceLanguage.INSTRUCTION_STEP_DUPLICATE);

    }
}