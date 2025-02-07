using Docsm.Options;
using Docsm.Services.Interfaces;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Identity;
using Docsm.Models;
using Docsm.Exceptions;

namespace Docsm.Services.Implements
{
    public class EmailService : IEmailService
    {
        readonly SmtpClient _client;
        readonly MailAddress _from;
        readonly HttpContext Context;
        readonly UserManager<User> _userManager;
        public EmailService(IOptions<SmtpOptions> opts, IHttpContextAccessor acc,UserManager<User> userManager)
        {
            var opt = opts.Value;
            _client = new(opt.Host, opt.Port);
            _client.EnableSsl = true;
            _client.Credentials = new NetworkCredential(opt.Sender, opt.Password);
            _from = new MailAddress(opt.Sender, "Docsm");
            Context = acc.HttpContext;
            _userManager = userManager;
        }

        public async Task SendConfirmEmailAsync(string email, string token)
        {
            string subject = "Emailinizi Təsdiqləyin";
            string body = $"<h3>Salam!</h3><p>Emailinizi təsdiqləmək üçün aşağıdakı kodu istifadə edin:</p>" +
                  $"<p><strong>{token}</strong></p>" +
                  $"<p>Tokeni sistemə daxil edib təsdiqləyin.</p>";

            await SendEmailAsync(email, subject, body); 
        }

        public async Task SendResetPasswordAsync(string email)
        {
            var user=await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                throw new NotFoundException<User>();
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            string subject = "Şifrənizi Sıfırlamaq Üçün Token";
            string body = $"<h3>Salam!</h3><p>Şifrənizi sıfırlamaq üçün aşağıdakı tokeni istifadə edin:</p>" +
                          $"<p><strong>{token}</strong></p>";
            await SendEmailAsync (email, subject, body);

        }


        public async Task SendEmailAsync(string email, string subject, string body)
        {
            MailAddress to = new(email);
            MailMessage message = new MailMessage(_from,to);
            message.Body = body;
            message .Subject = subject;
            message.IsBodyHtml = true;
            await  _client.SendMailAsync (message);
        }

        
    }
}
