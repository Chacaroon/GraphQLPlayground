using HotChocolatePlayground.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace HotChocolatePlayground.Data;

public class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var rootFolder = new Folder { Id = Guid.NewGuid() };
        modelBuilder.Entity<Folder>()
            .HasData(rootFolder);

        var folders = Enumerable.Range(1, 100)
            .Select(_ => new Folder { Id = Guid.NewGuid(), ParentFolderId = rootFolder.Id })
            .ToArray();
        modelBuilder.Entity<Folder>()
            .HasData(folders);

        var materials = Enumerable.Range(0, 300)
            .Select(x => new Material { Id = Guid.NewGuid(), Name = $"Material {x + 1}", FolderId = folders[x / 3].Id })
            .ToArray();
        modelBuilder.Entity<Material>().HasData(materials);

        var documents = Enumerable.Range(0, 900)
            .Select(x => new Document { Id = Guid.NewGuid(), Name = $"Document {x + 1}", MaterialId = materials[x / 3].Id })
            .ToArray();
        modelBuilder.Entity<Document>()
            .HasData(documents);

        modelBuilder.Entity<User>()
            .HasData(_users);

        modelBuilder.Entity<IsFavoriteEntity>()
            .HasKey(x => new { x.UserId, x.EntityId });

        modelBuilder.Entity<IsFavoriteEntity>()
            .HasData(CreateIsFavoriteEntityRecords(folders.Select(x => x.Id).ToArray(), documents.Select(x => x.Id).ToArray()));
    }

    private IsFavoriteEntity[] CreateIsFavoriteEntityRecords(Guid[] folderIds, Guid[] documentIds)
    {
        var folders = folderIds
            .Take(50)
            .Select(x => new IsFavoriteEntity { EntityId = x, UserId = _users[0].Id })
            .Concat(folderIds
                .Skip(50).Select(x => new IsFavoriteEntity { EntityId = x, UserId = _users[1].Id }));

        var documents = documentIds
            .Take(300)
            .Select(x => new IsFavoriteEntity { EntityId = x, UserId = _users[0].Id })
            .Concat(documentIds
                .Skip(300).Take(300).Select(x => new IsFavoriteEntity { EntityId = x, UserId = _users[1].Id }));

        return documents.Concat(folders).ToArray();
    }

    private readonly User[] _users =
    {
        new()
        {
            Id = Guid.NewGuid()
        },
        new()
        {
            Id = Guid.NewGuid()
        }
    };
}