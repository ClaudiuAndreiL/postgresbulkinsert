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
        Task PerformBulkInsertBinaryAsync(List<Message> messages);
    }

    public class BulkInsertService : IBulkInsertService
    {
        private const string TempMessageTableName = "temp_message";

        public IConfiguration Configuration { get; }

        public BulkInsertService(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public async Task PerformBulkInsertTextAsync(List<Message> messages)
        {
            var sw = Stopwatch.StartNew();
            var connectionString = Configuration.GetConnectionString("DefaultConnection");

            var statements = BulkInsertStatementBuilder.GetStatements(messages, [ nameof(Message.Id) ]);

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
     


        public async Task PerformBulkInsertBinaryAsync(List<Message> messages)
        {
            var sw = Stopwatch.StartNew();

            var connectionString = Configuration.GetConnectionString("DefaultConnection");

            var statements = BulkInsertStatementBuilder.GetStatements(messages, [nameof(Message.Id)]);

            using var connection = new NpgsqlConnection(connectionString);
            await connection.OpenAsync();
            await RunCommandAsync(connection, statements.CreateTempTableSqlStatement);
            Console.WriteLine($"Created temp -  {sw.Elapsed.TotalMilliseconds}ms");

            await BulkInsertBinaryAsync(connection, statements, messages);
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

        private async Task BulkInsertBinaryAsync<T>(NpgsqlConnection connection, BulkStatements statement, List<T> items) where T : class
        {
            var binaryWritter = new BulkCopyBinaryService();
            await binaryWritter.BulkInsertBinaryAsync(connection, statement.CopyBinarySqlStatement, items);
        }

        private async Task BulkInsertBinaryAsync(NpgsqlConnection connection, BulkStatements statement, List<Message> messages)
        {
            using (var writer = await connection.BeginBinaryImportAsync(statement.CopyBinarySqlStatement))
            {
                foreach (var message in messages)
                {
                    writer.StartRow();

                    // Write data in binary format
                    writer.Write(message.Id, NpgsqlTypes.NpgsqlDbType.Uuid);
                    writer.Write(message.Originator, NpgsqlTypes.NpgsqlDbType.Text);
                    writer.Write(message.Recipient, NpgsqlTypes.NpgsqlDbType.Text);
                    writer.Write(message.CharacterSet, NpgsqlTypes.NpgsqlDbType.Text);
                    writer.Write(message.Body, NpgsqlTypes.NpgsqlDbType.Text);
                    writer.Write(message.MessagePartCount, NpgsqlTypes.NpgsqlDbType.Integer);
                    writer.Write(message.SentAt, NpgsqlTypes.NpgsqlDbType.Timestamp);
                }
                await writer.CompleteAsync();
            }
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