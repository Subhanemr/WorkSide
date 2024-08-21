using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Workwise.Domain.Entities;
using Workwise.Persistance.Common;

namespace Workwise.Persistance.DAL
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        private readonly IHttpContextAccessor _http;

        public AppDbContext(DbContextOptions<AppDbContext> options, IHttpContextAccessor http) : base(options)
        {
            _http = http;
        }

        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Education> Educations { get; set; } = null!;
        public DbSet<Experience> Experiences { get; set; } = null!;
        public DbSet<Follow> Follows { get; set; } = null!;
        public DbSet<HireAccount> HireAccounts { get; set; } = null!;
        public DbSet<Job> Jobs { get; set; } = null!;
        public DbSet<JobComment> JobComments { get; set; } = null!;
        public DbSet<JobReply> JobReplies { get; set; } = null!;
        public DbSet<Location> Locations { get; set; } = null!;
        public DbSet<Portfolio> Portfolios { get; set; } = null!;
        public DbSet<Project> Projects { get; set; } = null!;
        public DbSet<ProjectComment> ProjectComments { get; set; } = null!;
        public DbSet<ProjectLike> ProjectLikes { get; set; } = null!;
        public DbSet<ProjectReply> ProjectReplies { get; set; } = null!;
        public DbSet<Settings> Settings { get; set; } = null!;
        public DbSet<Skill> Skills { get; set; } = null!;
        public DbSet<WorkTime> WorkTimes { get; set; } = null!;
        public DbSet<Chat> Chats { get; set; } = null!;
        public DbSet<Message> Messages { get; set; } = null!;
        public DbSet<Notification> Notifications { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.FollowDbSettings();
            modelBuilder.JobCommentReplyDbSettings();
            modelBuilder.JobLikeDbSettings();
            modelBuilder.ProjectCommentReplyDbSettings();
            modelBuilder.ProjectLikeDbSettings();
            modelBuilder.ChatDbSetting();

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entities = ChangeTracker.Entries<BaseEntity>();
            foreach (var data in entities)
            {
                switch (data.State)
                {
                    case EntityState.Added:
                        data.Entity.Id = Guid.NewGuid().ToString();
                        data.Entity.CreateAt = DateTime.Now;
                        data.Entity.CreatedBy = _http.HttpContext.User.Identity.Name;
                        break;
                    case EntityState.Modified:
                        data.Entity.UpdateAt = DateTime.Now;
                        data.Entity.CreatedBy = _http.HttpContext.User.Identity.Name;
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
