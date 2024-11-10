using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using System;
using VerificationCode.Models;
using VerificationCode.Models.Request;
using VerificationCode.Service;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VerificationCode.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VerifyController : ControllerBase
    {
        private readonly TokenService tokenService;

        private const int EXPIRED_MINUTES = 5;

        public VerifyController(WebContext context)
        {
            tokenService = new TokenService(context);
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

            Token token = tokenService.GetToken(identity, request.Usage, type);


            token.Code = generateCode();
            token.CreateDateTime = DateTime.Now;
            token.ExpireDateTime = DateTime.Now.AddMinutes(EXPIRED_MINUTES);

            tokenService.UpdateToken(token);
        }

        [Route("Register")]
        [HttpPost]
        public bool Register(RegisterRequest request)
        {
            bool verifyResult = tokenService.Verify(request.Email, request.Code, request.Usage);
            return verifyResult;

        }

        private string generateCode()
        {
            Random random = new Random();
            return random.Next(100000, 1000000).ToString();
        }
    }
}
