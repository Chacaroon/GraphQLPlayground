namespace HotChocolatePlayground.Models;

public class Folder : Entity
{
    public int? ParentFolderId { get; set; }

    public Folder? ParentFolder { get; set; }
    
    public ICollection<Material> Materials { get; set; }
}