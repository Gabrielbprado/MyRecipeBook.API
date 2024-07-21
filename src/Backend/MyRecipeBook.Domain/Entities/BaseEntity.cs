using System.ComponentModel.DataAnnotations;

namespace MyRecipeBook.Domain.Entities;

public abstract class BaseEntity
{
    [Key]
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;    

}