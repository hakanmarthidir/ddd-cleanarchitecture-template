using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Abstraction.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string receiver, string subject, string content, CancellationToken token = default(CancellationToken));
    }
}
