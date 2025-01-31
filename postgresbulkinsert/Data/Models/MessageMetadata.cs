using System.ComponentModel.DataAnnotations;

namespace BulkInsertAPI.Data.Models
{
    public class MessageMetadata
    {
        [Key]
        public Guid Id { get; set; }
        public Guid MessageId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
