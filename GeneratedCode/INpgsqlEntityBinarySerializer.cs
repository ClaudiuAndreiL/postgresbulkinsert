using Npgsql;

namespace GeneratedCode
{
    public interface INpgsqlEntityBinarySerializer<in T>
        where T : class
    {
        Task BulkInsertBinaryAsync(NpgsqlBinaryImporter writer, NpgsqlConnection connection, T message);
    }
}
