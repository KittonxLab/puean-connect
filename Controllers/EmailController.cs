using Microsoft.AspNetCore.Mvc;
using PuanConnect.Interfaces;

namespace EmailController.Controllers
{
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("send-reminder")]
        public async Task<IActionResult> SendEventReminder(string recipientEmail)
        {
            try
            {
                DateTime eventTime = DateTime.UtcNow.ToLocalTime();
                await _emailService.SendNotification(recipientEmail, "Hello");
                return Ok("Email sent successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to send email: {ex.Message}");
            }
        }
    }
}
