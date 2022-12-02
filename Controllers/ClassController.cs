using System.Text;
using Microsoft.AspNetCore.Mvc;
using DaisyStudy.Models.Catalog.Classes;
using DaisyStudy.Application.Catalog.Classes;
using AutoMapper;
using DaisyStudy.Application.Catalog.Rooms;
using DaisyStudy.Application.System.Users;

namespace DaisyStudy.Controllers
{
    public class ClassController : BaseController
    {
        private readonly IClassService _classService;
        private readonly IRoomService _roomService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public ClassController(
            IClassService classService,
            IMapper mapper,
            IRoomService roomService,
            IUserService userService)
        {
            _classService = classService;
            _mapper = mapper;
            _roomService = roomService;
            _userService = userService;
        }

        // GET: Classes
        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = 10)
        {
            var request = new ClassPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            var classes = await _classService.GetAllClassPaging(request);
            ViewBag.Keyword = keyword;
            if (TempData["result"] != null)
            {
                ViewBag.SuccessMsg = TempData["result"];
            }
            return View(classes);
        }

        [Route("danh-sach-lop-hoc")]
        public async Task<IActionResult> MyClass(string keyword, int pageIndex = 1, int pageSize = 10)
        {
            var request = new ClassPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            var classes = await _classService.GetAllMyClassPaging(request, HttpContext.Session.GetString("UserId").ToString());

            ViewBag.Keyword = keyword;
            if (TempData["result"] != null)
            {
                ViewBag.SuccessMsg = TempData["result"];
            }
            return View(classes);
        }

        [Route("giao-vien/danh-sach-lop-hoc")]
        public async Task<IActionResult> MyAdminClass(string keyword, int pageIndex = 1, int pageSize = 10)
        {
            var request = new ClassPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            var classes = await _classService.GetAllMyAdminClassPaging(request, HttpContext.Session.GetString("UserId").ToString());

            ViewBag.Keyword = keyword;
            if (TempData["result"] != null)
            {
                ViewBag.SuccessMsg = TempData["result"];
            }
            return View(classes);
        }

        [HttpGet("tong-quan-lop-hoc")]
        public async Task<IActionResult> OverView(int id)
        {
            var result = await _classService.GetById(id);
            if (result == null) NotFound();
            await _classService.AddViewCount(id);
            if (TempData["result"] != null)
            {
                ViewBag.SuccessMsg = TempData["result"];
            }
            return View(result);
        }

        [HttpGet("chi-tiet-lop-hoc")]
        public async Task<IActionResult> Details(int id)
        {
            var result = await _classService.GetById(id);
            if (result == null) NotFound();
            await _classService.AddViewCount(id);
            if (TempData["result"] != null)
            {
                ViewBag.SuccessMsg = TempData["result"];
            }
            return View(result);
        }

        [HttpGet("tao-lop-hoc")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("tao-lop-hoc")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] ClassCreateRequest request)
        {
            request.UserName = User.Identity.Name;
            if (!ModelState.IsValid)
                return View(request);

            var result = await _classService.Create(request);

            await _roomService.Create(request.UserName, request.ClassName);

            if (result != 0)
            {
                TempData["result"] = "Thêm mới lớp học thành công";
                return RedirectToAction("OverView", "Class", new{id = result});
            }

            ModelState.AddModelError("", "Thêm lớp học thất bại");
            return View(request);
        }

        [HttpPost, ActionName("JoinClass")]
        public async Task<IActionResult> JoinClass(int id)
        {
            var result = await _classService.JoinClass(id, User.Identity.Name);
            if (result.Contains("Tham gia lớp học thành công"))
            {
                TempData["result"] = result;
                return RedirectToAction("Details", new { id = id });
            }

            if (result.Contains("đã tham gia") == true)
            {
                return RedirectToAction("Details", new { id = id });
            }

            TempData["result"] = result;
            return RedirectToAction("OverView", new { id = id });
        }

        [HttpPost("tham-gia-lop-hoc"), ActionName("JoinClassById")]
        public async Task<IActionResult> JoinClassById(string ClassID)
        {
            var @class = await _classService.GetById(ClassID.Trim());
            if(@class == null) return BadRequest(); 
            var result = await _classService.JoinClass(@class.ID, User.Identity.Name);
            return RedirectToAction("OverView", new { id = @class.ID });
        }

        [HttpGet("chinh-sua-lop-hoc")]
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _classService.GetById(id);
            if (result == null) return NotFound();
            var classViewModel = _mapper.Map<ClassViewModel, ClassUpdateRequest>(result);
            return View(classViewModel);
        }

        [HttpPost("chinh-sua-lop-hoc")]
        [ValidateAntiForgeryToken]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Edit([FromForm] ClassUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);

            await _classService.Update(request);
            TempData["result"] = "Cập nhật lớp học thành công";
            return RedirectToAction("Details", "Class", new { id = request.ID });
        }

        public async Task<IActionResult> ChangeClassID(int id)
        {
            await _classService.ChangeClassID(id);
            TempData["result"] = "Đổi mã lớp thành công";
            return RedirectToAction("Details", "Class", new { id = id });
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadImage()
        {
            string filePath = "";
            if (!ModelState.IsValid)
                return View();
            List<ClassImageUpdateRequest> list = new List<ClassImageUpdateRequest>();
            foreach (IFormFile image in Request.Form.Files)
            {
                ClassImageUpdateRequest classImageUpdateRequest = new ClassImageUpdateRequest();
                classImageUpdateRequest.ThumbnailImage = image;
                list.Add(classImageUpdateRequest);
            }

            foreach (ClassImageUpdateRequest classImageCreateRequest in list)
            {
                filePath = await _classService.UploadImage(classImageCreateRequest);
            }
            return Json(new { url = filePath });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _classService.GetById(id);
            if (result == null) return NotFound();
            return View(result);
        }

        // POST: Classes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _classService.Delete(id);
            if (result != 0)
            {
                TempData["result"] = "Xoá lớp học thành công";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Xoá lớp học thất bại");
            var @class = await _classService.GetById(id);
            return View(@class);
        }
    }
}
