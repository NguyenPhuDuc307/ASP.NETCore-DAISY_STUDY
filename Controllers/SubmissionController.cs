using AutoMapper;
using DaisyStudy.Application.Catalog.Submissions;
using DaisyStudy.Models.Catalog.Submissions;
using Microsoft.AspNetCore.Mvc;

namespace DaisyStudy.Controllers;

public class SubmissionController : BaseController
{
    private readonly ISubmissionService _submissionService;
    private readonly ISubmissionService _classService;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;

    public SubmissionController(ISubmissionService submissionService,
                                  ISubmissionService classService,
                                  IMapper mapper,
                                  IConfiguration configuration)
    {
        _configuration = configuration;
        _submissionService = submissionService;
        _classService = classService;
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

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var result = await _submissionService.GetById(id);
        if (result == null) return NotFound();
        var submissionViewModel = _mapper.Map<SubmissionViewModel, SubmissionUpdateRequest>(result);
        return View(submissionViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Edit([FromForm] SubmissionUpdateRequest request)
    {
        if (!ModelState.IsValid)
            return View(request);

        var result = await _submissionService.Update(request);
        TempData["result"] = "Cập nhật thông báo thành công";
        var submission = await _submissionService.GetById(request.SubmissionID);
        return RedirectToAction("Details", "Homework", new { id = submission.HomeworkID });
    }

    public async Task<IActionResult> Delete(int id, string returnUrl)
    {
        if (!ModelState.IsValid)
            return View();

        var result = await _submissionService.Delete(id);
        if (result == true)
        {
            TempData["result"] = "Xoá thông báo thành công";
            return Redirect(returnUrl);
        }

        ModelState.AddModelError("", "Xoá thông báo thất bại");
        return Redirect(returnUrl);
    }
}