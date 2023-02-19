using HotChocolatePlayground.Models;

namespace HotChocolatePlayground.GraphQL;

public class MaterialType : ObjectType<Material>
{
    protected override void Configure(IObjectTypeDescriptor<Material> descriptor)
    {
        descriptor.Name("Material");

        descriptor.BindFieldsImplicitly();
    }
}