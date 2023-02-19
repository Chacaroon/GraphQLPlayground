namespace HotChocolatePlayground.Models;

public class Material : Entity
{
    public string Name { get; set; }

    public ICollection<Document> Documents { get; set; }

    public int FolderId { get; set; }
}