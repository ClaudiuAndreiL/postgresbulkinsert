namespace BulkInsertAPI.Services.Helpers.Model
{
    public class BulkStatements
    {

        public string CreateTempTableSqlStatement { get; set; } = default!;


        /// <summary>
        /// Desired sample   var messageLines = messages.Select(m => $"{m.Id}\t{Escape(m.Originator)}\t{Escape(m.Recipient)}\t{Escape(m.CharacterSet)}\t{Escape(m.Body)}\t{m.MessagePartCount}\t{m.SentAt:yyyy-MM-dd HH:mm:ss.ffffff}").ToList();
        /// </summary>
        public List<string> CopyTextLines { get; set; } = default!;

        /*
          private readonly string CopyTextSql = $"COPY {TempMessageTableName} (" +
             $"\"{nameof(Message.Id)}\", \"{nameof(Message.Originator)}\", \"{nameof(Message.Recipient)}\", " +
             $"\"{nameof(Message.CharacterSet)}\", \"{nameof(Message.Body)}\", " +
             $"\"{nameof(Message.MessagePartCount)}\", \"{nameof(Message.SentAt)}\"" +
             $") FROM STDIN (FORMAT TEXT)";

        private readonly string CopyBinarySql = $"COPY {TempMessageTableName} (" +
                $"\"{nameof(Message.Id)}\", \"{nameof(Message.Originator)}\", \"{nameof(Message.Recipient)}\", " +
                $"\"{nameof(Message.CharacterSet)}\", \"{nameof(Message.Body)}\", " +
                $"\"{nameof(Message.MessagePartCount)}\", \"{nameof(Message.SentAt)}\"" +
                $") FROM STDIN (FORMAT BINARY)";
         
         */
        public string CopyTextSqlStatement { get; set; } = default!;
        public string CopyBinarySqlStatement { get; set; } = default!;

        /// <summary>
        /// Desired SQL:   var insertSql = $@"INSERT INTO ""{nameof(Message)}"" (""{nameof(Message.Id)}"", ""{nameof(Message.Originator)}"", ""{nameof(Message.Recipient)}"",""{nameof(Message.CharacterSet)}"",""{nameof(Message.Body)}"",""{nameof(Message.MessagePartCount)}"", ""{nameof(Message.SentAt)}"") 
        /// SELECT ""{nameof(Message.Id)}"", ""{nameof(Message.Originator)}
        /// "", ""{ nameof(Message.Recipient)}
        /// "",""{ nameof(Message.CharacterSet)}
        /// "",""{ nameof(Message.Body)}
        /// "",""{ nameof(Message.MessagePartCount)}
        /// "", ""{ nameof(Message.SentAt)}
        /// FROM {TempMessageTableName} ON CONFLICT(""{nameof(Message.Id)}"") DO NOTHING; ";
        /// </summary>
        public string InsertTempTableSqlStatement { get; set; } = default!;
        public string DropTempTableSqlStatement { get; set; } = default!;
    }
}
