using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolWebsite1.Data.Repositories;
using SchoolWebsite1.Models;
using System.Threading.Tasks;

namespace SchoolWebsite1.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdmissionController : Controller
    {
        private readonly IAdmissionRepository _admissionRepository;

        public AdmissionController(IAdmissionRepository admissionRepository)
        {
            _admissionRepository = admissionRepository;
        }

        public async Task<IActionResult> Index()
        {
            var admissions = await _admissionRepository.GetAllAsync();
            return View(admissions); 
        }

        public async Task<IActionResult> Details(int id)
        {
            var admission = await _admissionRepository.GetByIdAsync(id);
            if (admission == null) return NotFound();
            return View(admission); 
        }

        public IActionResult Create()
        {
            return View(); 
        }

        [HttpPost]
        public async Task<IActionResult> Create(Admission model)
        {
            if (!ModelState.IsValid) return View(model);

            model.Status = "Pending";
            model.SubmittedAt = DateTime.UtcNow;

            await _admissionRepository.AddAsync(model);
            return RedirectToAction("Index");
        }

        // 5Update status (Approve/Reject)
        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            var admission = await _admissionRepository.GetByIdAsync(id);
            if (admission == null)
            {
                TempData["Error"] = "Admission not found!";
                return RedirectToAction("Index");
            }
            await _admissionRepository.UpdateStatusAsync(id, status);
            return RedirectToAction("Index");
        }

        // Delete admission
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var admission = await _admissionRepository.GetByIdAsync(id);
            if (admission == null)
            {
                TempData["Error"] = "Admission not found!";
                return RedirectToAction("Index");
            }
            await _admissionRepository.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}
