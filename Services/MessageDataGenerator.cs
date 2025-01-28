using Bogus;
using BulkInsertAPI.Data.Models;

namespace BulkInsertAPI.Services
{
    public static class MessageDataGenerator
    {
        public static List<Message> GetMessages(int no)
        {
            var faker = new Faker<Message>();

            var messages = faker
                .RuleFor(x => x.MessageId, v => v.Random.Guid())
                .RuleFor(x => x.RequestId, v => v.Random.Guid())
                .RuleFor(x => x.Reason, v => v.Random.Word())
                .RuleFor(x => x.Status, v => v.Random.Word())
                .RuleFor(x => x.Msisdn, v => v.Random.Number(10000000).ToString())
                .RuleFor(x => x.NormalizedMsisdn, v => v.Random.Number(10000000).ToString())
                .RuleFor(x => x.DestinationCode, v => v.Address.CountryCode())
                .RuleFor(x => x.MessagePartCount, 1)
                .RuleFor(x => x.SentMessagePartCount, 1)
                .RuleFor(x => x.FailedMessagePartCount, 0)
                .RuleFor(x => x.Originator, v => v.Random.Word())
                .RuleFor(x => x.SenderType, v => v.Random.Word())
                .RuleFor(x => x.IndustrySector, v => v.Random.Word())
                .RuleFor(x => x.Variables, v => v.Random.Word())
                .RuleFor(x => x.NetworkUsed, v => v.Random.Word())
                .RuleFor(x => x.SentAt, v => DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc))
                .RuleFor(x => x.CreateTimestamp, v => DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc))
                .RuleFor(x => x.LastUpdateTimestamp, v => DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc))
                .RuleFor(x => x.IsBillable, v => true)
                .RuleFor(x => x.Direction, "Outbound")
                .RuleFor(x => x.CharacterSet, v => v.Random.Word())
                .RuleFor(x => x.IsRead, v => true)
                .RuleFor(x => x.ReadBy, v => v.Random.Guid())
                .Generate(no);

            return messages;
        }
    }
}
