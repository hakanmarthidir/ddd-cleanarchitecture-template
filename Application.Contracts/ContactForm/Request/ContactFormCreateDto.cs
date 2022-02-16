namespace Application.Contracts.ContactForm.Request
{
    public class ContactFormCreateDto
    {
        public string Salutation { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Topic { get; set; }
        public string Message { get; set; }
    }
}
