namespace Application.Contracts.Auth.Request
{
    public class ResetPasswordDto
    {
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
        public string Token { get; set; }
    }
}
