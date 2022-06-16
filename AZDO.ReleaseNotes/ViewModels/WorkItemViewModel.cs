using AZDO.ReleaseNotes.Models;

namespace AZDO.ReleaseNotes.ViewModels;

public class WorkItemViewModel
{
    private WorkItemViewModel(string id, string title, string url)
    {
        Id = id;
        Title = title;
        Url = url;
    }

    public string Id { get; }
    public string Title { get; }
    public string Url { get; }

    public static WorkItemViewModel From(WorkItemModel workItem)
        => new WorkItemViewModel(
            workItem.Id.ToString(),
            workItem.Title,
            workItem.Url);
}
