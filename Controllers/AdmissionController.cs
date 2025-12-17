using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolWebsite1.Data.Repositories;
using SchoolWebsite1.Models;
using System.Threading.Tasks;
using SchoolWebsite1.Services;

namespace SchoolWebsite1.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdmissionController : Controller
    {
        private readonly IAdmissionRepository _admissionRepository;
        private readonly IEmailService _emailService;

        public AdmissionController(IAdmissionRepository admissionRepository, IEmailService emailService)
        {
            _admissionRepository = admissionRepository;
            _emailService = emailService;
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
            // Email logic
            string subject;
            string body;

            if (status == "Accepted")
            {
                subject = "Admission Accepted – Morning Star High School";
                body = $@"
            <h3>Congratulations {admission.ApplicantFullName}!</h3>
            <p>Your admission for class <strong>{admission.ApplyingForClass}</strong> has been <b>ACCEPTED</b>.</p>
            <p>Our administration will contact you soon.</p>
            <br/>
            <p>Regards,<br/>Morning Star High School</p>";
            }
            else
            {
                subject = "Admission Status Update – Morning Star High School";
                body = $@"
            <p>Dear {admission.ApplicantFullName},</p>
            <p>Thank you for applying to Morning Star High School.</p>
            <p>We regret to inform you that your application was <b>NOT ACCEPTED</b> at this time.</p>
            <p>We wish you the best for the future.</p>
            <br/>
            <p>Regards,<br/>Morning Star High School</p>";
            }

            if (!string.IsNullOrEmpty(admission.Email))
            {
                await _emailService.SendEmailAsync(admission.Email, subject, body);
            }
           
            return RedirectToAction("Index");
        }
        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> UpdateStatusAjax(int id, string status)
        {
            var admission = await _admissionRepository.GetByIdAsync(id);
            if (admission == null)
            {
                return Json(new { success = false, message = "Admission not found!" });
            }

            // Update status in database
            await _admissionRepository.UpdateStatusAsync(id, status);

            // Prepare email content based on status
            string subject;
            string body;

            if (status == "Approved")
            {
                subject = "Admission Approved – Morning Star High School";
                body = $@"
        <h3>Congratulations {admission.ApplicantFullName}!</h3>
        <p>Your admission for class <strong>{admission.ApplyingForClass}</strong> has been <b>APPROVED</b>.</p>
        <p>Our administration will contact you soon.</p>
        <br/>
        <p>Regards,<br/>Morning Star High School</p>";
            }
            else // Rejected
            {
                subject = "Admission Status Update – Morning Star High School";
                body = $@"
        <p>Dear {admission.ApplicantFullName},</p>
        <p>Thank you for applying to Morning Star High School.</p>
        <p>We regret to inform you that your application was <b>REJECTED</b> at this time.</p>
        <p>We wish you the best for the future.</p>
        <br/>
        <p>Regards,<br/>Morning Star High School</p>";
            }

            // Send email
            try
            {
                if (!string.IsNullOrEmpty(admission.Email))
                {
                    await _emailService.SendEmailAsync(admission.Email, subject, body);
                }
            }
            catch
            {
                // If email fails, still return success but notify user
                return Json(new { success = true, message = $"Admission {status} but failed to send email." });
            }

            // Return success JSON message
            return Json(new { success = true, message = $"Admission {status} and email sent successfully." });
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
