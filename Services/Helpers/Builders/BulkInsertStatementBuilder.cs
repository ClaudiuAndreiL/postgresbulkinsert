using BulkInsertAPI.Services.Helpers.Model;
using System.Text;

namespace BulkInsertAPI.Services.Helpers.Builders
{
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

            statements.CopyBinarySqlStatement = BulkCopyStatementBuilder.GetCopyBinaryStatement(type, tempTableName);

            statements.InsertTempTableSqlStatement = GetInsertStatement(type, tempTableName, uniqueness);

            return statements;
        }

        private static string GetInsertStatement(Type type, string tempTableName, string[] uniqueness)
        {
            var sb = new StringBuilder();

            sb.Append($"INSERT INTO \"");
            sb.Append(type.Name);
            sb.Append("\" (");

            var propertyNames = type.GetProperties().Select(x => x.Name);
            var tableProperties = "\"" + string.Join("\", \"", propertyNames) + "\"";
            sb.Append(tableProperties);

            sb.Append(") SELECT ");
            sb.Append(tableProperties);
            sb.Append(" FROM ");
            sb.Append(tempTableName);

            if (uniqueness.Length > 0)
            {
                sb.Append(" ON CONFLICT ");
                var uniquenessColumns = "(\"" + string.Join("\", \"", uniqueness) + "\")";
                sb.Append(uniquenessColumns);
                sb.Append(" DO NOTHING;");
            }

            return sb.ToString();
        }
    }
}
