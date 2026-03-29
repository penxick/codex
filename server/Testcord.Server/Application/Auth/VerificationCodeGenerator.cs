using System.Security.Cryptography;

namespace Testcord.Server.Application.Auth;

public sealed class VerificationCodeGenerator : IVerificationCodeGenerator
{
    public string Generate()
    {
        var value = RandomNumberGenerator.GetInt32(100000, 999999);
        return value.ToString();
    }
}
