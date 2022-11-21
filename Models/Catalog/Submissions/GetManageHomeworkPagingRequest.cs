using DaisyStudy.Data.Enums;
using DaisyStudy.Models.Common;

namespace DaisyStudy.Models.Catalog.Submissions;

public class GetManageSubmissionPagingRequest : PagingRequestBase
{
    public Delay Delay { get; set; }
    public int HomeworkID { get; set; }
}