using Application.Abstraction.Interfaces;
using Domain.Entities.UserAggregate.Events;
using MediatR;

namespace Application.User.Handlers
{
    public class UserActivationCodeDemandedEventHandler : INotificationHandler<UserActivationCodeDemandedEvent>
    {
        private readonly IEmailService _mailGunService;
        private readonly ITemplateService _templateService;
        private readonly ILogService<UserCreatedEventHandler> _logger;

        public UserActivationCodeDemandedEventHandler(IEmailService mailGunService, ITemplateService templateService, ILogService<UserCreatedEventHandler> logger)
        {
            _mailGunService = mailGunService;
            _templateService = templateService;
            _logger = logger;
        }

        public async Task Handle(UserActivationCodeDemandedEvent notification, CancellationToken cancellationToken)
        {
            await SendActivationCodeEmailAsync(notification.DemandedUser, "RE:Activation Code");
        }

        private async Task SendActivationCodeEmailAsync(Domain.Entities.UserAggregate.User demaindingUser, string emailTitle)
        {
            var activationEmailBody = await this._templateService.GetActivationTemplateAsync(
                demaindingUser.Id.ToString(),
                demaindingUser.FirstName,
                demaindingUser.LastName,
                demaindingUser.Activation.ActivationCode).ConfigureAwait(false);

            this._logger.LogInformation($"Activation Email was re-sent to {demaindingUser.Id.ToString()}.");

            //await this._mailGunService.SendEmailAsync(demaindingUser.Email, emailTitle, activationEmailBody).ConfigureAwait(false);
        }
    }
}
