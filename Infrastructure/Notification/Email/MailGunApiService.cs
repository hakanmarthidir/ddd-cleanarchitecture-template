using Application.Abstraction.Interfaces;
using Infrastructure.Notification.Config;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text;

namespace Infrastructure.Notification.Email
{
    public class MailGunApiService : IEmailService
    {

        private readonly MailGunConfig _mailGunConfig;
        public MailGunApiService(IOptionsMonitor<MailGunConfig> mailGunConfig)
        {
            this._mailGunConfig = mailGunConfig.CurrentValue;
        }

        public async Task SendEmailAsync(string receiver, string subject, string content, CancellationToken token = default)
        {
            using (var httpClient = new HttpClient())
            {
                var authToken = Encoding.ASCII.GetBytes($"api:{_mailGunConfig.Password}");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authToken));
                var formContent = new FormUrlEncodedContent(new Dictionary<string, string> {
                                { "from", $"{_mailGunConfig.DisplayName} <{_mailGunConfig.Sender}>" },
                                { "to", receiver },
                                { "subject", subject },
                                { "html", content }
                                });
                var result = await httpClient.PostAsync($"{_mailGunConfig.SmtpServer}{_mailGunConfig.UserName}/messages", formContent).ConfigureAwait(false);
                result.EnsureSuccessStatusCode();
            }
        }
    }
}
