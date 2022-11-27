using DaisyStudy.Models.Catalog.ExamSchedules;
using DaisyStudy.Models.Common;

namespace DaisyStudy.Application.Catalog.ExamSchedules;

public interface  IExamScheduleService
{
    Task<int> Create(ExamSchedulesCreateRequest request);
    Task<int> Update(ExamSchedulesUpdateRequest request);
    Task<int> Delete(int ExamScheduleID);
    Task<ExamSchedulesViewModel> GetById(int ExamScheduleID);
    Task<PagedResult<ExamSchedulesViewModel>> GetAllPaging(GetManageExamSchedulesPagingRequest request);
}