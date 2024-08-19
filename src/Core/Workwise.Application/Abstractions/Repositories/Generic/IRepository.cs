using System.Linq.Expressions;
using Workwise.Domain.Entities;

namespace Workwise.Application.Abstractions.Repositories.Generic
{
    public interface IRepository<T> where T : BaseEntity, new()
    {
        IQueryable<T> GetAll(bool isTracking = true,
            params string[] includes);

        IQueryable<T> GetAllWhere(Expression<Func<T, bool>>? expression = null,
            int skip = 0,
            int take = 0,
            bool isTracking = true,
            params string[] includes);

        IQueryable<T> GetAllWhereByOrder(Expression<Func<T, bool>>? expression = null,
            Expression<Func<T, object>>? orderException = null,
            bool isDescending = false,
            bool isDeleted = false,
            int skip = 0,
            int take = 0,
            bool isTracking = true,
            params string[] includes);

        Task<T> GetByIdAsync(string id,
            bool isTracking = true,
            params string[] includes);

        Task<T> GetByExpressionAsync(Expression<Func<T, bool>> expression,
            bool isTracking = true,
            params string[] includes);

        Task<double> CountAsync(Expression<Func<T, bool>>? expression = null, bool isDeleted = false);

        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        void SoftDelete(T entity);
        void ReverseSoftDelete(T entity);
        Task SaveChangeAsync();
        Task<bool> CheckUniqueAsync(Expression<Func<T, bool>> expression);
    }
}
