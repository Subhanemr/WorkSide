using Microsoft.EntityFrameworkCore;
using System.Drawing;
using Workwise.Domain.Entities;

namespace Workwise.Persistance.Common
{
    internal static class FollowDbSetting
    {
        public static void FollowDbSettings(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Follow>()
            .HasKey(f => new { f.FollowerId, f.FollowingId });

            modelBuilder.Entity<Follow>()
                .HasOne(f => f.Follower)
                .WithMany(u => u.Followings)
                .HasForeignKey(f => f.FollowerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Follow>()
                .HasOne(f => f.Following)
                .WithMany(u => u.Followers)
                .HasForeignKey(f => f.FollowingId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
