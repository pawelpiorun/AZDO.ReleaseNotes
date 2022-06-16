using AZDO.ReleaseNotes.Models;
using AZDO.ReleaseNotes.Requests;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AZDO.ReleaseNotes.Services;

public class CommitsService
{
    public CommitsService()
    {
        // get CollectionUri and PAT from config?
    }

    public async Task<IEnumerable<CommitModel>> Search(
        CommitSearchRequest commitSearchRequestModel)
    {
        using var vssConnection = new VssConnection(
            new Uri(Settings.OrganizationUri),
            new VssBasicCredential(string.Empty, Settings.PersonalAccessToken));
        using var gitClient = vssConnection.GetClient<GitHttpClient>();

        var descriptor = new GitVersionDescriptor()
        {
            Version = commitSearchRequestModel.Branch,
            VersionType = GitVersionType.Branch
        };
        var commits = await gitClient.GetCommitsAsync(
            commitSearchRequestModel.ProjectName,
            commitSearchRequestModel.ProjectName,
            new GitQueryCommitsCriteria()
            {
                IncludeWorkItems = commitSearchRequestModel.IncludeWorkItems,
                ItemVersion = descriptor,

                FromDate = commitSearchRequestModel.StartDate,
                ToDate = commitSearchRequestModel.EndDate,
                Top = commitSearchRequestModel.PageSize,
                Skip = commitSearchRequestModel.StartFrom,
            });

        return commits.Select(c => CommitModel.From(c)).ToList();
    }
}
