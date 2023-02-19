namespace HotChocolatePlayground.Models;

public class IsFavoriteEntity
{
    public int UserId { get; set; }
    
    public int EntityId { get; set; }

    public EntityType EntityType { get; set; }
}

public enum EntityType
{
    Folder = 1,
    Document = 2
}