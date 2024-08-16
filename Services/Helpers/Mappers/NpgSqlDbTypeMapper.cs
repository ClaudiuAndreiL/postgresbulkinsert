using NpgsqlTypes;

namespace BulkInsertAPI.Services.Helpers.Mappers
{
    public static class NpgSqlDbTypeMapper
    {
        public static NpgsqlDbType GetNpgSqlDbType(Type type)
        {
            if (type == typeof(Guid)) return NpgsqlDbType.Uuid;
            if (type == typeof(string)) return NpgsqlDbType.Text;
            if (type == typeof(int)) return NpgsqlDbType.Integer;
            if (type == typeof(DateTime)) return NpgsqlDbType.Timestamp;
            if (type == typeof(bool)) return NpgsqlDbType.Boolean;
            if (type == typeof(long)) return NpgsqlDbType.Bigint;
            if (type == typeof(decimal)) return NpgsqlDbType.Numeric;
            if (type == typeof(double)) return NpgsqlDbType.Double;
            // Add more mappings as needed

            throw new InvalidOperationException($"Unsupported type: {type}");
        }
    }
}
