using System.ComponentModel.DataAnnotations.Schema;
using MyRecipeBook.Domain.Enums;

namespace MyRecipeBook.Domain.Entities;

[Table("Recipes")]
public class Recipe : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public CookingTime CookingTime { get; set; }
    public Difficulty Difficulty { get; set; }
    public IList<Ingredient> Ingredients { get; set; } = [];
    public IList<Instruction> Instructions { get; set; } = [];
    public IList<DishTypes> DishTypes { get; set; } = [];
    public long UserId { get; set; }
}