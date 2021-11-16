﻿using System.Threading.Tasks;

namespace DDDTemplate.Infrastructure.Notification.Template
{
    public interface ITemplateService
    {
        Task<string> GetActivationTemplateAsync(string userId, string userFirstName, string userLastName, string activationId);
        Task<string> GetContactFormTemplateAsync(string salutation, string firstname, string lastname, string email, string topic, string message);
        Task<string> GetForgotPasswordTemplateAsync(string userFirstName, string token);
    }
}
