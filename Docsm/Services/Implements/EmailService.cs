using Docsm.Options;
using Docsm.Services.Interfaces;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Identity;
using Docsm.Models;
using Docsm.Exceptions;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Cryptography;

namespace Docsm.Services.Implements
{
    public class EmailService : IEmailService
    {
        readonly SmtpClient _client;
        readonly MailAddress _from;
        readonly HttpContext Context;
        readonly UserManager<User> _userManager;
        readonly IMemoryCache _cache;
        public EmailService(IOptions<SmtpOptions> opts, IHttpContextAccessor acc,UserManager<User> userManager,IMemoryCache cache)
        {
            var opt = opts.Value;
            _client = new(opt.Host, opt.Port);
            _client.EnableSsl = true;
            _client.Credentials = new NetworkCredential(opt.Sender, opt.Password);
            _from = new MailAddress(opt.Sender, "Docsm");
            Context = acc.HttpContext;
            _userManager = userManager;
            _cache=cache;
        }

        public async Task SendConfirmEmailAsync(string email)
        {
            if (_cache.TryGetValue(email, out int existingCode))
            {
                throw new ExistException("Emaile artıq kod göndərilib");
            }

            int code = GenerateSecureCode();

            string subject = "Emailinizi Təsdiqləyin";
            string body = $"<h3>Salam!</h3><p>Emailinizi təsdiqləmək üçün aşağıdakı kodu istifadə edin:</p>" +
                          $"<p><strong>{code}</strong></p>" +
                  $"<p>Codu sistemə daxil edib təsdiqləyin.</p>";

            await SendEmailAsync(email, subject, body);
            _cache.Set(email, code, TimeSpan.FromMinutes(5));
        }

        public async Task SendResetPasswordAsync(string email)
        {
            var user=await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                throw new NotFoundException<User>();
            }
            if (_cache.TryGetValue(email, out int existingCode))
            {
                throw new ExistException("Şifrə sıfırlama kodu artıq göndərilib");
            }
            int code = GenerateSecureCode();
           
            string subject = "Şifrənizi Sıfırlamaq Üçün Kod";
            string body = $"<h3>Salam!</h3><p>Şifrənizi sıfırlamaq üçün aşağıdakı kodu istifadə edin:</p>" +
                          $"<p><strong>{code}</strong></p>";
            await SendEmailAsync (email, subject, body);
            _cache.Set(email, code, TimeSpan.FromMinutes(5));

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
        private int GenerateSecureCode()
        {
            return RandomNumberGenerator.GetInt32(100000, 999999);
        }



    }
}
