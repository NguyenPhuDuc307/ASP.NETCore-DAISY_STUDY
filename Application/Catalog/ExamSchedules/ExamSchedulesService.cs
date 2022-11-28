using DaisyStudy.Data;
using DaisyStudy.Models.Catalog.ExamSchedules;
using DaisyStudy.Models.Common;
using DaisyStudy.Utilities.Exceptions;
using Microsoft.EntityFrameworkCore;


namespace DaisyStudy.Application.Catalog.ExamSchedules;

public class ExamScheduleService : IExamScheduleService
{
    private readonly ApplicationDbContext _context;
    public ExamScheduleService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Create(ExamSchedulesCreateRequest request)
    {
        var examSchedule = new ExamSchedule()
        {
            ClassID = request.ClassID,
            ExamScheduleName = request.ExamScheduleName,
            ExamDateTime = request.ExamDateTime,
            ExamTime = request.ExamTime,
            DateTimeCreated = DateTime.Now,
            Description = request.Description,
        };
        _context.ExamSchedules.Add(examSchedule);
        await _context.SaveChangesAsync();
        return examSchedule.ExamScheduleID;
    }

    public async Task<int> Delete(int ExamScheduleID)
    {
        var examSchedule = await _context.ExamSchedules.FindAsync(ExamScheduleID);
        if (examSchedule == null) throw new DaisyStudyException($"Cannot find a class {ExamScheduleID}");

        _context.ExamSchedules.Remove(examSchedule);
        return await _context.SaveChangesAsync();
    }

    public async Task<PagedResult<ExamSchedulesViewModel>> GetAllPaging(GetManageExamSchedulesPagingRequest request)
    {
        //1. Select join
        var query = from es in _context.ExamSchedules
                    join c in _context.Classes on es.ClassID equals c.ID into esc
                    from c in esc.DefaultIfEmpty()
                    select new { es, c };
        //2. filter
        if (!string.IsNullOrEmpty(request.Keyword))
            query = query.Where(x => x.es.ExamScheduleName.Contains(request.Keyword)
                || x.c.ClassID.Contains(request.Keyword));

        if (request.ClassID != null && request.ClassID != 0)
        {
            query = query.Where(p => p.es.ClassID == request.ClassID);
        }

        //3. Paging
        int totalRow = await query.CountAsync();

        var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(x => new ExamSchedulesViewModel()
            {
                ExamScheduleID = x.es.ExamScheduleID,
                ClassID = x.c.ID,
                ExamScheduleName = x.es.ExamScheduleName,
                DateTimeCreated = x.es.DateTimeCreated,
                ExamDateTime = x.es.ExamDateTime,
                ExamTime = x.es.ExamTime,
                Description = x.es.Description
            }).ToListAsync();

        //4. Select and projection
        var pagedResult = new PagedResult<ExamSchedulesViewModel>()
        {
            TotalRecords = totalRow,
            PageSize = request.PageSize,
            PageIndex = request.PageIndex,
            Items = data
        };
        return pagedResult;
    }

    public async Task<ExamSchedulesViewModel> GetById(int ExamScheduleID)
    {
        var examSchedule = await _context.ExamSchedules.FindAsync(ExamScheduleID);
        if (examSchedule == null) throw new DaisyStudyException($"Cannot find a exam schedules {ExamScheduleID}");

        var _class = await _context.Classes.FindAsync(examSchedule.ClassID);
        if (_class == null) throw new DaisyStudyException($"Cannot find a class {examSchedule.ClassID}");

        var examScheduleViewModel = new ExamSchedulesViewModel()
        {
            ExamScheduleID = examSchedule.ExamScheduleID,
            ClassID = _class.ID,
            ClassName = _class.ClassName,
            ExamScheduleName = examSchedule.ExamScheduleName,
            DateTimeCreated = examSchedule.DateTimeCreated,
            ExamDateTime = examSchedule.ExamDateTime,
            ExamTime = examSchedule.ExamTime,
            Description = examSchedule.Description
        };
        return examScheduleViewModel;  
    }

    public async Task<int> Update(ExamSchedulesUpdateRequest request)
    {
        var examSchedule = await _context.ExamSchedules.FindAsync(request.ExamScheduleID);
        if (examSchedule == null) throw new DaisyStudyException($"Cannot find a exam schedule {request.ExamScheduleID}");
        examSchedule.ExamScheduleName = request.ExamScheduleName;
        examSchedule.ExamDateTime = request.ExamDateTime;
        examSchedule.ExamTime = request.ExamTime;
        examSchedule.Description = request.Description;
        return await _context.SaveChangesAsync();
    }
}