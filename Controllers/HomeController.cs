using AutoMapper;
using DaisyStudy.Application.Catalog.Classes;
using DaisyStudy.Application.System.Users;
using DaisyStudy.Models;
using DaisyStudy.Data;
using DaisyStudy.Models.Catalog.Classes;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DaisyStudy.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IClassService _classService;
        private readonly ApplicationDbContext _context;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public HomeController(IClassService classService,
                              IMapper mapper,
                              IUserService userService,
                              ApplicationDbContext context)
        {
            _classService = classService;
            _mapper = mapper;
            _userService = userService;
            _context = context;
        }

        public async Task<IActionResult> Index(string keyword,
                                         int pageIndex = 1,
                                         int pageSize = 10)
        {
            if (User.Identity != null)
            {
                var user = User.Identity.Name;
            }

            var request = new ClassPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            var data = await _classService.GetAllClassPagingHome(request);
            
            ViewBag.Keyword = keyword;
            if (TempData["result"] != null)
            {
                ViewBag.SuccessMsg = TempData["result"];
            }
            return View(data);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}