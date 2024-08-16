using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BulkInsertAPI.Data.Models
{
    [Table(nameof(Message))]
    public class Message
    {
        [Key]
        public Guid Id { get; set; } // Primary Key, generated as a Guid

        public string Originator { get; set; } = default!; // Identifier of the sender (Guid)
        public string Recipient { get; set; } = default!; // Identifier of the receiver (Guid)

        public string CharacterSet { get; set; } = default!; // Subject of the message
        public string Body { get; set; } = default!; // The main content of the message
        public int MessagePartCount { get; set; }
        public DateTime SentAt { get; set; } // Timestamp when the message was sent    
    }
}
