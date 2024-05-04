using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CaptureIt.Migrations
{
    /// <inheritdoc />
    public partial class First : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddPrimaryKey(
            name: "PK_EventParticipants",
            table: "EventParticipants",
            columns: new[] { "EventId", "ParticipantId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddPrimaryKey(
        name: "PK_EventParticipants",
        table: "EventParticipants",
        columns: new[] { "EventId", "ParticipantId" });
        }
    }
}
