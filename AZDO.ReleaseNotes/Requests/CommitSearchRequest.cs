namespace AZDO.ReleaseNotes.Requests;

public class CommitSearchRequest
{
    private CommitSearchRequest(
        string projectName,
        string repositoryName,
        string branch,
        string startDate,
        string endDate,
        bool includeWorkItems,
        int pageSize,
        int startFrom)
    {
        ProjectName = projectName;
        RepositoryName = repositoryName;
        Branch = branch;
        StartDate = startDate;
        EndDate = endDate;
        IncludeWorkItems = includeWorkItems;
        PageSize = pageSize;
        StartFrom = startFrom;
    }

    public string ProjectName { get; }
    public string RepositoryName { get; }
    public string Branch { get; }
    public string StartDate { get; }
    public string EndDate { get; }
    public bool IncludeWorkItems { get; }
    public int PageSize { get; }
    public int StartFrom { get; }

    internal static CommitSearchRequest For(
        string projectName,
        string repositoryName,
        string branch,
        string startDate,
        string endDate,
        bool includeWorkItems,
        int pageSize = 1000,
        int startFrom = 0)
        => new CommitSearchRequest(projectName, repositoryName, branch, startDate, endDate, includeWorkItems, pageSize, startFrom);
}
