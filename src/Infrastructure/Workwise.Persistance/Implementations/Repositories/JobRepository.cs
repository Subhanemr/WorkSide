using Workwise.Application.Abstractions.Repositories;
using Workwise.Domain.Entities;
using Workwise.Persistance.DAL;
using Workwise.Persistance.Implementations.Repositories.Generic;

namespace Workwise.Persistance.Implementations.Repositories
{
    public class JobRepository : Repository<Job>, IJobRepository
    {
        public JobRepository(AppDbContext context) : base(context) { }
    }
}
