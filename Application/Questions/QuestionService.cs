using DaisyStudy.Utilities.Exceptions;
using DaisyStudy.Models.Common;
using DaisyStudy.Models.Catalog.Question;
using Microsoft.EntityFrameworkCore;
using DaisyStudy.Application.Questions;
using DaisyStudy.Application.Common;
using System.Net.Http.Headers;
using DaisyStudy.Data;

namespace DaisyStudy.Application.Catalog.Questions;

public class QuestionService : IQuestionService
{
    private readonly ApplicationDbContext _context;
    private readonly IStorageService _storageService;
    private const string USER_CONTENT_FOLDER_NAME = "user-content";

    public QuestionService(ApplicationDbContext context, IStorageService storageService)
    {
        _storageService = storageService;
        _context = context;
    }

    public async Task<int> Create(QuestionsCreateRequest request)
    {
        var question = new Question()
        {
            ExamScheduleID = request.ExamScheduleID,
            QuestionString = request.QuestionString,
            Point = request.Point,
            ImageFileSize = request.FileSize
        };
        // Save file
        if (request.ThumbnailImage != null)
        {
            question.ImagePath = await this.SaveFile(request.ThumbnailImage);
        }
        _context.Questions.Add(question);
        await _context.SaveChangesAsync();
        return question.QuestionID;
    }

    private async Task<string?> SaveFile(IFormFile thumbnailImage)
    {
        var originalFileName = ContentDispositionHeaderValue.Parse(thumbnailImage.ContentDisposition).FileName.Trim('"');
        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
        await _storageService.SaveFileAsync(thumbnailImage.OpenReadStream(), fileName);
        return "/" + USER_CONTENT_FOLDER_NAME + "/" + fileName;
    }

    public async Task<int> Delete(int QuestionID)
    {
        var question = await _context.Questions.FindAsync(QuestionID);
        if (question == null) throw new DaisyStudyException($"Cannot find a question {QuestionID}");

        _context.Questions.Remove(question);
        return await _context.SaveChangesAsync();
    }

    public async Task<PagedResult<QuestionViewModel>> GetAllPaging(GetManageQuestionPagingRequest request)
    {
        //1. Select join
        var query = from q in _context.Questions
                    join e in _context.ExamSchedules on q.ExamScheduleID equals e.ExamScheduleID into qn
                    from e in qn.DefaultIfEmpty()
                    select new { q, e };
        //2. filter
        if (request.ExamScheduleID != null && request.ExamScheduleID != 0)
        {
            query = query.Where(p => p.q.ExamScheduleID == request.ExamScheduleID);
        }

        //3. Paging
        int totalRow = await query.CountAsync();
        var data = await query
           .Select(x => new QuestionViewModel()
            {
                QuestionID = x.q.QuestionID,
                ExamScheduleID = x.e.ExamScheduleID,
                QuestionString = x.q.QuestionString,
                Point = x.q.Point,
            }).ToListAsync();

        //4. Select and projection
        var pagedResult = new PagedResult<QuestionViewModel>()
        {
            TotalRecords = totalRow,
            PageSize = request.PageSize,
            PageIndex = request.PageIndex,
            Items = data
        };
        return pagedResult;
    }

    public async Task<QuestionViewModel> GetById(int QuestionID)
    {
        var question = await _context.Questions.FindAsync(QuestionID);
        if (question == null) throw new DaisyStudyException($"Cannot find a question {QuestionID}");

        var questionViewModel = new QuestionViewModel()
        {
            QuestionID = question.QuestionID,
            ExamScheduleID = question.ExamScheduleID,
            QuestionString = question.QuestionString,
            Point = question.Point,
            ImagePath = question.ImagePath,
            FileSize = question.ImageFileSize
        };
        return questionViewModel;
    }

    public async Task<int> Update(QuestionUpdateRequest request)
    {
        var question = await _context.Questions.FindAsync(request.QuestionID);
        if (question == null) throw new DaisyStudyException($"Cannot find a question {request.QuestionID}");
        question.QuestionString = request.QuestionString;
        question.Point = request.Point;

        //Save image
        if (request.ThumbnailImage != null)
        {
            if (question.ImagePath != null)
            {
                question.ImagePath = await this.SaveFile(request.ThumbnailImage);
            }
        }
        return await _context.SaveChangesAsync();
    }
}