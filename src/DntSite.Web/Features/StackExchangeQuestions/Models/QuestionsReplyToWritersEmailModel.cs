using DntSite.Web.Features.Common.Models;

namespace DntSite.Web.Features.StackExchangeQuestions.Models;

public class QuestionsReplyToWritersEmailModel : BaseEmailModel
{
    public required string Title { get; set; }

    public required string Username { get; set; }

    public required string Body { get; set; }

    public required string PmId { get; set; }

    public required string CommentId { get; set; }
}
