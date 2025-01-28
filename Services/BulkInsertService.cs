using Npgsql;
using BulkInsertAPI.Data.Models;
using Microsoft.EntityFrameworkCore;
using BulkInsertAPI.Services.Helpers.Builders;
using BulkInsertAPI.Services.Helpers.Model;
using System.Diagnostics;

namespace BulkInsertAPI.Services
{
    public interface IBulkInsertService
    {
        Task PerformBulkInsertTextAsync(List<Message> messages);
    }

    public class BulkInsertService : IBulkInsertService
    {
        public IConfiguration Configuration { get; }


        public BulkInsertService(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public async Task PerformBulkInsertTextAsync(List<Message> messages)
        {
            var sw = Stopwatch.StartNew();
            var connectionString = Configuration.GetConnectionString("DefaultConnection");

            var statements = BulkInsertStatementBuilder.GetStatements(messages, [ nameof(Message.MessageId) ]);

            using var connection = new NpgsqlConnection(connectionString);

            await connection.OpenAsync();
            await RunCommandAsync(connection, statements.CreateTempTableSqlStatement); 
            Console.WriteLine($"Created temp -  {sw.Elapsed.TotalMilliseconds}ms");

            await BulkInsertAsync(connection, statements);
            Console.WriteLine($"Inserted into temp -  {sw.Elapsed.TotalMilliseconds}ms");

            using (var transaction = await connection.BeginTransactionAsync())
            {
                await using (var command = connection.CreateCommand())
                {
                    command.Transaction = transaction;
                    command.CommandText = statements.InsertTempTableSqlStatement;
                    await command.ExecuteNonQueryAsync();
                }

                await transaction.CommitAsync();
            }
            Console.WriteLine($"Copied to target from temp -  {sw.Elapsed.TotalMilliseconds}ms");

            await RunCommandAsync(connection, statements.DropTempTableSqlStatement);
        }
     
        private async Task BulkInsertAsync(NpgsqlConnection connection, BulkStatements statements)
        {
            using var writer = await connection.BeginTextImportAsync(statements.CopyTextSqlStatement);
            foreach (var line in statements.CopyTextLines)
            {
                await writer.WriteLineAsync(line);
            }
        }

        private async Task RunCommandAsync(NpgsqlConnection connection, string sqlStatement)
        {
            await using var command = connection.CreateCommand();
            command.CommandText = sqlStatement;
            await command.ExecuteNonQueryAsync();
        }
    }
}