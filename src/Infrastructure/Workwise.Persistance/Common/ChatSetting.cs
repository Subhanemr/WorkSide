using Microsoft.EntityFrameworkCore;
using Workwise.Domain.Entities;

namespace Workwise.Persistance.Common
{
    internal static class ChatSetting
    {
        public static void ChatDbSetting(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Chat>()
                .HasOne(c => c.AppUser1)
                .WithMany(u => u.Chats)
                .HasForeignKey(c => c.AppUser1Id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Chat>()
                .HasOne(c => c.AppUser2)
                .WithMany()
                .HasForeignKey(c => c.AppUser2Id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Chat>()
                .HasMany(c => c.Messages)
                .WithOne(m => m.Chat)
                .HasForeignKey(m => m.ChatId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
