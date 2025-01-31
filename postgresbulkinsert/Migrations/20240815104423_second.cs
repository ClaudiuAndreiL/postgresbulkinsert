using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BulkInsertAPI.Migrations
{
    /// <inheritdoc />
    public partial class second : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Message_Originator_Recipient_IPAddress_MessageType_Characte~",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "AttachmentSize",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "AttachmentUrl",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "ConversationId",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "IPAddress",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "IsArchived",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "IsRead",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "MessageType",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "ReadAt",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "Source",
                table: "Message");

            migrationBuilder.RenameColumn(
                name: "Priority",
                table: "Message",
                newName: "MessagePartCount");

            migrationBuilder.CreateIndex(
                name: "IX_Message_Originator_Recipient_CharacterSet_MessagePartCount_~",
                table: "Message",
                columns: new[] { "Originator", "Recipient", "CharacterSet", "MessagePartCount", "SentAt" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Message_Originator_Recipient_CharacterSet_MessagePartCount_~",
                table: "Message");

            migrationBuilder.RenameColumn(
                name: "MessagePartCount",
                table: "Message",
                newName: "Priority");

            migrationBuilder.AddColumn<long>(
                name: "AttachmentSize",
                table: "Message",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "AttachmentUrl",
                table: "Message",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "ConversationId",
                table: "Message",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IPAddress",
                table: "Message",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsArchived",
                table: "Message",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Message",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsRead",
                table: "Message",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "MessageType",
                table: "Message",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "ReadAt",
                table: "Message",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Source",
                table: "Message",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Message_Originator_Recipient_IPAddress_MessageType_Characte~",
                table: "Message",
                columns: new[] { "Originator", "Recipient", "IPAddress", "MessageType", "CharacterSet", "SentAt" },
                unique: true);
        }
    }
}
