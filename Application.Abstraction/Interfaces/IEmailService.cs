using RestSharp;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Abstraction.Interfaces
{
    public interface IEmailService
    {
        Task<RestResponse> SendEmailAsync(string receiver, string subject, string content, string? attachmentFilePath=null, CancellationToken token = default(CancellationToken));
    }
}
