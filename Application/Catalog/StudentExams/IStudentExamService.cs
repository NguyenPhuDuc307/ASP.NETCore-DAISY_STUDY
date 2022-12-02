using DaisyStudy.Models.Catalog.StudentExams;
using DaisyStudy.Models.Common;

namespace DaisyStudy.Application.Catalog.StudentExams;

public interface IStudentExamService
{
    Task<int> Create(StudentExamsCreateRequest request);
    Task<int> Update(StudentExamsUpdateRequest request);
    Task<int> Delete(int StudentExamID);
    Task<StudentExamsViewModel> GetById(int ExamScheduleID, string UserId);
    Task<PagedResult<StudentExamsViewModel>> GetAllPaging(GetManageStudentExamPagingRequest request);
}