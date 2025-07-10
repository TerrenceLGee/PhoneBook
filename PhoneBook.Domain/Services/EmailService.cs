using PhoneBook.Core.Results;
using PhoneBook.Core.Models;
using PhoneBook.Domain.Interfaces;
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
    private string _errorMessage = string.Empty;

    public EmailService(IOptions<EmailSettings> emailSettings, ILogger<EmailService> logger)
    {
        _emailSettings = emailSettings.Value;
        _logger = logger;
    }

    public async Task<Result> SendEmailAsync(string recipientEmail, string recipientName, string subject, string body)
    {
        try
        {
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
            _errorMessage = $"Argument cannot be null: {ex.Message}";
            _logger.LogError(_errorMessage);
            return Result.Fail(_errorMessage);
        }
        catch (AuthenticationException ex)
        {
            _errorMessage = $"Email authentication failed: {ex}";
            _logger.LogError(_errorMessage);
            return Result.Fail(_errorMessage);
        }
        catch (SmtpCommandException ex)
        {
            _errorMessage = $"There has been a Smtp command error: {ex.Message}";
            _logger.LogError(_errorMessage);
            return Result.Fail(_errorMessage);
        }
        catch (SmtpProtocolException ex)
        {
            _errorMessage = $"There has been a Smtp protocol error: {ex.Message}";
            _logger.LogError(_errorMessage);
            return Result.Fail(_errorMessage);
        }
        catch (SocketException ex)
        {
            _errorMessage = $"There has been a Socket error: {ex.Message}";
            _logger.LogError(_errorMessage);
            return Result.Fail(_errorMessage);
        }
        catch (ProtocolException ex)
        {
            _errorMessage = $"There has been a Protocol error: {ex.Message}";
            _logger.LogError(_errorMessage);
            return Result.Fail(_errorMessage);
        }
        catch (InvalidOperationException ex)
        {
            _errorMessage = $"Invalid operation performed: {ex.Message}";
            _logger.LogError(_errorMessage);
            return Result.Fail(_errorMessage);
        }
        catch (Exception ex)
        {
            _errorMessage = $"An unexpected error has occurred: {ex.Message}";
            _logger.LogError(_errorMessage);
            return Result.Fail(_errorMessage);
        }
    }
}

