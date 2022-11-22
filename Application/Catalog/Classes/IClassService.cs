using DaisyStudy.Data.Enums;
using DaisyStudy.Models.Catalog.Classes;
using DaisyStudy.Models.Common;

namespace DaisyStudy.Application.Catalog.Classes
{
    public interface IClassService
    {
        Task<int> Create(ClassCreateRequest request);
        Task<string> UploadImage(ClassImageUpdateRequest request);
        Task<int> Update(ClassUpdateRequest request);
        Task<int> Delete(int ID);
        Task<ClassViewModel> GetById(int ID);
        Task<bool> UpdateTuition(int ID, decimal tuition);
        Task<bool> UpdateStatus(int ID, Status status);
        Task<bool> UpdateIsPublic(int ID, IsPublic isPublic);
        Task AddViewCount(int ID);
        Task<PagedResult<ClassViewModel>> GetAllClassPaging(ClassPagingRequest request);
        Task<PagedResult<ClassViewModel>> GetAllClassPagingHome(ClassPagingRequest request);
        Task<PagedResult<ClassDetailViewModel>> GetAllStudentByClassIDPaging(GetAllStudentInClassPagingRequest request);
        Task<int> UpdateImage(int classID, ClassImageUpdateRequest request);
        Task<bool> ChangeClassID(int ID);
        Task<string> JoinClass(int ClassID, string UserName);
    }
}