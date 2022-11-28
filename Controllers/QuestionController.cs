using AutoMapper;
using DaisyStudy.Application.Catalog.Questions;
using DaisyStudy.Models.Catalog.Question;
using Microsoft.AspNetCore.Mvc;

namespace DaisyStudy.Controllers;

public class QuestionController : BaseController
{
    private readonly IQuestionService _questionService;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;

    public QuestionController(IQuestionService QuestionService,
                              IConfiguration configuration,
                              IMapper mapper)
    {
        _configuration = configuration;
        _questionService = QuestionService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<JsonResult> LoadData()
    {
        var request = new GetManageQuestionPagingRequest()
        {
            Keyword = null,
            PageIndex = 1,
            PageSize = 10
        };
        var data = await _questionService.GetAllPaging(request);
        return Json(new{
            data = data.Items.ToList(),
            status = true
        });
    }

    public async Task<IActionResult> Index(string examSchedule, int pageIndex = 1, int pageSize = 10)
    {
        var request = new GetManageQuestionPagingRequest()
        {
            Keyword = examSchedule,
            PageIndex = pageIndex,
            PageSize = pageSize
        };
        var data = await _questionService.GetAllPaging(request);
        ViewBag.Keyword = examSchedule;
        if (TempData["result"] != null)
        {
            ViewBag.SuccessMsg = TempData["result"];
        }
        return View(data);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Create([FromForm] QuestionsCreateRequest request)
    {

        var result = await _questionService.Create(request);
        if (result != null)
        {
            TempData["result"] = "Thêm mới bài tập thành công";
            return RedirectToAction("Index");
        }

        ModelState.AddModelError("", "Thêm bài tập thất bại");
        return View(request);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var result = await _questionService.GetById(id);
        if (result == null) NotFound();
        if (TempData["result"] != null)
        {
            ViewBag.SuccessMsg = TempData["result"];
        }
        return View(result);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var result = await _questionService.GetById(id);
        if (result == null) return NotFound();
        var QuestionViewModel = _mapper.Map<QuestionViewModel, QuestionUpdateRequest>(result);
        return View(QuestionViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Edit([FromForm] QuestionUpdateRequest request)
    {
        if (!ModelState.IsValid)
            return View(request);

        var result = await _questionService.Update(request);
        TempData["result"] = "Cập nhật bài tập thành công";
        var Question = await _questionService.GetById(request.QuestionID);
        return RedirectToAction("Details", "Question", new { id = Question.QuestionID });
    }

    public async Task<IActionResult> Delete(int id, string returnUrl)
    {
        if (!ModelState.IsValid)
            return View();
        var Question = await _questionService.GetById(id);
        var result = await _questionService.Delete(id);
        if (result != 0)
        {
            TempData["result"] = "Xoá bài tập thành công";
            return RedirectToAction("Details", "Class", new { id = Question.QuestionID });
        }

        ModelState.AddModelError("", "Xoá bài tập thất bại");
        return Redirect(returnUrl);
    }
}