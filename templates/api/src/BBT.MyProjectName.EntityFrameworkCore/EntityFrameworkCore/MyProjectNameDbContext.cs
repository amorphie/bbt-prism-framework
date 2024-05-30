using System;
using BBT.MyProjectName.Issues;
using BBT.Prism.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BBT.MyProjectName.EntityFrameworkCore;

public class MyProjectNameDbContext(
    IServiceProvider serviceProvider,
    DbContextOptions<MyProjectNameDbContext> options
)
    : PrismDbContext<MyProjectNameDbContext>(serviceProvider, options)
{
    public virtual DbSet<GitRepository> GitRepositories { get; set; }
    public virtual DbSet<Issue> Issues { get; set; }
    public virtual DbSet<Comment> Comments { get; set; }
    public virtual DbSet<Label> Labels { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureMyProjectName();
    }
}