namespace DDDTemplate.Infrastructure.Notification.Config
{
    public class TemplateConfig
    {
        public string ProjectName { get; set; }
        public string ActivationBaseUrl { get; set; }
        public string ActivationTemplateName { get; set; }
        public string ActivationEmailTitle { get; set; }
        public string ContactFormTemplateName { get; set; }
        public string ContactFormReceiver { get; set; }
        public string ContactFormEmailTitle { get; set; }
        public string ForgotPasswordBaseUrl { get; set; }
        public string ForgotPasswordTemplateName { get; set; }
        public string ForgotPasswordEmailTitle { get; set; }
    }
}
