using System;
using System.IO;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using DDDTemplate.Application.Abstraction.External;
using DDDTemplate.Infrastructure.Extensions;
using DDDTemplate.Infrastructure.Notification.Config;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;

namespace DDDTemplate.Infrastructure.Notification.Template
{
    public class TemplateService : ITemplateService
    {
        private readonly TemplateConfig _templateConfig;
        private IWebHostEnvironment _hostingEnvironment;

        public TemplateService(IOptionsMonitor<TemplateConfig> templateConfig, IWebHostEnvironment hostingEnvironment)
        {
            this._templateConfig = templateConfig.CurrentValue;
            this._hostingEnvironment = hostingEnvironment;
        }

        private async Task<string> GetEmailBodyAsync(string templateName)
        {
            string emailBody = string.Empty;
            var templatePath = $"{this._hostingEnvironment.ContentRootPath}{templateName}";

            using (StreamReader reader = new StreamReader(templatePath))
            {
                emailBody = await reader.ReadToEndAsync().ConfigureAwait(false);
            }

            Guard.Against.NullOrWhiteSpace(emailBody, nameof(emailBody), "Template could not be found.");
            return emailBody;
        }

        public async Task<string> GetActivationTemplateAsync(string userId, string userFirstName, string userLastName, string activationId)
        {
            string emailBody = await this.GetEmailBodyAsync(_templateConfig.ActivationTemplateName).ConfigureAwait(false);

            var url = $"{this._templateConfig.ActivationBaseUrl}{userId}/{activationId}";
            emailBody = emailBody.Replace("{{USERNAME}}", userFirstName + " " + userLastName);
            emailBody = emailBody.Replace("{{URL}}", url);
            emailBody = emailBody.Replace("{{PROJECTNAME}}", this._templateConfig.ProjectName);
            return emailBody;
        }

        public async Task<string> GetContactFormTemplateAsync(string salutation, string firstname, string lastname, string email, string topic, string message)
        {
            string emailBody = await this.GetEmailBodyAsync(_templateConfig.ContactFormTemplateName).ConfigureAwait(false);

            emailBody = emailBody.Replace("{{SALUTATION}}", salutation.CleanHtml());
            emailBody = emailBody.Replace("{{FIRSTNAME}}", firstname.CleanHtml());
            emailBody = emailBody.Replace("{{LASTNAME}}", lastname.CleanHtml());
            emailBody = emailBody.Replace("{{EMAIL}}", email.CleanHtml());
            emailBody = emailBody.Replace("{{TOPIC}}", topic.CleanHtml());
            emailBody = emailBody.Replace("{{MESSAGE}}", message.CleanHtml());

            return emailBody;
        }

        public async Task<string> GetForgotPasswordTemplateAsync(string userFirstName, string token)
        {
            string emailBody = await this.GetEmailBodyAsync(_templateConfig.ForgotPasswordTemplateName).ConfigureAwait(false);
            var url = $"{this._templateConfig.ForgotPasswordBaseUrl}{token}";

            emailBody = emailBody.Replace("{{FIRSTNAME}}", userFirstName);
            emailBody = emailBody.Replace("{{URL}}", url);

            return emailBody;
        }

    }
}
