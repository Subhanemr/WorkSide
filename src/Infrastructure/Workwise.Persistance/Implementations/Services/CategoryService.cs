using Workwise.Application.Abstractions.Repositories;
using Workwise.Application.Abstractions.Services;

namespace Workwise.Persistance.Implementations.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;

        public CategoryService(ICategoryRepository repository)
        {
            _repository = repository;
        }


    }
}
