namespace DDDTemplate.Infrastructure.Notification.Config
{
    public class MailGunConfig
    {
        public string Sender { get; set; }
        public string SmtpServer { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string DisplayName { get; set; }
    }
}
