using Application.Abstraction.Interfaces;
using Domain.Entities.UserAggregate.Events;
using MediatR;

namespace Application.User.Handlers
{
    public class UserCreatedEventHandler : INotificationHandler<UserCreatedEvent>
    {
        private readonly IEmailService _mailGunService;
        private readonly ITemplateService _templateService;
        private readonly ILogService<UserCreatedEventHandler> _logger;

        public UserCreatedEventHandler(IEmailService mailGunService, ITemplateService templateService, ILogService<UserCreatedEventHandler> logger)
        {
            _mailGunService = mailGunService;
            _templateService = templateService;
            _logger = logger;
        }

        public async Task Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
        {
            await SendActivationCodeEmailAsync(notification.CreatedUser, "Welcome!");
        }

        private async Task SendActivationCodeEmailAsync(Domain.Entities.UserAggregate.User registeredUser, string emailTitle)
        {
            var activationEmailBody = await this._templateService.GetActivationTemplateAsync(
                registeredUser.Id.ToString(),
                registeredUser.FirstName,
                registeredUser.LastName,
                registeredUser.Activation.ActivationCode).ConfigureAwait(false);

            this._logger.LogInformation($"Activation Email was sent to {registeredUser.Id.ToString()}.");

            //await this._mailGunService.SendEmailAsync(registeredUser.Email, emailTitle, activationEmailBody).ConfigureAwait(false);
        }
    }
}
