using DaisyStudy.Models.Catalog.StudentExams;
using DaisyStudy.Models.Common;

namespace DaisyStudy.Application.StudentExams;

public interface IStudentExamService
{
    Task<int> Create(StudentExamsCreateRequest request);
    Task<int> Update(StudentExamsUpdateRequest request);
    Task<int> Delete(int StudentExamID);
    Task<StudentExamsViewModel> GetById(int ExamScheduleID);
    Task<PagedResult<StudentExamsViewModel>> GetAllPaging(GetManageStudentExamPagingRequest request);
}