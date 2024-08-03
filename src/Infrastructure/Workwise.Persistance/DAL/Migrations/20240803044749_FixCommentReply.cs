using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Workwise.Persistance.DAL.Migrations
{
    public partial class FixCommentReply : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReplyComment",
                table: "ProjectReplies",
                newName: "Comment");

            migrationBuilder.RenameColumn(
                name: "ReplyComment",
                table: "JobReplies",
                newName: "Comment");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Comment",
                table: "ProjectReplies",
                newName: "ReplyComment");

            migrationBuilder.RenameColumn(
                name: "Comment",
                table: "JobReplies",
                newName: "ReplyComment");
        }
    }
}
