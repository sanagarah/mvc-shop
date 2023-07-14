using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Identity.UI.Services;
using Newtonsoft.Json.Linq;

namespace mvc_shop.Utility
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;


        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Execute(email, subject, htmlMessage);
        }

    public async Task Execute(string email, string subject, string body)
    {
            MailjetClient client = new MailjetClient(_configuration.GetValue<string>("ApiKey"), _configuration.GetValue<string>("SecretKey"));
            MailjetRequest request = new MailjetRequest
            {
                Resource = Send.Resource,
            }
             .Property(Send.Messages, new JArray {
     new JObject {
      {
       "From",
       new JObject {
        {"Email", "hi@younes.ninja"},
        {"Name", "Younes"}
       }
      }, {
       "To",
       new JArray {
        new JObject {
         {
          "Email",
          email
         }, {
          "Name",
          "DotNetMastery"
         }
        }
       }
      }, {
       "Subject",
       subject
      }, {
       "HTMLPart",
       body
      }
     }
             });
            await client.PostAsync(request);
        }
    }
}
