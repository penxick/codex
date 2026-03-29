namespace Testcord.Server.Application.Auth;

public interface IVerificationCodeGenerator
{
    string Generate();
}
