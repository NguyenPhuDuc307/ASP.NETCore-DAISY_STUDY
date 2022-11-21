using DaisyStudy.Models.Common;

namespace DaisyStudy.Models.Catalog.Classes;

public class GetManageClassPagingRequest : PagingRequestBase
{
    public string? Keyword { get; set; }
}