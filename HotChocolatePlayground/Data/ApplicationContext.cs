using HotChocolatePlayground.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging.Console;

namespace HotChocolatePlayground.Data;

public class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.ConfigureWarnings(x => x.Log(
            (RelationalEventId.ConnectionOpened, LogLevel.Warning),
            (RelationalEventId.ConnectionClosed, LogLevel.Warning)));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Document>()
            .HasData(Enumerable.Range(0, 900).Select(x => new Document { Id = x + 1, Name = $"Document {x + 1}", MaterialId = x / 3 + 1 }));

        modelBuilder.Entity<Material>()
            .HasData(Enumerable.Range(0, 300).Select(x => new Material { Id = x + 1, Name = $"Material {x + 1}", FolderId = x / 3 + 1 }));

        modelBuilder.Entity<Folder>()
            .HasData(Enumerable.Range(1, 100).Select(x => new Folder { Id = x, ParentFolderId = 101 }).Concat(new[] { new Folder { Id = 101 }}));

        modelBuilder.Entity<User>()
            .HasData(_users);

        modelBuilder.Entity<IsFavoriteEntity>()
            .HasKey(x => new { x.UserId, x.EntityId, x.EntityType });

        modelBuilder.Entity<IsFavoriteEntity>()
            .HasData(CreateIsFavoriteEntityRecords());
    }

    private IsFavoriteEntity[] CreateIsFavoriteEntityRecords()
    {
        var folderIds = Enumerable.Range(1, 100).ToArray();

        var folders = folderIds
            .Take(50)
            .Select(x => new IsFavoriteEntity { EntityId = x, UserId = 1, EntityType = EntityType.Folder })
            .Concat(folderIds
                .Skip(50).Select(x => new IsFavoriteEntity { EntityId = x, UserId = 2, EntityType = EntityType.Folder }));
        
        var documentIds = Enumerable.Range(1, 600).ToArray();
        var documents = documentIds
            .Take(300)
            .Select(x => new IsFavoriteEntity { EntityId = x, UserId = 1, EntityType = EntityType.Document })
            .Concat(documentIds
                .Skip(300).Select(x => new IsFavoriteEntity { EntityId = x, UserId = 2, EntityType = EntityType.Document }));

        return documents.Concat(folders).ToArray();
    }

    private readonly User[] _users =
    {
        new()
        {
            Id = 1
        },
        new()
        {
            Id = 2
        }
    };
}