using AZDO.ReleaseNotes.Models;

namespace AZDO.ReleaseNotes.ViewModels;

public class CommitWorkItemViewModel
{
    public CommitWorkItemViewModel(string id, string commitId, string url)
    {
        Id = id;
        CommitId = commitId;
        Url = url;
    }

    public string Id { get; }
    public string CommitId { get; }
    public string Url { get; }

    public static CommitWorkItemViewModel From(CommitWorkItemModel commitWorkItem)
        => new(
            commitWorkItem.Id,
            commitWorkItem.CommitId,
            commitWorkItem.Url);
}
