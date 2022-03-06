namespace Infrastructure.Notification.Config
{
    public class MailGunConfig
    {
        public string BaseUrl { get; set; }
        public string Sender { get; set; }
        public string Domain { get; set; }
        public string ApiKey { get; set; }
        public string DisplayName { get; set; }        
    }
}
