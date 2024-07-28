using Dapper;
using Microsoft.Data.Sqlite;
using MySqlConnector;

namespace MyRecipeBook.Infrastructure.Data;

public static class Database
{
    public static void AddDatabase(string connectionString)
    {
        SQLitePCL.Batteries.Init();
        var connectionStringBuilder = new SqliteConnectionStringBuilder(connectionString);
        var databaseName = connectionStringBuilder.DataSource;
        using var connector = new SqliteConnection(connectionString);
        var param = new DynamicParameters();
        param.Add("databaseName", databaseName);
        if (!File.Exists(databaseName))
            using (File.Create(databaseName)) { }
    }
}