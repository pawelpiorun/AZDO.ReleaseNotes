using AZDO.ReleaseNotes.Commands;
using AZDO.ReleaseNotes.Infrastructure;
using AZDO.ReleaseNotes.Requests;
using AZDO.ReleaseNotes.Services;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;

namespace AZDO.ReleaseNotes.ViewModels;

public class MainViewModel : NotifyPropertyChanged
{
    private string projectName = "";
    private string repositoryName = "";
    private string branchName = "";
    private bool isProcessing;
    private bool isExporting;
    private DateTime startDate = DateTime.Today - TimeSpan.FromDays(3);
    private DateTime endDate = DateTime.Today;
    private readonly CommitsService commitsService;
    private readonly WorkItemsService workItemsService;

    public MainViewModel()
    {
        Commits = new ObservableCollection<CommitViewModel>();
        WorkItems = new ObservableCollection<WorkItemViewModel>();
        RunSearchCommand = new RunSearchCommand(this);
        ExportReleaseNotesCommand = new ExportReleaseNotesCommand(this);
        commitsService = new CommitsService();
        workItemsService = new WorkItemsService();
    }

    public string ProjectName
    {
        get => projectName;
        set => Set(ref projectName, value);
    }

    public string RepositoryName
    {
        get => repositoryName;
        set => Set(ref repositoryName, value);
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

    public bool IsExporting
    {
        get => isExporting;
        set => Set(ref isExporting, value);
    }

    public ObservableCollection<CommitViewModel> Commits { get; }
    public ObservableCollection<WorkItemViewModel> WorkItems { get; }

    public RunSearchCommand RunSearchCommand { get; }

    public ExportReleaseNotesCommand ExportReleaseNotesCommand { get; }

    public async void Run()
    {
        if (string.IsNullOrEmpty(Settings.OrganizationUri)
            || string.IsNullOrEmpty(Settings.PersonalAccessToken))
        {
            System.Windows.MessageBox.Show("Please setup OrganizationUri and PersonalAccessToken.");
            return;
        }

        IsProcessing = true;

        var searchCommitsRequest = CommitSearchRequest.For(
            projectName,
            repositoryName,
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
    public async void Export()
    {
        IsExporting = true;

        var saveDialog = new SaveFileDialog()
        {
            Filter = "Markup file (*.md)|*.md|Text files (*.txt)|*.txt|All files (*.*)|*.*",
        };
        _ = saveDialog.ShowDialog();
        var fileName = saveDialog.FileName;
        if (string.IsNullOrEmpty(fileName))
        {
            return;
        }

        var releaseNotes = WorkItems.Select(x => $"[{x.Id}: {x.Title}]({x.Url})");
        await File.WriteAllLinesAsync(fileName, releaseNotes);
        Process.Start(new ProcessStartInfo()
        {
            FileName = fileName,
            UseShellExecute = true,
        });

        IsExporting = false;
    }
}
