using DntSite.Web.Features.AppConfigs.Components;

namespace DntSite.Web.Features.Layout;

public partial class MainLayout
{
    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    private IDictionary<string, object?> FooterMenuParameters => new Dictionary<string, object?>
    {
        {
            nameof(FooterMenu.SiteName), ApplicationState.AppSetting?.BlogName
        }
    };
}
