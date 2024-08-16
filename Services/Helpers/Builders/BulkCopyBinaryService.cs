using BulkInsertAPI.Services.Helpers.Mappers;
using Npgsql;

namespace BulkInsertAPI.Services.Helpers.Builders
{
    public class BulkCopyBinaryService
    {
        public async Task BulkInsertBinaryAsync<T>(NpgsqlConnection connection, string copyBinarySqlStatement, List<T> items)
        {
            if (items == null || items.Count == 0)
            {
                return;
            }

            var properties = typeof(T).GetProperties();
            var propertyTypeMappings = properties
                .Select(p => NpgSqlDbTypeMapper.GetNpgSqlDbType(p.PropertyType))
                .ToArray();

            using var writer = await connection.BeginBinaryImportAsync(copyBinarySqlStatement);

            for(int itemIterator = 0; itemIterator < items.Count; itemIterator++)
            {
                writer.StartRow();

                for (int propIterator = 0; propIterator < properties.Length; propIterator++)
                {
                    var property = properties[propIterator];
                    var value = property.GetValue(items[itemIterator]);
                    var npgsqlType = propertyTypeMappings[propIterator];

                    if (value is null) writer.WriteNull();
                    else writer.Write(value, npgsqlType);
                }
            }

            await writer.CompleteAsync();
        }
    }
}
