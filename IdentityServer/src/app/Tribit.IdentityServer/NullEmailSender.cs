using Microsoft.AspNetCore.Identity.UI.Services;

namespace Tribit.IdentityServer.App
{
    class NullEmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage) => Task.CompletedTask;
    }
}