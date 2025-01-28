using System.Diagnostics;
using BulkInsertAPI.Data.Models;
using BulkInsertAPI.Services.Helpers.Builders;
using BulkInsertAPI.Services.Helpers.Serializers;
using Npgsql;

namespace BulkInsertAPI.Services;

public interface IBulkInsertBinaryService
{
    Task PerformBulkInsertBinaryAsync(List<Message> messages);
}

//TODO: make this typed
public class BulkInsertBinaryService : IBulkInsertBinaryService
{
    private readonly IConfiguration _configuration;
    private readonly INpgsqlEntityBinarySerializer<Message> _entitySerializer;

    public BulkInsertBinaryService(IConfiguration configuration, INpgsqlEntityBinarySerializer<Message> entitySerializer)
    {
        _configuration = configuration;
        _entitySerializer = entitySerializer;
    }

    public async Task PerformBulkInsertBinaryAsync(List<Message> messages)
    {
        var sw = Stopwatch.StartNew();

        var connectionString = _configuration.GetConnectionString("DefaultConnection");

        using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync();

        var statements = BulkInsertBinaryStatementBuilder<Message>.GetStatements();
        await RunCommandAsync(connection, statements.CreateTempTableSqlStatement);
        await BulkInsertBinaryAsync(connection, statements.CopyBinarySqlStatement, messages);

        Console.WriteLine($"Inserted into temp -  {sw.Elapsed.TotalMilliseconds}ms");

        await using var command = connection.CreateCommand();
        command.CommandText = statements.InsertTempTableSqlStatement;
        await command.ExecuteNonQueryAsync();
        Console.WriteLine($"Copied to target from temp -  {sw.Elapsed.TotalMilliseconds}ms");

        await RunCommandAsync(connection, statements.DropTempTableSqlStatement);
    }

    private async Task BulkInsertBinaryAsync(NpgsqlConnection connection, string copyBinarySqlStatement, List<Message> messages)
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
