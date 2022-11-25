using AutoMapper;
using DaisyStudy.Application.Catalog.Homeworks;
using DaisyStudy.Models.Catalog.Homeworks;
using Microsoft.AspNetCore.Mvc;

namespace DaisyStudy.Controllers;

public class HomeworkController : BaseController
{
    private readonly IHomeworkService _homeworkService;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;

    public HomeworkController(IHomeworkService homeworkService,
                              IConfiguration configuration,
                              IMapper mapper)
    {
        _configuration = configuration;
        _homeworkService = homeworkService;
        _mapper = mapper;
    }

    public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = 10)
    {
        var request = new GetManageHomeworkPagingRequest()
        {
            Keyword = keyword,
            PageIndex = pageIndex,
            PageSize = pageSize
        };
        var data = await _homeworkService.GetAllPaging(request);
        ViewBag.Keyword = keyword;
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
    public async Task<IActionResult> Create([FromForm] HomeworkCreateRequest request)
    {
        if (request.SubmissionDateTime == new DateTime(0001, 01, 01, 00, 00, 00))
            request.SubmissionDateTime = DateTime.Now;

        if (request.HomeworkName == null || request.Deadline == null || request.Description == null)
            return View(request);
        var result = await _homeworkService.Create(request);
        if (result != null)
        {
            TempData["result"] = "Thêm mới lớp học thành công";
            return RedirectToAction("Index");
        }

        ModelState.AddModelError("", "Thêm lớp học thất bại");
        return View(request);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var result = await _homeworkService.GetById(id);
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
        var result = await _homeworkService.GetById(id);
        if (result == null) return NotFound();
        var homeworkViewModel = _mapper.Map<HomeworkViewModel, HomeworkUpdateRequest>(result);
        return View(homeworkViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Edit([FromForm] HomeworkUpdateRequest request)
    {
        if (!ModelState.IsValid)
            return View(request);

        var result = await _homeworkService.Update(request);
        TempData["result"] = "Cập nhật bài tập thành công";
        var homework = await _homeworkService.GetById(request.HomeworkID);
        return RedirectToAction("Details", "Homework", new { id = homework.HomeworkID });
    }

    public async Task<IActionResult> Delete(int id, string returnUrl)
    {
        if (!ModelState.IsValid)
            return View();
        var homework = await _homeworkService.GetById(id);
        var result = await _homeworkService.Delete(id);
        if (result != 0)
        {
            TempData["result"] = "Xoá bài tập thành công";
            return RedirectToAction("Details", "Class", new { id = homework.ID});
        }

        ModelState.AddModelError("", "Xoá bài tập thất bại");
        return Redirect(returnUrl);
    }
}