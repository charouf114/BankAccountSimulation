using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BankAccount.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    public class BaseApiController : ControllerBase
    {
        private IMediator _mediator = null!;
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<IMediator>();
    }
}
