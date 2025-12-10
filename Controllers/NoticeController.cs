using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolWebsite1.Data.Repositories;
using SchoolWebsite1.Models;
using System.Threading.Tasks;

namespace SchoolWebsite1.Controllers
{
    [Authorize(Roles = "Admin")]
    public class NoticeController : Controller
    {
        private readonly INoticeRepository _noticeRepository;

        public NoticeController(INoticeRepository noticeRepository)
        {
            _noticeRepository = noticeRepository;
        }

        //  List all notices (Admin)
        public async Task<IActionResult> Index()
        {
            var notices = await _noticeRepository.GetAllAsync();
            return View(notices);
        }

        // Show Create Notice Form
        public IActionResult Create()
        {
            return View();
        }

        //  Save Notice
        [HttpPost]
        public async Task<IActionResult> Create(Notice model)
        {
            if (!ModelState.IsValid)
                return View(model);

            model.CreatedAt = DateTime.Now;

            await _noticeRepository.AddAsync(model);
            return RedirectToAction("Index");
        }

        //  Delete notice
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var notice = await _noticeRepository.GetByIdAsync(id);
            if (notice == null)
            {
                return NotFound();
            }
            await _noticeRepository.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}
