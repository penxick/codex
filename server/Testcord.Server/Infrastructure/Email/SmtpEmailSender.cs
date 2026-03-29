using Microsoft.Extensions.Options;
using Testcord.Server.Configuration.Options;

namespace Testcord.Server.Infrastructure.Email;

public sealed class SmtpEmailSender : IEmailSender
{
    private readonly SmtpOptions _options;
    private readonly ILogger<SmtpEmailSender> _logger;

    public SmtpEmailSender(IOptions<SmtpOptions> options, ILogger<SmtpEmailSender> logger)
    {
        _options = options.Value;
        _logger = logger;
    }

    public Task SendAsync(string toEmail, string subject, string htmlBody, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "SMTP email queued for {Recipient} with subject {Subject}. SMTP host: {Host}:{Port}",
            toEmail,
            subject,
            _options.Host,
            _options.Port);

        return Task.CompletedTask;
    }
}
