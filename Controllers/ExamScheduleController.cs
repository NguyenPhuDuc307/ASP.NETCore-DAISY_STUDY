using DaisyStudy.Application.Catalog.ExamSchedules;
using DaisyStudy.Models.Catalog.ExamSchedules;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

namespace ASP.NETCoreIdentityCustom.Controllers
{
    public class ExamScheduleController : Controller
    {
        private readonly IExamScheduleService _examScheduleService;
        private readonly IMapper _mapper;

        public ExamScheduleController(IExamScheduleService examScheduleService, IMapper mapper)
        {
            _examScheduleService = examScheduleService;
            _mapper = mapper;
        }

        [HttpGet("them-ky-thi")]
        public IActionResult Create()
        {
            if (User.Identity != null)
            {
                var user = User.Identity.Name;
            }
            return View();
        }

        [HttpPost("them-ky-thi")]
        public async Task<IActionResult> Create([FromForm] ExamSchedulesCreateRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);


            var result = await _examScheduleService.Create(request);
            if (result != null)
            {
                TempData["result"] = "Thêm mới kỳ thi thành công";
                return RedirectToAction("Details", "Class", new { id = request.ClassID });
            }

            ModelState.AddModelError("", "Thêm kỳ thi thất bại");
            return View(request);
        }

        [HttpGet("ky-thi")]
        public async Task<IActionResult> OverView(int id)
        {
            var result = await _examScheduleService.GetById(id);
            if (result == null) NotFound();
            if (TempData["result"] != null)
            {
                ViewBag.SuccessMsg = TempData["result"];
            }
            return View(result);
        }

        [HttpGet("de-thi")]
        public async Task<IActionResult> Details(int id)
        {
            var result = await _examScheduleService.GetById(id);
            if (result == null) NotFound();
            if (TempData["result"] != null)
            {
                ViewBag.SuccessMsg = TempData["result"];
            }
            return View(result);
        }

        [HttpGet("chinh-sua-ky-thi")]
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _examScheduleService.GetById(id);
            if (result == null) return NotFound();
            var examSchedulesViewModel = _mapper.Map<ExamSchedulesViewModel, ExamSchedulesUpdateRequest>(result);
            return View(examSchedulesViewModel);
        }

        [HttpPost("chinh-sua-ky-thi")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromForm] ExamSchedulesUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);

            var result = await _examScheduleService.Update(request);
            TempData["result"] = "Cập nhật kỳ thi thành công";
            var homework = await _examScheduleService.GetById(request.ExamScheduleID);
            return RedirectToAction("Details", "ExamSchedule", new { id = homework.ExamScheduleID });
        }

        public async Task<IActionResult> Delete(int id, string returnUrl)
        {
            if (!ModelState.IsValid)
                return View();
            var schedulesViewModel = await _examScheduleService.GetById(id);
            var result = await _examScheduleService.Delete(id);
            if (result != 0)
            {
                TempData["result"] = "Xoá kỳ thi thành công";
                return RedirectToAction("Details", "Class", new { id = schedulesViewModel.ClassID});
            }

            ModelState.AddModelError("", "Xoá kỳ thi thất bại");
            return Redirect(returnUrl);
        }

    }
}