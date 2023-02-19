using HotChocolatePlayground.Models;

namespace HotChocolatePlayground.GraphQL;

public class DocumentType : ObjectType<Document>
{
    protected override void Configure(IObjectTypeDescriptor<Document> descriptor)
    {
        descriptor.Name("Document");

        descriptor.BindFieldsImplicitly();
    }
}