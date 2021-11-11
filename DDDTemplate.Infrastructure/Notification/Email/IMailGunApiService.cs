using System.Threading;
using System.Threading.Tasks;

namespace DDDTemplate.Infrastructure.Notification.Email
{
    public interface IMailGunApiService
    {
        Task SendWithMailgunApiAsync(string receiver, string subject, string content, CancellationToken token = default(CancellationToken));
    }
}
