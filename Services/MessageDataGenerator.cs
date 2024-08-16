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
                .RuleFor(x => x.Id, v => v.Random.Guid())
                .RuleFor(x => x.Originator, v => v.Random.Word())
                .RuleFor(x => x.Recipient, v => v.Random.Word())
                .RuleFor(x => x.CharacterSet, v => v.Random.Word())
                .RuleFor(x => x.Body, v => v.Lorem.Paragraph())
                .RuleFor(x => x.MessagePartCount, v=> v.Random.Int())
                .RuleFor(x => x.SentAt, v => DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified))
                .Generate(no);

            return messages;
        }
    }
}
