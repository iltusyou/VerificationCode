namespace VerificationCode.Models.Request
{
    public class RegisterRequest
    {
        public string Email { get; set; }
        public Usage Usage { get; set; }
        public string Code { get; set; }
    }
}
