using DntSite.Web.Features.StackExchangeQuestions.Entities;

namespace DntSite.Web.Features.StackExchangeQuestions.EfConfig;

public class StackExchangeQuestionBookmarkConfig : IEntityTypeConfiguration<StackExchangeQuestionBookmark>
{
    public void Configure(EntityTypeBuilder<StackExchangeQuestionBookmark> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(bookmark => bookmark.Parent)
            .WithMany(entity => entity.Bookmarks)
            .HasForeignKey(bookmark => bookmark.ParentId)
            .IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.StackExchangeQuestionBookmarks)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}