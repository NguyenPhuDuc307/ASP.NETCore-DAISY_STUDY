using DaisyStudy.Application.Common;
using DaisyStudy.Data;
using DaisyStudy.Models.Catalog.Answers;
using DaisyStudy.Models.Common;
using DaisyStudy.Utilities.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;

namespace DaisyStudy.Application.Answers;

public class AnswerService : IAnswerService
{
    private readonly ApplicationDbContext _context;
    private readonly IStorageService _storageService;
    private const string USER_CONTENT_FOLDER_NAME = "user-content";

    public AnswerService(ApplicationDbContext context, IStorageService storageService)
    {
        _storageService = storageService;
        _context = context;
    }
    public async Task<int> Create(AnswerCreateRequest request)
    {
        var answer = new Answer()
        {
            QuestionID = request.QuestionID,
            AnswerString = request.AnswerString,
            IsCorrect = request.IsCorrect,
            ImageFileSize = request.FileSize
        };
        // Save file
        if (request.ImagePath != null)
        {
            answer.ImagePath = await this.SaveFile(request.ImagePath);
        }
        _context.Answers.Add(answer);
        await _context.SaveChangesAsync();
        return answer.AnswerID;
    }

    private async Task<string?> SaveFile(IFormFile imagePath)
    {
        var originalFileName = ContentDispositionHeaderValue.Parse(imagePath.ContentDisposition).FileName.Trim('"');
        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
        await _storageService.SaveFileAsync(imagePath.OpenReadStream(), fileName);
        return "/" + USER_CONTENT_FOLDER_NAME + "/" + fileName;
    }

    public async Task<int> Delete(int AnswerID)
    {
        var answer = await _context.Answers.FindAsync(AnswerID);
        if (answer == null) throw new DaisyStudyException($"Cannot find a answer {AnswerID}");

        _context.Answers.Remove(answer);
        return await _context.SaveChangesAsync();
    }

    public async Task<PagedResult<AnswerViewModel>> GetAllPaging(GetManageAnswerPagingRequest request)
    {
        //1. Select join
        var query = from a in _context.Answers
                    join q in _context.Questions on a.QuestionID equals q.QuestionID into aq
                    from q in aq.DefaultIfEmpty()
                    select new { a, q };
        //2. filter
        if (request.QuestionID != null && request.QuestionID != 0)
        {
            query = query.Where(p => p.a.QuestionID == request.QuestionID);
        }

        //3. Paging
        int totalRow = await query.CountAsync();
        var data = await query
           .Select(x => new AnswerViewModel()
           {
               AnswerString = x.a.AnswerString,
               IsCorrect = x.a.IsCorrect
           }).ToListAsync();

        //4. Select and projection
        var pagedResult = new PagedResult<AnswerViewModel>()
        {
            TotalRecords = totalRow,
            PageSize = request.PageSize,
            PageIndex = request.PageIndex,
            Items = data
        };
        return pagedResult;
    }

    public async Task<AnswerViewModel> GetById(int AnswerID)
    {
        var answer = await _context.Answers.FindAsync(AnswerID);
        if (answer == null) throw new DaisyStudyException($"Cannot find a answer {AnswerID}");

        var answerViewModel = new AnswerViewModel()
        {
           AnswerID = answer.AnswerID,
           QuestionID = answer.QuestionID,
           AnswerString = answer.AnswerString,
           IsCorrect = answer.IsCorrect,
           ImagePath = answer.ImagePath,
           FileSize = answer.ImageFileSize
        };
        return answerViewModel;
    }

    public async Task<int> Update(AnswerUpdateRequest request)
    {
        var answer = await _context.Answers.FindAsync(request.AnswerID);
        if (answer == null) throw new DaisyStudyException($"Cannot find a answer {request.AnswerID}");
        answer.AnswerString= request.AnswerString;
        answer.IsCorrect = request.IsCorrect;

        //Save image
        if (request.ImagePath != null)
        {
            if (answer.ImagePath != null)
            {
                answer.ImagePath = await this.SaveFile(request.ImagePath);
            }
        }
        return await _context.SaveChangesAsync();
    }
}