using AspNetCoreWebIdentityApp.Core.OptionModels;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace AspNetCoreWebIdentityApp.Services.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;
        public EmailService(IOptions<EmailSettings> options)
        {
            _emailSettings = options.Value;
        }
        public async Task SendResetPasswordEmail(string resetPasswordEmailLink, string toEmail)
        {
            var stmpClient = new SmtpClient();
            stmpClient.Host = _emailSettings.Host;
            stmpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            stmpClient.UseDefaultCredentials = false;
            stmpClient.Port = 587;
            stmpClient.EnableSsl = true;
            stmpClient.Credentials = new NetworkCredential(_emailSettings.Email, _emailSettings.Password);
            var mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(_emailSettings.Email);
            mailMessage.To.Add(toEmail);
            var encodedResetLink = WebUtility.HtmlEncode(resetPasswordEmailLink);
            mailMessage.Subject = "Reset your password";
            mailMessage.Body = $"""
                <!doctype html>
                <html lang="en">
                <head><meta charset="utf-8"><meta name="viewport" content="width=device-width, initial-scale=1"></head>
                <body style="margin:0;padding:0;background-color:#edf5f0;color:#183329;font-family:Arial,sans-serif;">
                    <table role="presentation" width="100%" cellpadding="0" cellspacing="0" style="background-color:#edf5f0;">
                        <tr>
                            <td align="center" style="padding:32px 16px;">
                                <table role="presentation" width="100%" cellpadding="0" cellspacing="0" style="max-width:520px;background-color:#ffffff;border:1px solid #d5e8dd;border-radius:14px;">
                                    <tr>
                                        <td style="padding:32px;border-top:5px solid #198754;border-radius:14px;">
                                            <p style="margin:0 0 10px;color:#236947;font-size:12px;font-weight:bold;letter-spacing:1.5px;">ACCOUNT SECURITY</p>
                                            <h1 style="margin:0 0 14px;color:#123d2a;font-size:28px;line-height:1.2;">Reset your password</h1>
                                            <p style="margin:0 0 24px;color:#40574c;font-size:16px;line-height:1.6;">Use the button below to choose a new password for your account.</p>
                                            <table role="presentation" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td bgcolor="#146c43" style="border-radius:8px;">
                                                        <a href="{encodedResetLink}" style="display:inline-block;padding:14px 24px;color:#ffffff;font-size:16px;font-weight:bold;text-decoration:none;">Reset Password</a>
                                                    </td>
                                                </tr>
                                            </table>
                                            <p style="margin:24px 0 0;color:#50665b;font-size:14px;line-height:1.5;">If you didn’t request a password reset, you can ignore this email.</p>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </body>
                </html>
                """;
            mailMessage.IsBodyHtml = true;
            await stmpClient.SendMailAsync(mailMessage);
        }
    }
}
