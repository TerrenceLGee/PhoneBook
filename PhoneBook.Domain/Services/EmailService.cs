using PhoneBook.Core.Results;
using PhoneBook.Core.Models;
using PhoneBook.Domain.Interfaces;
using PhoneBook.Core.Extensions;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using System.Net.Sockets;
using MailKit;

namespace PhoneBook.Domain.Services;

public class EmailService : IEmailService
{
    private readonly EmailSettings _emailSettings;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IOptions<EmailSettings> emailSettings, ILogger<EmailService> logger)
    {
        _emailSettings = emailSettings.Value;
        _logger = logger;
    }

    public async Task<Result> SendEmailAsync(string recipientEmail, string recipientName, string? subject, string body)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(recipientEmail))
                return _logger.LogErrorAndReturnFail("There must have a recipient email given in order to send emails");

            if (string.IsNullOrWhiteSpace(recipientName))
                return _logger.LogErrorAndReturnFail("Recipient name should not be null or blank");


            if (string.IsNullOrWhiteSpace(body))
                return _logger.LogErrorAndReturnFail("Email must have a body");

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.SenderEmail));
            message.To.Add(new MailboxAddress(recipientName, recipientEmail));
            message.Subject = subject;
            message.Body = new TextPart("plain") { Text = body };

            using var client = new SmtpClient();
            await client.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.SmtpPort, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_emailSettings.SenderEmail, _emailSettings.AppPassword);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);

            return Result.Ok();
        }
        catch (ArgumentNullException ex)
        {
            return _logger.LogErrorAndReturnFail($"Argument cannot be null: {ex.Message}");
        }
        catch (AuthenticationException ex)
        {
            return _logger.LogErrorAndReturnFail($"Email authentication failed: {ex}");
        }
        catch (SmtpCommandException ex)
        {
            return _logger.LogErrorAndReturnFail($"There has been a Smtp command error: {ex.Message}");
        }
        catch (SmtpProtocolException ex)
        {
            return _logger.LogErrorAndReturnFail($"There has been a Smtp protocol error: {ex.Message}");
        }
        catch (SocketException ex)
        {
            return _logger.LogErrorAndReturnFail($"There has been a Socket error: {ex.Message}");
        }
        catch (ProtocolException ex)
        {
            return _logger.LogErrorAndReturnFail($"There has been a Protocol error: {ex.Message}");
        }
        catch (InvalidOperationException ex)
        {
            return _logger.LogErrorAndReturnFail($"Invalid operation performed: {ex.Message}");
        }
        catch (Exception ex)
        {
            return _logger.LogErrorAndReturnFail($"An unexpected error has occurred: {ex.Message}");
        }
    }
}

