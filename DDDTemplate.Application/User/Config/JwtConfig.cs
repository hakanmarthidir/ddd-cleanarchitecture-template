using System;
namespace DDDTemplate.Application.User.Config
{
    public class JwtConfig
    {
        public string Secret { get; set; }
        public int? Duration { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}
