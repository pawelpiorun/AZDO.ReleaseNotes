using System.Collections.Generic;

namespace AZDO.ReleaseNotes.Requests;

public class RetrieveWorkItemsRequest
{
    public RetrieveWorkItemsRequest(string projectName, List<string> ids)
    {
        ProjectName = projectName;
        Ids = ids;
    }

    public List<string> Ids { get; }

    public string ProjectName { get; }

    public static RetrieveWorkItemsRequest ByIds(string projectName, List<string> ids)
        => new RetrieveWorkItemsRequest(projectName, ids);
}
