using Microsoft.TeamFoundation.SourceControl.WebApi;
using System.Collections.Generic;
using System.Linq;

namespace AZDO.ReleaseNotes.Models;

public class CommitModel
{
    private CommitModel(
        string commitId,
        string comment,
        string url,
        List<CommitWorkItemModel> workItems)
    {
        Id = commitId;
        Comment = comment;
        Url = url;
        WorkItems = workItems;
    } 
    public string Id { get; }
    public string? Comment { get; }
    public string? Url { get; }
    public List<CommitWorkItemModel> WorkItems { get; }

    public static CommitModel From(GitCommitRef c)
        => new CommitModel(
            c.CommitId,
            c.Comment,
            c.Url,
            c.WorkItems.Select(w => CommitWorkItemModel.For(w.Id, c.CommitId, w.Url)).ToList());
}
