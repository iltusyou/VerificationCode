namespace VerificationCode.Models
{
    public class Token
    {
        public int Id { get; set; }

        public string Identity {  get; set; }

        public string Code { get; set; }

        public Type Type { get; set; }

        public Usage Usage { get; set; }

        public DateTime ExpireDateTime { get; set; }

        public DateTime CreateDateTime { get; set; }
    }

    public enum Type
    {
        Email = 1,
        SMS = 2
    }

    public enum Usage
    {
        Register = 1,
        ChangePassword = 2,
        ForgetPassword = 3
    }
}
