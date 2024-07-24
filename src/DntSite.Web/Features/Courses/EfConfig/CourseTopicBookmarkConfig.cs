using DntSite.Web.Features.Courses.Entities;

namespace DntSite.Web.Features.Courses.EfConfig;

public class CourseTopicBookmarkConfig : IEntityTypeConfiguration<CourseTopicBookmark>
{
    public void Configure(EntityTypeBuilder<CourseTopicBookmark> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(bookmark => bookmark.Parent)
            .WithMany(entity => entity.Bookmarks)
            .HasForeignKey(bookmark => bookmark.ParentId)
            .IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.CourseTopicBookmarks)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}
