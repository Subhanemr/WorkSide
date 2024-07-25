using Microsoft.EntityFrameworkCore;
using Workwise.Domain.Entities;

namespace Workwise.Persistance.Common
{
    internal static class CommentReplyDbSetting
    {
        public static void JobCommentReplyDbSettings(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<JobComment>()
            .HasOne(c => c.Job)
            .WithMany(p => p.JobComments)
            .HasForeignKey(c => c.JobId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<JobComment>()
                .HasOne(c => c.AppUser)
                .WithMany(u => u.JobComments)
                .HasForeignKey(c => c.AppUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<JobReply>()
                .HasOne(r => r.JobComment)
                .WithMany(c => c.JobReplies)
                .HasForeignKey(r => r.JobCommentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<JobReply>()
                .HasOne(r => r.AppUser)
                .WithMany(u => u.JobReplies)
                .HasForeignKey(r => r.AppUserId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        public static void ProjectCommentReplyDbSettings(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProjectComment>()
            .HasOne(c => c.Project)
            .WithMany(p => p.ProjectComments)
            .HasForeignKey(c => c.ProjectId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProjectComment>()
                .HasOne(c => c.AppUser)
                .WithMany(u => u.ProjectComments)
                .HasForeignKey(c => c.AppUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProjectReply>()
                .HasOne(r => r.ProjectComment)
                .WithMany(c => c.ProjectReplies)
                .HasForeignKey(r => r.ProjectCommentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProjectReply>()
                .HasOne(r => r.AppUser)
                .WithMany(u => u.ProjectReplies)
                .HasForeignKey(r => r.AppUserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
