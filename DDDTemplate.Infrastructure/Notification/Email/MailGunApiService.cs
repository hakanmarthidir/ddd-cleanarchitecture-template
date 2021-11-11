using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using DDDTemplate.Infrastructure.Notification.Config;

namespace DDDTemplate.Infrastructure.Notification.Email
{
    public class MailGunApiService : IMailGunApiService
    {

        private readonly MailGunConfig _mailGunConfig;
        public MailGunApiService(IOptionsMonitor<MailGunConfig> mailGunConfig)
        {
            this._mailGunConfig = mailGunConfig.CurrentValue;
        }

        public async Task SendWithMailgunApiAsync(string receiver, string subject, string content, CancellationToken token = default)
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
