namespace BulkInsertAPI.Services.Helpers.Model;

public class BulkCopyBinaryStatements
{
    public string CreateTempTableSqlStatement { get; set; } = default!;
    public string InsertTempTableSqlStatement { get; set; } = default!;
    public string DropTempTableSqlStatement { get; set; } = default!;
    public string CopyBinarySqlStatement { get; set; } = default!;
}
