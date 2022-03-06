using Application.Abstraction.Interfaces;
using Ardalis.GuardClauses;
using Infrastructure.Notification.Config;
using Microsoft.Extensions.Options;
using RestSharp;
using RestSharp.Authenticators;

namespace Infrastructure.Notification.Email
{
    public class MailGunApiService : IEmailService
    {

        private readonly MailGunConfig _mailGunConfig;
        public MailGunApiService(IOptionsMonitor<MailGunConfig> mailGunConfig)
        {
            this._mailGunConfig = mailGunConfig.CurrentValue ?? throw new ArgumentNullException(nameof(mailGunConfig));
        }

        public async Task<RestResponse> SendEmailAsync(string receiver, string subject, string content, string? attachmentFilePath = null, CancellationToken token = default)
        {

            var to = Guard.Against.NullOrWhiteSpace(receiver, nameof(receiver), "Receiver could not be null.");
            var topic = Guard.Against.NullOrWhiteSpace(subject, nameof(subject), "Subject could not be null.");
            var html = Guard.Against.NullOrWhiteSpace(content, nameof(content), "Content could not be null.");

            RestClient client = new RestClient(_mailGunConfig.BaseUrl);
            client.Authenticator = new HttpBasicAuthenticator("api", _mailGunConfig.ApiKey);

            var request = new RestRequest();
            request.AddParameter("domain", _mailGunConfig.Domain, ParameterType.UrlSegment);
            request.Resource = "{domain}/messages";
            request.AddParameter("to", to);
            request.AddParameter("from", $"{_mailGunConfig.Domain} <{_mailGunConfig.Sender}>");
            request.AddParameter("subject", topic);
            request.AddParameter("html", html);

            if (!string.IsNullOrWhiteSpace(attachmentFilePath))
            {
                request.AddFile("attachment", attachmentFilePath, "multipart/form-data");
            }

            request.Method = Method.Post;
            var result = await client.ExecuteAsync(request, token).ConfigureAwait(false);
            return result;


        }
    }
}
