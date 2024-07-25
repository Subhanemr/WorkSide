using Microsoft.EntityFrameworkCore;
using Workwise.Domain.Entities;

namespace Workwise.Persistance.Common
{
    internal static class LikeDbSetting
    {
        public static void JobLikeDbSettings(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<JobLike>()
                 .HasOne(l => l.AppUser)
                 .WithMany(u => u.JobLikes)
                 .HasForeignKey(l => l.AppUserId)
                 .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<JobLike>()
                .HasOne(l => l.Jobs)
                .WithMany(p => p.JobLikes)
                .HasForeignKey(l => l.JobId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        public static void ProjectLikeDbSettings(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProjectLike>()
                 .HasOne(l => l.AppUser)
                 .WithMany(u => u.ProjectLikes)
                 .HasForeignKey(l => l.AppUserId)
                 .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProjectLike>()
                .HasOne(l => l.Project)
                .WithMany(p => p.ProjectLikes)
                .HasForeignKey(l => l.ProjectId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
