using DaisyStudy.Models.Catalog.Answers;
using DaisyStudy.Models.Common;

namespace DaisyStudy.Application.Catalog.Answers;

public interface IAnswerService
{
    Task<int> Create(AnswerCreateRequest request);
    Task<int> Update(AnswerUpdateRequest request);
    Task<int> Delete(int AnswerID);
    Task<AnswerViewModel> GetById(int AnswerID);
    Task<PagedResult<AnswerViewModel>> GetAllPaging(GetManageAnswerPagingRequest request);
}