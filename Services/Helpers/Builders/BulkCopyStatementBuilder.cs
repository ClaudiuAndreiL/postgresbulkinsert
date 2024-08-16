namespace BulkInsertAPI.Services.Helpers.Builders
{
    public static class BulkCopyStatementBuilder
    {
        public static string GetCopyTextStatement(Type type, string tempTableName)
        {
            var propertyNames = type.GetProperties().Select(x => x.Name).ToList();
            var tableProperties = "\"" + string.Join("\", \"", propertyNames) + "\"";

            return $"COPY {tempTableName} ({tableProperties}) FROM STDIN (FORMAT TEXT)";
        }

        public static string GetCopyBinaryStatement(Type type, string tempTableName)
        {
            var propertyNames = type.GetProperties().Select(x => x.Name).ToList();
            var tableProperties = "\"" + string.Join("\", \"", propertyNames) + "\"";

            return $"COPY {tempTableName} ({tableProperties}) FROM STDIN (FORMAT BINARY)";
        }
    }
}
