namespace Forja.Infrastructure.Services;

public class EmailService : IEmailService
{
    // Necessary to configure the right frontend and backend URLs
    private readonly string _frontendBaseUrl = "https://localhost:3003";
    //private readonly string _backBaseUrl = "https://localhost:7052";
    
    private readonly string _smtpHost;
    private readonly int _smtpPort;
    private readonly bool _smtpEnableSsl;
    private readonly string _smtpUser;
    private readonly string _smtpPassword;

    public EmailService(IConfiguration configuration)
    {
        _smtpHost = configuration["Email:SMTPHost"] ?? throw new Exception("SMTP configuration error: 'SmtpHost' is required.");
        _smtpPort = int.TryParse(configuration["Email:SMTPPort"], out var port) 
            ? port 
            : throw new Exception("SMTP configuration error: 'SmtpPort' must be a valid integer.");
        _smtpEnableSsl = bool.TryParse(configuration["Email:SMTPEnableSSL"], out var enableSsl) 
            ? enableSsl 
            : throw new Exception("SMTP configuration error: 'SmtpEnableSsl' must be a valid boolean.");
        _smtpUser = configuration["Email:SMTPUser"] ?? throw new Exception("SMTP configuration error: 'SmtpUser' is required.");
        _smtpPassword = configuration["Email:SMTPPassword"] ?? throw new Exception("SMTP configuration error: 'SmtpPassword' is required.");
    }

    /// <inheritdoc />
    public async Task SendPasswordResetEmailAsync(string email, string resetLink, string locale)
    {
        var fullResetLink = _frontendBaseUrl + resetLink;

        string subject;
        string emailBody;

        switch (locale?.ToLowerInvariant())
        {
            case "uk":
                subject = "����� �� �������� ������";
                emailBody = $"<p>��� ������� ������, �������� <a href='{fullResetLink}'>����</a>.</p>";
                break;

            case "en":
            default:
                subject = "Password Reset Request";
                emailBody = $"<p>To reset your password, please click <a href='{fullResetLink}'>here</a>.</p>";
                break;
        }

        await SendEmailAsync(email, subject, emailBody, isBodyHtml: true);
    }


    /// <inheritdoc />
    public async Task SendEmailConfirmationAsync(string email, string username, string confirmationLink)
    {
        var fullConfirmationLink = _frontendBaseUrl + confirmationLink;
        
        var emailBody = $@"
        <html>
        <body>
            <p>Hi {username},</p>
            <p>Thank you for registering! To complete your registration, please confirm your email address by clicking the link below:</p>
            <a href='{fullConfirmationLink}' target='_blank' rel='noopener noreferrer'>Confirm Email</a>
            <p>If you did not register, you can ignore this email.</p>
        </body>
        </html>";
        
        await SendEmailAsync(email, "Confirm your email address", emailBody, isBodyHtml: true);
    }
    
    private async Task SendEmailAsync(string recipientEmail, string subject, string body, bool isBodyHtml)
    {
        var fromAddress = new MailAddress(_smtpUser, "Forja Support");
        var toAddress = new MailAddress(recipientEmail);
        
        var mailMessage = new MailMessage(fromAddress, toAddress)
        {
            Subject = subject,
            Body = body,
            IsBodyHtml = isBodyHtml
        };

        try
        {
            using var smtpClient = ConfigureSmtpClient();
            await smtpClient.SendMailAsync(mailMessage);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to send email.", ex);
        }
    }
    
    private SmtpClient ConfigureSmtpClient()
    {
        var smtpClient = new SmtpClient(_smtpHost, _smtpPort)
        {
            EnableSsl = _smtpEnableSsl,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(_smtpUser, _smtpPassword)
        };
        return smtpClient;
    }
}