using HotChocolatePlayground.Models;

namespace HotChocolatePlayground.GraphQL;

public class DocumentType : ObjectType<Document>
{
    protected override void Configure(IObjectTypeDescriptor<Document> descriptor)
    {
        descriptor.Name("Document");

        descriptor.BindFieldsImplicitly();

        descriptor.Field("isFavorite")
            .Type<BooleanType>()
            .Resolve(async ctx =>
            {
                var result = await ctx.Services.GetRequiredService<IsFavoriteEntityDataLoader>()
                    .LoadAsync(ctx.Parent<Entity>().Id, ctx.RequestAborted);

                return result != null;
            });
    }
}