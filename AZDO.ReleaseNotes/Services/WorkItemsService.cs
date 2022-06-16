using AZDO.ReleaseNotes.Models;
using AZDO.ReleaseNotes.Requests;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AZDO.ReleaseNotes.Services;

public class WorkItemsService
{
    public WorkItemsService()
    {

    }

    public async Task<IEnumerable<WorkItemModel>> GetByIds(RetrieveWorkItemsRequest request)
    {
        using var vssConnection = new VssConnection(
            new Uri(Settings.OrganizationUri),
            new VssBasicCredential(string.Empty, Settings.PersonalAccessToken));
        using var witClient = vssConnection.GetClient<WorkItemTrackingHttpClient>();
        var workItems = await witClient.GetWorkItemsAsync(request.Ids.Select(id => int.Parse(id)), expand: WorkItemExpand.All);
        return workItems
            .Where(w => w.Id.HasValue)
            .Select(w => WorkItemModel.For(
                w.Id!.Value,
                w.Fields["System.Title"] as string ?? string.Empty,
                $"{Settings.OrganizationUri}/{request.ProjectName}/_workItems/edit/{w.Id!.Value}"))
            .ToList();
    }
}
