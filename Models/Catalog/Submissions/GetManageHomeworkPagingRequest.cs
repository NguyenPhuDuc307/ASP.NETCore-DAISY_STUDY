using DaisyStudy.Data.Enums;
using DaisyStudy.Models.Common;

namespace DaisyStudy.Models.Catalog.Submissions;

public class GetManageSubmissionPagingRequest : PagingRequestBase
{
    public int HomeworkID { get; set; }
}