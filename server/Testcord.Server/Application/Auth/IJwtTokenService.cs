using Testcord.Server.Domain.Entities;
using Testcord.Shared.Contracts.Auth;

namespace Testcord.Server.Application.Auth;

public interface IJwtTokenService
{
    SessionTokensDto CreateTokens(User user);
}
