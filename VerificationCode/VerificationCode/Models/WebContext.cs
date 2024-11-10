using Microsoft.EntityFrameworkCore;

namespace VerificationCode.Models
{
    public class WebContext : DbContext
    {
        public WebContext(DbContextOptions<WebContext> options) : base(options)
        {
        }

        public DbSet<Token> Tokens { get; set; }
    }
}
