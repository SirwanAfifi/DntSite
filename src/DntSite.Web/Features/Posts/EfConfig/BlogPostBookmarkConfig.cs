using DntSite.Web.Features.Posts.Entities;

namespace DntSite.Web.Features.Posts.EfConfig;

public class BlogPostBookmarkConfig : IEntityTypeConfiguration<BlogPostBookmark>
{
    public void Configure(EntityTypeBuilder<BlogPostBookmark> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(bookmark => bookmark.Parent)
            .WithMany(entity => entity.Bookmarks)
            .HasForeignKey(bookmark => bookmark.ParentId)
            .IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.BlogPostBookmarks)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}
