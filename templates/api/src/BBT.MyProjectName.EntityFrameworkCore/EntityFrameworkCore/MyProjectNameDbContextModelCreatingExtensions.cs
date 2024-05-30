using BBT.MyProjectName.Issues;
using BBT.Prism.EntityFrameworkCore.Modeling;
using Microsoft.EntityFrameworkCore;

namespace BBT.MyProjectName.EntityFrameworkCore;

public static class MyProjectNameDbContextModelCreatingExtensions
{
    public static void ConfigureMyProjectName(
        this ModelBuilder builder)
    {
        /* Configure all entities here. */

        builder.Entity<GitRepository>(b =>
        {
            b.ToTable("Repositories");
            b.ConfigureByConvention();
            b.Property(p => p.Name).IsRequired().HasMaxLength(GitRepositoryConsts.MaxNameLength);

            b.HasMany<Issue>()
                .WithOne()
                .HasForeignKey(p => p.RepositoryId);
        });

        builder.Entity<Issue>(b =>
        {
            b.ToTable("Issues");
            b.ConfigureByConvention();

            b.Property(p => p.Title).IsRequired().HasMaxLength(IssueConsts.MaxTitleLength);
            b.Property(p => p.Text).HasMaxLength(IssueConsts.MaxTextLength);

            b.HasMany(p => p.Comments)
                .WithOne()
                .HasForeignKey(p => p.IssueId);

            b.OwnsMany(o => o.Labels,
                ilBuilder =>
                {
                    ilBuilder.ToTable("IssueLabels");
                    ilBuilder.HasKey(p => new { p.IssueId, p.LabelId });
                    ilBuilder.WithOwner()
                        .HasForeignKey(p => p.IssueId);
                    ilBuilder.HasOne<Label>()
                        .WithMany()
                        .HasForeignKey(p => p.LabelId);
                });
        });

        builder.Entity<Label>(b =>
        {
            b.ToTable("Labels");
            b.ConfigureByConvention();
            b.Property(p => p.Name).IsRequired().HasMaxLength(LabelConsts.MaxNameLength);
        });

        builder.Entity<Comment>(b =>
        {
            b.ToTable("Comments");
            b.ConfigureByConvention();

            // b.Property(p => p.Id).ValueGeneratedNever();

            b.Property(p => p.Text).IsRequired().HasMaxLength(CommentConsts.MaxTextLength);
            
            b.HasOne<Issue>()
                .WithMany(p => p.Comments)
                .HasForeignKey(p => p.IssueId);
        });
    }
}