using System.Data;
using FluentMigrator;

namespace MyRecipeBook.Infrastructure.Data.Migrations.Versions;

[Migration(VersionMigrationNumber.CreateRecipeTable, "Create Recipe, Ingredients, Instructions and  DishTypes Tables")]

public class CreateRecipeTable000002 : BaseMigration
{
    public override void Up()
    {
        CreateTable("Recipes")
            .WithColumn("Title").AsString().NotNullable()
            .WithColumn("CookingTime").AsInt32().NotNullable()
            .WithColumn("Difficulty").AsInt32().NotNullable()
            .WithColumn("UserId").AsInt64().NotNullable().ForeignKey("FK_Recipes_Users_Id", "Users", "Id");

        CreateTable("Ingredients")
            .WithColumn("Item").AsString().NotNullable()
            .WithColumn("RecipeId").AsInt64().NotNullable().ForeignKey("FK_Ingredients_Recipes_Id", "Recipes", "Id")
            .OnDelete(Rule.Cascade);
        
        CreateTable("Instructions")
            .WithColumn("Step").AsInt32().NotNullable()
            .WithColumn("Text").AsString(200).NotNullable()
            .WithColumn("RecipeId").AsInt64().NotNullable().ForeignKey("FK_Instructions_Recipes_Id", "Recipes", "Id")
            .OnDelete(Rule.Cascade);
        
        CreateTable("DishTypes")
            .WithColumn("Type").AsInt32().NotNullable()
            .WithColumn("RecipeId").AsInt64().NotNullable().ForeignKey("FK_DishTypes_Recipes_Id", "Recipes", "Id")
            .OnDelete(Rule.Cascade);
    }
}