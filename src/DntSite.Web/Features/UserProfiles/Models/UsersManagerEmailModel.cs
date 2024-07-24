using DntSite.Web.Features.Common.Models;

namespace DntSite.Web.Features.UserProfiles.Models;

public class UsersManagerEmailModel : BaseEmailModel
{
    public required string Username { get; set; }

    public required string FriendlyName { get; set; }

    public required string Operation { get; set; }
}