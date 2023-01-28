using Application.Common.Interfaces.Persistance;
using Application.Common.Interfaces.Services;
using Domain.Entities;
using MediatR;
using System.Security.Cryptography;
using System.Text;

namespace Application.Authentification.Commands.Authenticate
{
    public class AuthenticateCommandHandler : IRequestHandler<AuthenticateCommand, AuthenticateResponse>
    {
        private readonly ICardRepository _cardRepository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthenticateCommandHandler(ICardRepository cardRepository, IJwtTokenGenerator jwtTokenGenerator)
        {
            _cardRepository = cardRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<AuthenticateResponse> Handle(AuthenticateCommand request, CancellationToken cancellationToken)
        {
            if (_cardRepository.GetCardByNumber(request.CardNumber) is not Card card)
            {
                return new AuthenticateResponse() { IsSuccess = false, Message = "Card Not Found" };
            }

            if (card.GetCardStatus() != Domain.Enum.CardState.Enabled)
            {
                return new AuthenticateResponse() { IsSuccess = false, Message = "Card Not Enabled" };
            }

            using (var hmac = new HMACSHA512(card.Salt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(request.SecretCode));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != card.SecretCode[i])
                    {
                        return new AuthenticateResponse() { IsSuccess = false, Message = "Wrong Code" };
                    }
                }
            }

            return new AuthenticateResponse()
            {
                IsSuccess = true,
                AccessToken = _jwtTokenGenerator.GenerateToken(card),
                Message = "Success Authentification"
            };
        }
    }
}
