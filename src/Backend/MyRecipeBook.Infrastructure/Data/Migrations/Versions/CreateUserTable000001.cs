using FluentMigrator;

namespace MyRecipeBook.Infrastructure.Data.Migrations.Versions;

[Migration(VersionMigrationNumber.CreateUserTable,"Create User Table")]
public class CreateUserTable000001 : BaseMigration
{
    public override void Up()
    {
        CreateTable("Users")
            .WithColumn("Name").AsString().NotNullable()
            .WithColumn("Email").AsString().NotNullable()
            .WithColumn("Password").AsString().NotNullable()
            .WithColumn("UserIdentifier").AsGuid().NotNullable();
    }
}