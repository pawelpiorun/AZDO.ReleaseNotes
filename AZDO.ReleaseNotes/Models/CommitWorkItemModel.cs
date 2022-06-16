namespace AZDO.ReleaseNotes.Models;

public class CommitWorkItemModel
{
    private CommitWorkItemModel(
        string id,
        string commitId,
        string url)
    {
        Id = id;
        CommitId = commitId;
        Url = url;
    }

    public string Id { get; }
    public string CommitId { get; }
    public string Url { get; }

    public static CommitWorkItemModel For(string id, string commitId, string url)
        => new CommitWorkItemModel(id, commitId, url);
}
