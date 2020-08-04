using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RunningDinner.Extensions;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RunningDinner.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ApiController : Controller
    {
        private readonly IEmailSender _emailSender;
        private IConfiguration Configuration { get; set; }

        public ApiController(IConfiguration configuration, IEmailSender emailSender)
        {
            _emailSender = emailSender;
            Configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> SendMail([FromBody]string body)
        {
            var mailBody = JsonConvert.DeserializeObject<MailBody>(body);
            // Send the email
            string apiKey = Configuration?.GetEmailSettings("apiKey");
            string apiSecret = Configuration?.GetEmailSettings("apiSecret");
            bool success = await _emailSender.SendMailjetAsync(apiKey, apiSecret, 1112074, mailBody.Name, mailBody.Email, mailBody.Message).ConfigureAwait(false);
            if (success)
            {
                return Ok();
            }

            return Conflict();
        }
    }

    public class MailBody
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
    }
}
