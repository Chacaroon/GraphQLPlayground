namespace HotChocolatePlayground.Models;

public class Folder : Entity
{
    public Guid? ParentFolderId { get; set; }

    public Folder? ParentFolder { get; set; }
    
    public ICollection<Material> Materials { get; set; }
}