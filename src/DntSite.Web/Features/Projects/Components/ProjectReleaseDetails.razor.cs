using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.AppConfigs.Services;
using DntSite.Web.Features.Projects.Entities;
using DntSite.Web.Features.Projects.Models;
using DntSite.Web.Features.Projects.RoutingConstants;
using DntSite.Web.Features.Projects.Services.Contracts;

namespace DntSite.Web.Features.Projects.Components;

public partial class ProjectReleaseDetails
{
    private ReleaseModel? _releases;

    [Parameter] public int? ProjectId { set; get; }

    [Parameter] public int? ReleaseId { set; get; }

    private string LastPostUrl
        => Invariant($"{ProjectsRoutingConstants.ProjectReleasesBase}/{ProjectId}/{_releases!.PreviousItem?.Id}");

    private string NextPostUrl
        => Invariant($"{ProjectsRoutingConstants.ProjectReleasesBase}/{ProjectId}/{_releases!.NextItem?.Id}");

    private string PostUrlTemplate
        => Invariant($"{ProjectsRoutingConstants.ProjectReleasesBase}/{ProjectId}/{ReleaseId}");

    private string EditPostUrlTemplate
        => Invariant($"{ProjectsRoutingConstants.WriteProjectReleaseEditBase}/{ProjectId}/{ReleaseId}");

    private string DeletePostUrlTemplate
        => Invariant($"{ProjectsRoutingConstants.WriteProjectReleaseDeleteBase}/{ProjectId}/{ReleaseId}");

    private ProjectRelease? CurrentPost => _releases?.CurrentItem;

    private DateTime? ModifiedAt => CurrentPost?.AuditActions.Count > 0
        ? CurrentPost?.AuditActions[^1].CreatedAt
        : CurrentPost?.Audit.CreatedAt;

    private string? CurrentPostImageUrl
        => HtmlHelperService.ExtractImagesLinks(CurrentPost?.FileDescription ?? "").FirstOrDefault();

    [InjectComponentScoped] internal IProjectReleasesService ProjectReleasesService { set; get; } = null!;

    [Inject] internal IHtmlHelperService HtmlHelperService { set; get; } = null!;

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    private bool CanUserDeleteThisPost => ApplicationState.CurrentUser?.IsAdmin == true;

    private bool CanUserEditThisPost
        => ApplicationState.CanCurrentUserEditThisItem(CurrentPost?.UserId, CurrentPost?.Audit.CreatedAt);

    private List<string> GetTags() => CurrentPost?.Tags.Select(x => x.Name).ToList() ?? [];

    protected override async Task OnInitializedAsync()
    {
        if (!ProjectId.HasValue || !ReleaseId.HasValue)
        {
            ApplicationState.NavigateToNotFoundPage();

            return;
        }

        _releases = await ProjectReleasesService.ShowReleaseAsync(ProjectId.Value, ReleaseId.Value);

        if (_releases.CurrentItem is null)
        {
            ApplicationState.NavigateToNotFoundPage();
        }

        AddBreadCrumbs(_releases.CurrentItem?.Project.Title ?? "");
    }

    private void AddBreadCrumbs(string name)
        => ApplicationState.BreadCrumbs.AddRange([..ProjectsBreadCrumbs.DefaultProjectBreadCrumbs(name, ProjectId)]);
}
