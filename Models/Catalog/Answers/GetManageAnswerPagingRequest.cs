using DaisyStudy.Models.Common;

namespace DaisyStudy.Models.Catalog.Answers;

public class GetManageAnswerPagingRequest : PagingRequestBase
{
    public string? Keyword { set; get; }

    public int QuestionID { set; get; }
}