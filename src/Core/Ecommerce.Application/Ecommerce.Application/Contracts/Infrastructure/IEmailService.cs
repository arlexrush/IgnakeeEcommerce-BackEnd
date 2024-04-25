using Ecommerce.Application.Models.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Contracts.Infrastructure
{
    public interface IEmailService
    {
        Task<bool> SendEmail(EmailMessage email, string token);
    }
}
