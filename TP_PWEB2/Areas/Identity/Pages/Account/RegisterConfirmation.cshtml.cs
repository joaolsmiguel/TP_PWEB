using Microsoft.AspNetCore.Authorization;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using TP_PWEB2.Data;
using System;
using System.Net.Mail;
using System.Net;

namespace TP_PWEB2.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterConfirmationModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailSender _sender;

        public RegisterConfirmationModel(UserManager<AppUser> userManager, IEmailSender sender)
        {
            _userManager = userManager;
            _sender = sender;
        }

        public string Email { get; set; }

        public bool DisplayConfirmAccountLink { get; set; }

        public string EmailConfirmationUrl { get; set; }

        public async Task<IActionResult> OnGetAsync(string email, string returnUrl = null)
        {
            if (email == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound($"Unable to load user with email '{email}'.");
            }

            Email = email;
            // Once you add a real email sender, you should remove this code that lets you confirm the account
            DisplayConfirmAccountLink = true;
            if (DisplayConfirmAccountLink)
            {
                var userId = await _userManager.GetUserIdAsync(user);
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                EmailConfirmationUrl = Url.Page(
                    "/Account/ConfirmEmail",
                    pageHandler: null,
                    values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                    protocol: Request.Scheme);


                try
                {
                    var email_sender = Environment.GetEnvironmentVariable("PWEB_EMAIL", EnvironmentVariableTarget.User);
                    var pwd_email_sender = Environment.GetEnvironmentVariable("PWEB_EMAIL_PASS", EnvironmentVariableTarget.User);

                    if (email_sender != null || pwd_email_sender != null)
                    {
                        var cliente = new SmtpClient("smtp.gmail.com", 587)
                        {
                            Credentials = new NetworkCredential(email_sender, pwd_email_sender),
                            UseDefaultCredentials = false,
                            EnableSsl = true
                        };
                        MailMessage mail_a_enviar = new MailMessage(email_sender, user.Email);
                        mail_a_enviar.Subject = "Confirm User";
                        mail_a_enviar.Body = EmailConfirmationUrl;
                        //await cliente.SendMailAsync(email_sender, user.Email, "Reset Password", msg_a_enviar);
                        cliente.Send(mail_a_enviar);
                        cliente.Dispose();
                        return RedirectToPage("/Index");
                    }
                }
                catch (Exception)
                {
                    return Page();
                    throw;
                }
            }

            return Page();
        }
    }
}
