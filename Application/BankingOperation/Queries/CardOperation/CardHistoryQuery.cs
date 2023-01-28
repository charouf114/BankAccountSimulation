using MediatR;

namespace Application.Authentification.Commands.Authenticate
{
    public record CardHistoryQuery : IRequest<CardHistoryResponse>
    {
        public Guid CardId { get; set; }
    }
}
