using DntSite.Web.Features.Common.Models;

namespace DntSite.Web.Features.UserProfiles.Models;

public class DisabledModel : BaseEmailModel
{
    public required string BaseUrl { get; set; }

    public required string SiteName { get; set; }
}