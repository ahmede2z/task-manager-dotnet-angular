using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TaskManager.Application.Abstractions;
using TaskManager.Domain.Common;

namespace TaskManager.Infrastructure.Persistence.Repositories;

public class Repository<TEntity>(ApplicationDbContext context) : IRepository<TEntity>
    where TEntity : BaseEntity
{
    protected readonly ApplicationDbContext Context = context;

    public async Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken) =>
        await Context.Set<TEntity>().FindAsync([id], cancellationToken);

    public async Task<IReadOnlyList<TEntity>> ListAsync(
        Expression<Func<TEntity, bool>>? predicate,
        CancellationToken cancellationToken)
    {
        IQueryable<TEntity> query = Context.Set<TEntity>().AsNoTracking();

        if (predicate is not null)
        {
            query = query.Where(predicate);
        }

        return await query.ToListAsync(cancellationToken);
    }

    public Task<bool> ExistsAsync(int id, CancellationToken cancellationToken) =>
        Context.Set<TEntity>().AnyAsync(e => e.Id == id, cancellationToken);

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken) =>
        await Context.Set<TEntity>().AddAsync(entity, cancellationToken);

    public void Remove(TEntity entity) => Context.Set<TEntity>().Remove(entity);
}
