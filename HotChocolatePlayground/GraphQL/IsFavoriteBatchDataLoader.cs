using HotChocolatePlayground.Data;
using HotChocolatePlayground.Models;
using Microsoft.EntityFrameworkCore;

namespace HotChocolatePlayground.GraphQL;

public class IsFavoriteEntityDataLoader : BatchDataLoader<Guid, IsFavoriteEntity>
{
    private readonly IDbContextFactory<ApplicationContext> _dbContextFactory;

    public IsFavoriteEntityDataLoader(
        IDbContextFactory<ApplicationContext> dbContextFactoryFactory,
        IBatchScheduler batchScheduler,
        DataLoaderOptions options)
        : base(batchScheduler, options)
    {
        _dbContextFactory = dbContextFactoryFactory;
    }

    protected override async Task<IReadOnlyDictionary<Guid, IsFavoriteEntity>> LoadBatchAsync(
        IReadOnlyList<Guid> keys,
        CancellationToken cancellationToken)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
        
        var entities = await dbContext.Set<IsFavoriteEntity>()
            .Where(e => keys.Contains(e.EntityId))
            .ToListAsync(cancellationToken);

        return entities.ToDictionary(e => e.EntityId);
    }
}