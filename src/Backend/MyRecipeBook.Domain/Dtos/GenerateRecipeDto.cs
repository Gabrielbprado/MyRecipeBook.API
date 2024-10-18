using MyRecipeBook.Domain.Enums;

namespace MyRecipeBook.Domain.Dtos;

public class GenerateRecipeDto
{
    public string Title { get; set; } = string.Empty;
    public IList<string> Ingredients { get; set; } = [];
    public IList<InstructionsDto> Instructions { get; set; } = [];
    public IList<DishTypes> DishTypes { get; set; } = [];
    public CookingTime? CookingTime { get; set; }
    public Difficulty? Difficulty { get; set; }
}