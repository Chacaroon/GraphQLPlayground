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
            .Resolve((context, _) =>
            {
                var dbContext = context.Services.GetRequiredService<ApplicationContext>();

                return dbContext.Set<Folder>().AsQueryable().AsSplitQuery();
            });
        
    }
}
