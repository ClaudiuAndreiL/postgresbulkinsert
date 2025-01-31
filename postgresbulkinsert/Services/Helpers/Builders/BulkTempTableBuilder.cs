namespace BulkInsertAPI.Services.Helpers.Builders
{
    public static class BulkTempTableBuilder
    {
        private const string TempPrefix = "temp";

        public static string GetTempTableName(Type type)
        {
            return TempPrefix + '_' + type.Name.ToLower();
        }

        public static string GetCreateTempTableSqlStatement(Type type, string tempTableName)
        {
            return $@"CREATE TEMP TABLE {tempTableName} AS TABLE ""{type.Name}"" WITH NO DATA;";
        }

        public static string GetDropTempTableSqlStatement(string tempTableName)
        {
            return $@"DROP TABLE IF EXISTS {tempTableName};";
        }
    }
}
