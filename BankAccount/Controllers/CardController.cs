using Application.Authentification.Commands.Authenticate;
using Domain.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BankAccount.Controllers
{
    [Route("api/card")]
    [Authorize]

    public class CardController : BaseApiController
    {
        [HttpPost("Deposit")]
        public async Task<IActionResult> DepositMoney(TransactionInput transactionInput)
        {
            var addTransactionCommand = new AddTransactionCommand()
            {
                CardId = transactionInput.CardId,
                Amount = transactionInput.Amount,
                Currency = transactionInput.Currency,
                TransactionType = Domain.Enum.TransactionType.Credit
            };
            var result = await Mediator.Send(addTransactionCommand);
            return Ok(result);
        }

        [HttpPost("WithDrawal")]
        public async Task<IActionResult> WithDrawalMoney(TransactionInput transactionInput)
        {
            var addTransactionCommand = new AddTransactionCommand()
            {
                CardId = transactionInput.CardId,
                Amount = -1 * transactionInput.Amount,
                Currency = transactionInput.Currency,
                TransactionType = Domain.Enum.TransactionType.Debit
            };

            var result = await Mediator.Send(addTransactionCommand);
            return Ok(result);
        }

        [HttpGet("History")]
        public async Task<IActionResult> GetHistory()
        {
            if (HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier) is not null)
            {
                var cardId = new Guid(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
                var cardHistoryQuery = new CardHistoryQuery() { CardId = cardId };
                var result = await Mediator.Send(cardHistoryQuery);
                return Ok(result);
            }
            else
            {
                return Problem("Unable to Identify the Card");
            }
        }
    }
}