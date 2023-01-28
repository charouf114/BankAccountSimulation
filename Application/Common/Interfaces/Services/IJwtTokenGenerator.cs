using Domain.Entities;

namespace Application.Common.Interfaces.Services
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(Card card);
    }
}
