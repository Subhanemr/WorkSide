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

        public DbSet<Category> Categories { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<Experience> Experiences { get; set; }
        public DbSet<Follow> Follows { get; set; }
        public DbSet<HireAccount> HireAccounts { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<JobComment> JobComments { get; set; }
        public DbSet<JobReply> JobReplies { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Portfolio> Portfolios { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectComment> ProjectComments { get; set; }
        public DbSet<ProjectLike> ProjectLikes { get; set; }
        public DbSet<ProjectReply> ProjectReplies { get; set; }
        public DbSet<Settings> Settings { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<WorkTime> WorkTimes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.FollowDbSettings();
            modelBuilder.JobCommentReplyDbSettings();
            modelBuilder.JobLikeDbSettings();
            modelBuilder.ProjectCommentReplyDbSettings();
            modelBuilder.ProjectLikeDbSettings();

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
