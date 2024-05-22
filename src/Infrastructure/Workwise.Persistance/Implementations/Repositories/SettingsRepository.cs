using Workwise.Application.Abstractions.Repositories;
using Workwise.Domain.Entities;
using Workwise.Persistance.DAL;
using Workwise.Persistance.Implementations.Repositories.Generic;

namespace Workwise.Persistance.Implementations.Repositories
{
    public class SettingsRepository : Repository<Settings>, ISettingsRepository
    {
        public SettingsRepository(AppDbContext context) : base(context) { }

    }
}
