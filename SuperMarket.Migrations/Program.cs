using FluentMigrator.Runner;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public class Program
{
    static void Main(string[] args)
    {
        var options = GetSettings(args, Directory.GetCurrentDirectory());

        var connectionString = options.ConnectionString;

        CreateDatabase(connectionString);
        var runner = CreateServices(connectionString);
        runner.MigrateUp();
    }

    private static IMigrationRunner CreateServices(string connectionString)
    {
        var container = new ServiceCollection()
            .AddFluentMigratorCore()
            .ConfigureRunner(_ => _
                .AddSqlServer()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(typeof(_202205020830_InitialTables).Assembly).For.All())
            .AddLogging(_ => _.AddFluentMigratorConsole())
            .BuildServiceProvider();
        return container.GetRequiredService<IMigrationRunner>();
    }

    private static void CreateDatabase(string connectionString)
    {
        var databaseName = GetDatabaseName(connectionString);
        string masterConnectionString = ChangeDatabaseName(connectionString, "master");
        var commandScript = $"if db_id(N'{databaseName}') is null create database [{databaseName}]";

        using var connection = new SqlConnection(masterConnectionString);
        using var command = new SqlCommand(commandScript, connection);
        connection.Open();
        command.ExecuteNonQuery();
        connection.Close();
    }

    private static MigrationSettings GetSettings(string[] args, string baseDir)
    {
        IConfigurationRoot configurations = new ConfigurationBuilder()
            .SetBasePath(baseDir)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .AddCommandLine(args)
            .Build();

        var settings = new MigrationSettings();
        settings.ConnectionString = configurations.GetValue<string>("ConnectionString");
        return settings;
    }

    private static string ChangeDatabaseName(string connectionString, string databaseName)
    {
        var csb = new SqlConnectionStringBuilder(connectionString)
        {
            InitialCatalog = databaseName
        };
        return csb.ConnectionString;
    }

    private static string GetDatabaseName(string connectionString)
    {
        return new SqlConnectionStringBuilder(connectionString).InitialCatalog;
    }
}
public class MigrationSettings
{
    public string ConnectionString { get; set; }
}