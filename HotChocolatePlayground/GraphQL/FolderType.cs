using HotChocolatePlayground.Models;

namespace HotChocolatePlayground.GraphQL;

public class FolderType : ObjectType<Folder>
{
    protected override void Configure(IObjectTypeDescriptor<Folder> descriptor)
    {
        descriptor.Name("Folder");

        descriptor.BindFieldsImplicitly();
    }
}