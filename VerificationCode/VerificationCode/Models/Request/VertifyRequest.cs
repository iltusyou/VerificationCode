namespace VerificationCode.Models.Request
{
    public class VertifyRequest
    {
        public string Phone { get; set; }

        public string Email { get; set; }       
        
        public Usage Usage { get; set; }
    }
}
