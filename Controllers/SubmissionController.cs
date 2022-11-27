using AutoMapper;
using DaisyStudy.Application.Catalog.Homeworks;
using DaisyStudy.Application.Catalog.Submissions;
using DaisyStudy.Models.Catalog.Submissions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DaisyStudy.Controllers;

public class SubmissionController : BaseController
{
    private readonly ISubmissionService _submissionService;
    private readonly IHomeworkService _homeworkService;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;

    public SubmissionController(ISubmissionService submissionService,
                                IHomeworkService homeworkService,
                                IMapper mapper,
                                IConfiguration configuration)
    {
        _configuration = configuration;
        _submissionService = submissionService;
        _homeworkService = homeworkService;
        _mapper = mapper;
    }

    public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = 10)
    {
        var request = new GetManageSubmissionPagingRequest()
        {
            PageIndex = pageIndex,
            PageSize = pageSize
        };
        var data = await _submissionService.GetAllPaging(request);
        ViewBag.Keyword = keyword;
        if (TempData["result"] != null)
        {
            ViewBag.SuccessMsg = TempData["result"];
        }
        return View(data);
    }

    [HttpGet("danh-sach-nop-bai")]
    public async Task<IActionResult> ListSubmission(int bai_tap, int pageIndex = 1, int pageSize = 10)
    {
        var request = new GetManageSubmissionPagingRequest()
        {
            PageIndex = pageIndex,
            PageSize = pageSize,
            HomeworkID = bai_tap
        };
        var data = await _submissionService.GetAllPaging(request);
        ViewBag.Keyword = bai_tap;

        var homeworks = await _homeworkService.GetAll();
        ViewBag.Homeworks = homeworks.Select(x=> new SelectListItem(){
            Text = x.HomeworkName + " - " + x.ClassName,
            Value = x.HomeworkID.ToString()
        });
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
    public async Task<IActionResult> Create([FromForm] SubmissionCreateRequest request)
    {
        if (!ModelState.IsValid)
        {
            TempData["result"] = "Vui lòng nhập đầy đủ cho thông báo";
            return Redirect(request.ReturnUrl);
        }

        var result = await _submissionService.Create(request);
        if (result == true)
        {
            TempData["result"] = "Nộp bài thành công";
            if (request.ReturnUrl != null)
                return Redirect(request.ReturnUrl);
            return RedirectToAction(request.ReturnUrl);
        }
        TempData["result"] = "Nộp bài thất bại, bạn đã nộp bài rồi, chỉ có thể chỉnh sửa";
        return Redirect(request.ReturnUrl);
    }

    [HttpGet("nop-bai")]
    public async Task<IActionResult> Edit(int id)
    {
        var result = await _submissionService.GetById(id);
        if (result == null) return NotFound();
        var submissionViewModel = _mapper.Map<SubmissionViewModel, SubmissionUpdateRequest>(result);
        return View(submissionViewModel);
    }

    [HttpPost("nop-bai")]
    [ValidateAntiForgeryToken]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Edit([FromForm] SubmissionUpdateRequest request)
    {
        if (!ModelState.IsValid)
            return View(request);

        var result = await _submissionService.Update(request);
        TempData["result"] = "Cập nhật bài nộp thành công";
        var submission = await _submissionService.GetById(request.SubmissionID);
        return RedirectToAction("Details", "Homework", new { id = submission.HomeworkID });
    }

    [HttpPost("cham-diem")]
    public async Task<IActionResult> UpdateMark([FromForm] SubmissionUpdateMarkRequest request)
    {
        if (request.Mark == null || request.Mark < 0)
            return RedirectToAction("Details", "Submission", new { id = request.SubmissionID });
        await _submissionService.UpdateMark(request);
        TempData["result"] = "Cập nhật điểm thành công";
        return RedirectToAction("Details", "Submission", new { id = request.SubmissionID });
    }

    public async Task<IActionResult> Delete(int id, string returnUrl)
    {
        if (!ModelState.IsValid)
            return View();
        var submission = await _submissionService.GetById(id);
        var result = await _submissionService.Delete(id);
        if (result == true)
        {
            TempData["result"] = "Xoá bài nộp thành công";
            return RedirectToAction("Details", "Homework", new { id = submission.HomeworkID });
        }

        ModelState.AddModelError("", "Xoá bài nộp thất bại");
        return Redirect(returnUrl);
    }

    [HttpGet("chi-tiet-bai-nop")]
    public async Task<IActionResult> Details(int id)
    {
        var result = await _submissionService.GetById(id);
        if (result == null) NotFound();
        if (TempData["result"] != null)
        {
            ViewBag.SuccessMsg = TempData["result"];
        }
        return View(result);
    }
}