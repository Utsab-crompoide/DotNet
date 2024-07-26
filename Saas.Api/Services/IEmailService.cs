using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Saas.Api.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string fromEmail, string subject, string body);
    }
}