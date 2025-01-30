using System.Diagnostics;

using Npgsql;

using BulkInsertAPI.Services.Helpers.Builders;
using BulkInsertAPI.Services.Helpers.Serializers;


namespace BulkInsertAPI.Services;

public interface IBulkInsertBinaryService<T> where T: class
{
    Task PerformBulkInsertBinaryAsync(List<T> messages);
}

public class BulkInsertBinaryService<T> : IBulkInsertBinaryService<T>
    where T : class
{
    private readonly IConfiguration _configuration;
    private readonly INpgsqlEntityBinarySerializer<T> _entitySerializer;
    private readonly IBulkInsertBinaryStatementBuilder<T> _bulkInsertBinaryStatementBuilder;

    public BulkInsertBinaryService(IConfiguration configuration, 
        INpgsqlEntityBinarySerializer<T> entitySerializer, 
        IBulkInsertBinaryStatementBuilder<T> bulkInsertBinaryStatementBuilder)
    {
        _configuration = configuration;
        _entitySerializer = entitySerializer;
        _bulkInsertBinaryStatementBuilder = bulkInsertBinaryStatementBuilder;
    }

    public async Task PerformBulkInsertBinaryAsync(List<T> messages)
    {
        var sw = Stopwatch.StartNew();

        var connectionString = _configuration.GetConnectionString("DefaultConnection");

        using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync();

        var statements = _bulkInsertBinaryStatementBuilder.GetStatements();
        await RunCommandAsync(connection, statements.CreateTempTableSqlStatement);
        await BulkInsertBinaryAsync(connection, statements.CopyBinarySqlStatement, messages);

        Console.WriteLine($"Inserted into temp -  {sw.Elapsed.TotalMilliseconds}ms");

        await using var command = connection.CreateCommand();
        command.CommandText = statements.InsertTempTableSqlStatement;
        await command.ExecuteNonQueryAsync();
        Console.WriteLine($"Copied to target from temp -  {sw.Elapsed.TotalMilliseconds}ms");

        await RunCommandAsync(connection, statements.DropTempTableSqlStatement);
    }

    private async Task BulkInsertBinaryAsync(NpgsqlConnection connection, string copyBinarySqlStatement, List<T> messages)
    {
        using var writer = await connection.BeginBinaryImportAsync(copyBinarySqlStatement);

        foreach (var message in messages)
        {
            await _entitySerializer.BulkInsertBinaryAsync(writer, connection, message);
        }

        await writer.CompleteAsync();
    }

    private async static Task RunCommandAsync(NpgsqlConnection connection, string sqlStatement)
    {
        await using var command = connection.CreateCommand();
        command.CommandText = sqlStatement;
        await command.ExecuteNonQueryAsync();
    }
}
