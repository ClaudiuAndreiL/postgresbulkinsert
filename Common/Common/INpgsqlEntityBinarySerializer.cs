using System.Threading.Tasks;
using Npgsql;

namespace Common
{
    public interface INpgsqlEntityBinarySerializer<in T>
        where T : class
    {
        Task BulkInsertBinaryAsync(NpgsqlBinaryImporter writer, NpgsqlConnection connection, T message);
    }
}
