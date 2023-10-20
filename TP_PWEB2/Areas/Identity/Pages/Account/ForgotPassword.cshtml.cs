using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using TP_PWEB2.Data;
using System.Net.Mail;
using System.Net;

namespace TP_PWEB2.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailSender _emailSender;

        public ForgotPasswordModel(UserManager<AppUser> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToPage("./ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please 
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new { area = "Identity", code },
                    protocol: Request.Scheme);

                /*await _emailSender.SendEmailAsync(
                    Input.Email,
                    "Reset Password",
                    $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
                */
                try
                {
                    var email_sender = Environment.GetEnvironmentVariable("PWEB_EMAIL", EnvironmentVariableTarget.User);
                    var pwd_email_sender = Environment.GetEnvironmentVariable("PWEB_EMAIL_PASS", EnvironmentVariableTarget.User);

                    if ( email_sender != null || pwd_email_sender != null)
                    {
                        var cliente = new SmtpClient("smtp.gmail.com", 587)
                        {
                            Credentials = new NetworkCredential(email_sender, pwd_email_sender),
                            UseDefaultCredentials = false,
                            EnableSsl = true
                        };
                        MailMessage mail_a_enviar = new MailMessage(email_sender, user.Email);
                        mail_a_enviar.Subject = "Reset Password";
                        mail_a_enviar.Body = $"Please reset your password by {HtmlEncoder.Default.Encode(callbackUrl)}";
                        //await cliente.SendMailAsync(email_sender, user.Email, "Reset Password", msg_a_enviar);
                        cliente.Send(mail_a_enviar);
                        cliente.Dispose();
                    }
                }
                catch (Exception)
                {
                    return RedirectToPage("./ForgotPasswordConfirmation");
                    throw;
                }

                return RedirectToPage("./ForgotPasswordConfirmation");
            }

            return Page();
        }
    }
}
