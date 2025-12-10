using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SchoolWebsite1.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace SchoolWebsite1.Data.Repositories
{
    public class AdmissionRepository : IAdmissionRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        private readonly IConfiguration _configuration;

        public AdmissionRepository(IDbConnectionFactory dbConnectionFactory,IConfiguration configuration)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _configuration = configuration;
        }

        public async Task<IEnumerable<Admission>> GetAllAsync()
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            string sql = "SELECT * FROM Admissions";
            return await connection.QueryAsync<Admission>(sql);
        }

        public async Task<Admission> GetByIdAsync(int id)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            string sql = "SELECT * FROM Admissions WHERE Id = @Id";
            return await connection.QueryFirstOrDefaultAsync<Admission>(sql, new { Id = id });
        }

        public async Task<int> AddAsync(Admission admission)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            string sql = @"INSERT INTO Admissions 
                           (ApplicantFullName, ApplyingForClass, DateOfBirth, Gender, ParentGuardianName, ParentContactNumber, Email, AlternateContact, Address, AdditionalNotes, Status, SubmittedAt) 
                           VALUES 
                           (@ApplicantFullName, @ApplyingForClass, @DateOfBirth, @Gender, @ParentGuardianName, @ParentContactNumber, @Email, @AlternateContact, @Address, @AdditionalNotes, @Status, @SubmittedAt)";
            return await connection.ExecuteAsync(sql, admission);
        }

        public async Task<int> UpdateAsync(Admission admission)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            string sql = @"UPDATE Admissions SET 
                           ApplicantFullName = @ApplicantFullName,
                           ApplyingForClass = @ApplyingForClass,
                           DateOfBirth = @DateOfBirth,
                           Gender = @Gender,
                           ParentGuardianName = @ParentGuardianName,
                           ParentContactNumber = @ParentContactNumber,
                           Email = @Email,
                           AlternateContact = @AlternateContact,
                           Address = @Address,
                           AdditionalNotes = @AdditionalNotes,
                           Status = @Status
                           WHERE Id = @Id";
            return await connection.ExecuteAsync(sql, admission);
        }

        public async Task<int> DeleteAsync(int id)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            string sql = "DELETE FROM Admissions WHERE Id = @Id";
            return await connection.ExecuteAsync(sql, new { Id = id });
        }

        public async Task<int> UpdateStatusAsync(int id, string status)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            string sql = "UPDATE Admissions SET Status = @Status WHERE Id = @Id";

            var result = await connection.ExecuteAsync(sql, new { Id = id, Status = status });

            // Load admission data to send email
            var admission = await GetByIdAsync(id);

            if (!string.IsNullOrEmpty(admission?.Email))
            {
                await SendEmailAsync(admission.Email, admission.ApplicantFullName, status);
            }

            return result;
        }

        private async Task SendEmailAsync(string toEmail, string applicantName, string status)
        {
            try
            {
                var smtpHost = _configuration["EmailSettings:SmtpHost"];
                var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"]);
                var smtpUser = _configuration["EmailSettings:SmtpUser"];
                var smtpPass = _configuration["EmailSettings:SmtpPass"];
                var fromEmail = _configuration["EmailSettings:FromEmail"];

                var message = new MailMessage();
                message.From = new MailAddress(fromEmail, "Morning Star High School");
                message.To.Add(toEmail);
                message.Subject = "Admission Status Update";
                message.Body = $"Dear {applicantName},\n\nYour admission has been {status}.\n\nRegards,\nMorning Star High School";
                message.IsBodyHtml = false;

                using var client = new SmtpClient(smtpHost, smtpPort)
                {
                    Credentials = new NetworkCredential(smtpUser, smtpPass),
                    EnableSsl = true
                };

                await client.SendMailAsync(message);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Email sending failed: {ex.Message}");
            }
        }
    }
}

