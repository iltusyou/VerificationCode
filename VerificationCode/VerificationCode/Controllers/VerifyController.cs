using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using System;
using VerificationCode.Models;
using VerificationCode.Models.Request;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VerificationCode.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VerifyController : ControllerBase
    {
        private readonly WebContext _context;

        private const int EXPIRED_MINUTES = 5;

        public VerifyController(WebContext context)
        {
            _context = context;
        }

        [Route("SendCode")]
        [HttpPost]
        public void SendCode([FromBody] VertifyRequest request)
        {
            Models.Type type = !string.IsNullOrEmpty(request.Email) ?
               Models.Type.Email :
               Models.Type.SMS;

            string identity = (type == Models.Type.Email) ?
                request.Email :
                request.Phone;
          
            Token token = _context.Tokens.SingleOrDefault(x => x.Identity == identity && x.Usage == request.Usage && x.Type == type);
            if (token == null)
            {
                token = new Token();
                token.Identity = identity;
                token.Usage = request.Usage;
                token.Type = type;
            }
            
            token.Code = generateCode();
            token.CreateDateTime = DateTime.Now;
            token.ExpireDateTime = DateTime.Now.AddMinutes(EXPIRED_MINUTES);

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

        [Route("Register")]
        [HttpPost]
        public bool Register(RegisterRequest request)
        {
            bool verifyResult = verify(request.Email, request.Code, request.Usage);
            return verifyResult;

        }

        private bool verify(string identity, string code, Usage usage)
        {
            return _context.Tokens.Any(p =>
            p.Code == code &&
            p.Identity == identity &&
            p.Usage == usage &&
            p.ExpireDateTime > DateTime.Now);
        }

        private string generateCode()
        {
            Random random = new Random();
            return random.Next(100000, 1000000).ToString();
        }
    }
}
