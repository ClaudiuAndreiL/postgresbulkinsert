using System.Diagnostics.CodeAnalysis;
using BulkInsertAPI.Services.Helpers.Model;

namespace BulkInsertAPI.Services.Helpers.Builders;

[SuppressMessage("SonarQube", "S2743", Justification = "Per-type static initialization is required for this scenario.")]
public static class BulkInsertBinaryStatementBuilder<T>
{
    public static string CreateTempTableSqlStatement { get; private set; }

    public static string CopyBinarySqlStatement { get; private set; }

    public static string InsertTempTableSqlStatement { get; private set; }
    public static string DropTempTableSqlStatement { get; private set; }

    private const string TempPrefix = "temp";

    private static string TypeName => typeof(T).Name;

    public static string GetTempTableName() => TempPrefix + '_' + TypeName.ToLower();

    public static string GetCreateTempTableSqlStatement()
    {
        string tempTableName = GetTempTableName();
        return $@"CREATE TEMP TABLE {tempTableName} AS TABLE ""{TypeName}"" WITH NO DATA;";
    }

    public static string GetDropTempTableSqlStatement()
    {
        string tempTableName = GetTempTableName();
        return $@"DROP TABLE IF EXISTS {tempTableName};";
    }

    public static string GetCopyBinaryStatement()
    {
        string tempTableName = GetTempTableName();
        //TODO: exclude some properties (e.g. autoincrement ones)
        var propertyNames = typeof(T).GetProperties().Select(x => x.Name).ToList();
        var tableProperties = "\"" + string.Join("\", \"", propertyNames) + "\"";

        return $"COPY {tempTableName} ({tableProperties}) FROM STDIN (FORMAT BINARY)";
    }

    private static string GetInsertStatement()
    {
        //TODO: get it dynamically from the entity type (where key attribute exists)
        string[] uniqueness = ["MessageId"];
        string tempTableName = GetTempTableName();

        var propertyNames = typeof(T).GetProperties().Select(x => x.Name);
        var tableProperties = "\"" + string.Join("\", \"", propertyNames) + "\"";
        var uniquenessColumns = "\"" + string.Join("\", \"", uniqueness) + "\"";

        string query = $$"""
INSERT INTO "{{TypeName}}"
({{tableProperties}})
SELECT {{tableProperties}}
FROM {{tempTableName}}
ON CONFLICT ({{uniquenessColumns}}) DO NOTHING;
""";
        return query;
    }

    static BulkInsertBinaryStatementBuilder()
    {
        CreateTempTableSqlStatement = GetCreateTempTableSqlStatement();
        CopyBinarySqlStatement = GetCopyBinaryStatement();
        InsertTempTableSqlStatement = GetInsertStatement();
        DropTempTableSqlStatement = GetDropTempTableSqlStatement();
    }

    public static BulkCopyBinaryStatements GetStatements()
    {
        return new BulkCopyBinaryStatements
        {
            CreateTempTableSqlStatement = CreateTempTableSqlStatement,
            CopyBinarySqlStatement = CopyBinarySqlStatement,
            InsertTempTableSqlStatement = InsertTempTableSqlStatement,
            DropTempTableSqlStatement = DropTempTableSqlStatement
        };
    }
}
