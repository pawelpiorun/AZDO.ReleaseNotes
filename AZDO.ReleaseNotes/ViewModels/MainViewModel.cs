using AZDO.ReleaseNotes.Commands;
using AZDO.ReleaseNotes.Infrastructure;
using AZDO.ReleaseNotes.Requests;
using AZDO.ReleaseNotes.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace AZDO.ReleaseNotes.ViewModels;

public class MainViewModel : NotifyPropertyChanged
{
    private string projectName = "";
    private string branchName = "";
    private bool isProcessing;
    private DateTime startDate = DateTime.Today - TimeSpan.FromDays(3);
    private DateTime endDate = DateTime.Today;
    private readonly CommitsService commitsService;
    private readonly WorkItemsService workItemsService;

    public MainViewModel()
    {
        Commits = new ObservableCollection<CommitViewModel>();
        WorkItems = new ObservableCollection<WorkItemViewModel>();
        RunSearchCommand = new RunSearchCommand(this);
        commitsService = new CommitsService();
        workItemsService = new WorkItemsService();
    }

    public string ProjectName
    {
        get => projectName;
        set => Set(ref projectName, value);
    }

    public string BranchName
    {
        get => branchName;
        set => Set(ref branchName, value);
    }

    public string FormattedStartDate => StartDate.ToString("MM/dd/yyyy");

    public DateTime StartDate
    {
        get => startDate;
        set => Set(ref startDate, value);
    }

    public string FormattedEndDate => EndDate.ToString("MM/dd/yyyy");

    public DateTime EndDate
    {
        get => endDate;
        set => Set(ref endDate, value);
    }

    public bool IsProcessing
    {
        get => isProcessing;
        set => Set(ref isProcessing, value);
    }

    public ObservableCollection<CommitViewModel> Commits { get; }
    public ObservableCollection<WorkItemViewModel> WorkItems { get; }

    public RunSearchCommand RunSearchCommand { get; }

    public async void Run()
    {
        if (string.IsNullOrEmpty(Settings.OrganizationUri)
            || string.IsNullOrEmpty(Settings.PersonalAccessToken))
        {
            MessageBox.Show("Please setup OrganizationUri and PersonalAccessToken.");
            return;
        }

        IsProcessing = true;

        var searchCommitsRequest = CommitSearchRequest.For(
            projectName,
            branchName,
            FormattedStartDate,
            FormattedEndDate,
            includeWorkItems: true);

        var commits = await commitsService.Search(searchCommitsRequest);
        Commits.Clear();
        foreach (var commit in commits)
        {
            Commits.Add(CommitViewModel.From(commit));
        }

        var allWorkItems = from commit in commits
                           from workItem in commit.WorkItems
                           select workItem.Id;

        var retrieveWorkItemsRequest = RetrieveWorkItemsRequest.ByIds(projectName, allWorkItems.ToList());
        var workItems = await workItemsService.GetByIds(retrieveWorkItemsRequest);
        WorkItems.Clear();
        foreach (var item in workItems)
        {
            WorkItems.Add(WorkItemViewModel.From(item));
        }

        IsProcessing = false;
    }
}
