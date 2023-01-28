using Application.Authentification.Commands.Authenticate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankAccount.Controllers
{
    [Route("api/auth")]
    [AllowAnonymous]

    public class AuthController : BaseApiController
    {
        [HttpPost("Authenticate")]
        public async Task<IActionResult> Authenticate(AuthenticateCommand authenticateCommand)
        {
            var result = await Mediator.Send(authenticateCommand);
            return Ok(result);
        }
    }
}