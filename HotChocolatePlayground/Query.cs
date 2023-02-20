using HotChocolatePlayground.Data;
using HotChocolatePlayground.GraphQL;
using HotChocolatePlayground.Models;
using Microsoft.EntityFrameworkCore;

namespace HotChocolatePlayground;

public class Query : ObjectType
{
    
    protected override void Configure(IObjectTypeDescriptor descriptor)
    {
        descriptor.Name(OperationTypeNames.Query);

        descriptor.Field("folder")
            .Type<ListType<FolderType>>()
            .UseProjection()
            .Resolve(async (context, ct) =>
            {
                await using var dbContext = await context.Services.GetRequiredService<IDbContextFactory<ApplicationContext>>().CreateDbContextAsync(ct);

                return dbContext.Set<Folder>().AsSplitQuery();
            });
        
    }
}
