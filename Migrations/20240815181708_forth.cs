﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BulkInsertAPI.Migrations
{
    /// <inheritdoc />
    public partial class forth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Message_Originator_Recipient_CharacterSet_MessagePartCount_~",
                table: "Message");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Message_Originator_Recipient_CharacterSet_MessagePartCount_~",
                table: "Message",
                columns: new[] { "Originator", "Recipient", "CharacterSet", "MessagePartCount", "SentAt" },
                unique: true);
        }
    }
}
