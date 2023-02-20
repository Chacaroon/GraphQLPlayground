using HotChocolatePlayground.Data;
using HotChocolatePlayground.Models;
using Microsoft.EntityFrameworkCore;

namespace HotChocolatePlayground.GraphQL;

public class FolderType : ObjectType<Folder>
{
    protected override void Configure(IObjectTypeDescriptor<Folder> descriptor)
    {
        descriptor.Name("Folder");

        descriptor.BindFieldsImplicitly();
        
        descriptor.Field(x => x.Materials)
            .Type<ListType<MaterialType>>();

        descriptor.Field(x => x.ParentFolder)
            .Type<FolderType>();
        
        descriptor.Field("isFavorite")
            .Type<BooleanType>()
            .Resolve(async ctx =>
            {
                var result = await ctx.Services.GetRequiredService<IsFavoriteEntityDataLoader>()
                    .LoadAsync(ctx.Parent<Entity>().Id, ctx.RequestAborted);

                return result != null;
            });

        descriptor.Field("subFolders")
            .Type<ListType<FolderType>>()
            .Resolve(async ctx =>
            {
                var result = await ctx.BatchDataLoader<Guid, Folder[]>(async (keys, ct) =>
                    {
                        await using var dbContext = await ctx.Services
                            .GetRequiredService<IDbContextFactory<ApplicationContext>>()
                            .CreateDbContextAsync(ct);

                        var entities = await dbContext.Set<Folder>()
                            .Where(x => x.ParentFolderId != null && keys.Contains(x.ParentFolderId!.Value))
                            .ToListAsync(ct);
 
                        return entities.GroupBy(x => x.ParentFolderId!.Value)
                            .ToDictionary(x => x.Key, x => x.ToArray());
                    })
                    .LoadAsync(ctx.Parent<Entity>().Id);
                
                return result;
            });
    }
}