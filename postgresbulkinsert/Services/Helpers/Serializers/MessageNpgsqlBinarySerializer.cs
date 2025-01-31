//using BulkInsertAPI.Data.Models;
//using Npgsql;

//namespace BulkInsertAPI.Services.Helpers.Serializers;

// TODO: uncomment if the generated one does not work
//public class MessageNpgsqlBinarySerializer : INpgsqlEntityBinarySerializer<Message>
//{
//    public async Task BulkInsertBinaryAsync(NpgsqlBinaryImporter writer, NpgsqlConnection connection, Message message)
//    {
//        await writer.StartRowAsync();

//        await writer.WriteAsync(message.MessageId, NpgsqlTypes.NpgsqlDbType.Uuid);
//        await writer.WriteAsync(message.RequestId, NpgsqlTypes.NpgsqlDbType.Uuid);
//        await writer.WriteAsync(message.ReferenceMessageId, NpgsqlTypes.NpgsqlDbType.Uuid);
//        await writer.WriteAsync(message.Reason, NpgsqlTypes.NpgsqlDbType.Citext);
//        await writer.WriteAsync(message.Status, NpgsqlTypes.NpgsqlDbType.Citext);
//        await writer.WriteAsync(message.IsBillable, NpgsqlTypes.NpgsqlDbType.Boolean);
//        await writer.WriteAsync(message.Msisdn, NpgsqlTypes.NpgsqlDbType.Text);
//        await writer.WriteAsync(message.NormalizedMsisdn, NpgsqlTypes.NpgsqlDbType.Text);
//        await writer.WriteAsync(message.DestinationCode, NpgsqlTypes.NpgsqlDbType.Text);
//        await writer.WriteAsync(message.Originator, NpgsqlTypes.NpgsqlDbType.Citext);
//        await writer.WriteAsync(message.SenderType, NpgsqlTypes.NpgsqlDbType.Citext);
//        await writer.WriteAsync(message.IndustrySector, NpgsqlTypes.NpgsqlDbType.Citext);
//        await writer.WriteAsync(message.CharacterSet, NpgsqlTypes.NpgsqlDbType.Citext);
//        await writer.WriteAsync(message.MessagePartCount, NpgsqlTypes.NpgsqlDbType.Integer);
//        await writer.WriteAsync(message.SentMessagePartCount, NpgsqlTypes.NpgsqlDbType.Integer);
//        await writer.WriteAsync(message.FailedMessagePartCount, NpgsqlTypes.NpgsqlDbType.Integer);
//        await writer.WriteAsync(message.Variables, NpgsqlTypes.NpgsqlDbType.Citext);
//        await writer.WriteAsync(message.NetworkUsed, NpgsqlTypes.NpgsqlDbType.Citext);
//        await writer.WriteAsync(message.IsPiiDeleted, NpgsqlTypes.NpgsqlDbType.Boolean);
//        await writer.WriteAsync(message.IsDeleted, NpgsqlTypes.NpgsqlDbType.Boolean);
//        await writer.WriteAsync(message.IsDeletedBy, NpgsqlTypes.NpgsqlDbType.Text);
//        await writer.WriteAsync(message.SentAt, NpgsqlTypes.NpgsqlDbType.TimestampTz);
//        await writer.WriteAsync(message.CreateTimestamp, NpgsqlTypes.NpgsqlDbType.TimestampTz);
//        await writer.WriteAsync(message.LastUpdateTimestamp, NpgsqlTypes.NpgsqlDbType.TimestampTz);
//        await writer.WriteAsync(message.Direction, NpgsqlTypes.NpgsqlDbType.Text);
//        await writer.WriteAsync(message.ConversationId, NpgsqlTypes.NpgsqlDbType.Uuid);
//        await writer.WriteAsync(message.IsLandline, NpgsqlTypes.NpgsqlDbType.Boolean);
//        await writer.WriteAsync(message.IsRead, NpgsqlTypes.NpgsqlDbType.Boolean);
//        await writer.WriteAsync(message.ReadBy, NpgsqlTypes.NpgsqlDbType.Uuid);
//    }
//}
