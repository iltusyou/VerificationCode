using Azure.Core;
using System.Diagnostics.Eventing.Reader;
using VerificationCode.Models;
using Type = VerificationCode.Models.Type;

namespace VerificationCode.Service
{
    public class TokenService
    {
        private readonly WebContext _context;
        public TokenService(WebContext context) 
        {
            _context = context;
        }

        public Token GetToken(string identity, Usage usage, Type type)
        {
            Token token = _context.Tokens.SingleOrDefault(x => x.Identity == identity && x.Usage == usage && x.Type == type);
            if (token == null)
            {
                token = new Token();
                token.Identity = identity;
                token.Usage = usage;
                token.Type = type;
            }
            return token;
        }

        public void UpdateToken(Token token)
        {
            if (token.Id == 0)
            {
                _context.Tokens.Add(token);
            }
            else
            {
                _context.Tokens.Update(token);
            }

            _context.SaveChanges();
        }

        public bool Verify(string identity, string code, Usage usage)
        {
            return _context.Tokens.Any(p =>
            p.Code == code &&
            p.Identity == identity &&
            p.Usage == usage &&
            p.ExpireDateTime > DateTime.Now);
        }
    }
}
