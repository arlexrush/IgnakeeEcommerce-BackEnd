using Ecommerce.Application.Contracts.Infrastructure;
using Ecommerce.Application.Exceptions;
using Ecommerce.Application.Models.Email;
using Ecommerce.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Features.Auths.Users.Commands.SendPassword
{
    public class SendPasswordCommandHandler : IRequestHandler<SendPasswordCommand, string>
    {
        private readonly IEmailService? _emailService;
        private readonly UserManager<User>? _userManager;

        public SendPasswordCommandHandler(IEmailService? emailService, UserManager<User>? userManager)
        {
            _emailService = emailService;
            _userManager = userManager;
        }

        public async Task<string> Handle(SendPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager!.FindByEmailAsync(request.Email!);
            if (user == null)
            {
                throw new BadRequestException("This User Not Exist");
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var plainTextBytes = Encoding.UTF8.GetBytes(token);
            token = Convert.ToBase64String(plainTextBytes);

            var emailMessage = new EmailMessage
            {
                Body = "By reset Password, click here",
                Subject = "Password Reset",
                To = request.Email,
            };

            var result = await _emailService!.SendEmail(emailMessage, token);

            if (!result)
            {
                throw new Exception("Can´t send Email");
            }

            var response = $"Email {request.Email} was sent successfully";

            return response;
        }
    }
}
