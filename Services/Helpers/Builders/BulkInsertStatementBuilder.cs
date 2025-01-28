using BulkInsertAPI.Services.Helpers.Model;

namespace BulkInsertAPI.Services.Helpers.Builders;

public static class BulkInsertStatementBuilder
{
    public static BulkStatements GetStatements<T>(List<T> items, string[] uniqueness) where T : class
    {
        var statements = new BulkStatements();
        var type = typeof(T);

        var tempTableName = BulkTempTableBuilder.GetTempTableName(type);

        statements.CreateTempTableSqlStatement = BulkTempTableBuilder.GetCreateTempTableSqlStatement(type, tempTableName);
        statements.DropTempTableSqlStatement = BulkTempTableBuilder.GetDropTempTableSqlStatement(tempTableName);

        statements.CopyTextSqlStatement = BulkCopyStatementBuilder.GetCopyTextStatement(type, tempTableName);
        statements.CopyTextLines = BulkCopyTextLinesBuilder.ConvertToLines(items);

        statements.InsertTempTableSqlStatement = GetInsertStatement(type, tempTableName, uniqueness);

        return statements;
    }

    private static string GetInsertStatement(Type type, string tempTableName, string[] uniqueness)
    {
        var propertyNames = type.GetProperties().Select(x => x.Name);
        var tableProperties = "\"" + string.Join("\", \"", propertyNames) + "\"";
        var uniquenessColumns = "(\"" + string.Join("\", \"", uniqueness) + "\")";

        string query = $$"""
INSERT INTO "{{type.Name}}"
({{tableProperties}})
SELECT {{tableProperties}}
FROM {{tempTableName}}
ON CONFLICT ({{uniquenessColumns}}) DO NOTHING;
""";
        return query;
    }
}
