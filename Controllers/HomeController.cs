using Microsoft.AspNetCore.Mvc;
using SchoolWebsite1.Data.Repositories;
using SchoolWebsite1.Models;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SchoolWebsite1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly INoticeRepository _noticeRepository;
        private readonly IAdmissionRepository _admissionRepository;

        public HomeController(ILogger<HomeController> logger,INoticeRepository noticeRepository, IAdmissionRepository admissionRepository)
        {
            _logger = logger;
            _noticeRepository = noticeRepository;
            _admissionRepository = admissionRepository;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Admission()
        {
            return View("Admission"); 
        }
        [HttpPost]
        public async Task<IActionResult> Admission(Admission model)
        {
            if (ModelState.IsValid)
            {
                await _admissionRepository.AddAsync(model);
                TempData["Message"] = "Your application has been submitted successfully. Current Status: Pending.You will be notified by Email.";
                return RedirectToAction("Confirmation"); 
            }

            return View("Admission", model);
        }

        [HttpGet]
        public IActionResult Confirmation()
        {
            ViewBag.Message = TempData["Message"];
            return View(); 
        }
        public async Task<IActionResult> Notice()
        {
            var notices = await _noticeRepository.GetAllAsync();
            return View(notices);
        }
        public IActionResult Contact()
        {
            return View();
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
