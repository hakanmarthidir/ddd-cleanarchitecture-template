using System;
using System.ComponentModel.DataAnnotations;
using DDDTemplate.Application.Contracts.Shared;

namespace DDDTemplate.Application.Contracts.ContactForm.Request
{
    public class ContactFormCreateRequest : IDto
    {
        [Required]
        public string Salutation { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Topic { get; set; }
        [Required]
        public string Message { get; set; }
    }
}
