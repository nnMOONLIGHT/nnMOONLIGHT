using StyleAI.Core.Entities.Interfaces;

namespace StyleAI.Core.Interfaces;

public interface IJwtService
{
    (string Token, DateTime ExpiresAt) GenerateToken(User user);
}