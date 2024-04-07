using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Workwise.Domain.Entities;

namespace Workwise.Persistance.DAL
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        private readonly IHttpContextAccessor _http;

        public AppDbContext(DbContextOptions<AppDbContext> options, IHttpContextAccessor http) : base(options)
        {
            _http = http;
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
