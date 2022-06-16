namespace AZDO.ReleaseNotes.Models;

public class WorkItemModel
{
    private WorkItemModel(
        int id,
        string title,
        string url)
    {
        Id = id;
        Title = title;
        Url = url;
    }

    public int Id { get; }
    public string Title { get; }
    public string Url { get; }

    public static WorkItemModel For(int id, string title, string url)
        => new WorkItemModel(id, title, url);
}
