using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common;

namespace BulkInsertAPI.Data.Models;

[GenerateSerializer]
public class Message
{
    [Key]
    public Guid MessageId { get; set; }
    public Guid RequestId { get; set; }

    /// <summary>
    /// will store RTLS found message id for Inbound messages
    /// </summary>
    public Guid? ReferenceMessageId { get; set; }

    [Column(TypeName = "citext")]
    public string Reason { get; set; } = string.Empty;
    [Column(TypeName = "citext")]
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// specifies that the message should be considered in the counts send to billing
    /// does not necessary that it will be billed, but it should be considered by billing
    /// </summary>
    public bool IsBillable { get; set; }

    [StringLength(32)]
    public string Msisdn { get; set; } = string.Empty;
    [StringLength(32)]
    public string NormalizedMsisdn { get; set; } = string.Empty;
    [StringLength(2)]
    public string DestinationCode { get; set; } = string.Empty;
    [Column(TypeName = "citext")]
    public string Originator { get; set; } = string.Empty;
    [Column(TypeName = "citext")]
    public string SenderType { get; set; } = string.Empty;
    [Column(TypeName = "citext")]
    public string? IndustrySector { get; set; } 
    [Column(TypeName = "citext")]
    public string CharacterSet { get; set; } = string.Empty;

    public int MessagePartCount { get; set; }
    public int SentMessagePartCount { get; set; }
    public int FailedMessagePartCount { get; set; }

    [Column(TypeName = "citext")]
    public string? Variables { get; set; }

    /// <summary>
    /// currently, for Inbound message this seems to be a "route"
    /// </summary>
    [Column(TypeName = "citext")]
    public string? NetworkUsed { get; set; }

    public bool IsPiiDeleted { get; set; }
    public bool IsDeleted { get; set; }

    [Column(TypeName = "text")]
    public string? IsDeletedBy { get; set; }

    public DateTime? SentAt { get; set; }
    public DateTime CreateTimestamp { get; set; }
    public DateTime LastUpdateTimestamp { get; set; }

    public long MessageOrderNo { get; set; }

    [StringLength(16)]
    public string Direction { get; set; } = default!; // will set Outbound as SQL default

    public Guid? ConversationId { get; set; }

    public bool IsLandline { get; set; }

    public bool? IsRead { get; set; }

    public Guid? ReadBy { get; set; }
}
