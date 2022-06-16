using AZDO.ReleaseNotes.Infrastructure;
using AZDO.ReleaseNotes.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace AZDO.ReleaseNotes.ViewModels;

public class CommitViewModel : NotifyPropertyChanged
{
    public CommitViewModel(
        string id,
        string? comment,
        string? url,
        ObservableCollection<CommitWorkItemViewModel> workItems)
    {
        Id = id;
        Comment = comment;
        Url = url;
        WorkItems = workItems;
    }

    public string Id { get; }
    public string? Comment { get; }
    public string? Url { get; }
    public ObservableCollection<CommitWorkItemViewModel> WorkItems { get; }

    public static CommitViewModel From(CommitModel commit)
        => new CommitViewModel(
            commit.Id,
            commit.Comment,
            commit.Url,
            new ObservableCollection<CommitWorkItemViewModel>(
                commit.WorkItems.Select(CommitWorkItemViewModel.From).ToList()));
}
