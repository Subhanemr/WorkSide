using Workwise.Application.Abstractions.Repositories;
using Workwise.Application.Abstractions.Services;

namespace Workwise.Persistance.Implementations.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _repository;

        public ProjectService(IProjectRepository repository)
        {
            _repository = repository;
        }


    }
}
