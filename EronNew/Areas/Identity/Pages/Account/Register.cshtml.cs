using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using EronNew.Data;
using EronNew.Helpers;
using EronNew.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace EronNew.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ExtendedIdentityUser> _signInManager;
        private readonly ExtendedUserManager<ExtendedIdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private IDomainModel _model;

        public RegisterModel(
            ExtendedUserManager<ExtendedIdentityUser> userManager,
            SignInManager<ExtendedIdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            IDomainModel model)
        {
            _model = model;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Όνομα")]
            public string FirstName { get; set; }

            [Required]
            [Display(Name = "Επίθετο")]
            public string LastName { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Κωδικός")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Επαλήθευση Κωδικού")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Τηλέφωνο Επικοινωνίας")]
            //[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string PhoneNumber { get; set; }

            public bool Newsletter { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null, string fbclid = null)
        {
            ReturnUrl = returnUrl;
            if (fbclid != null)
            {
                var user = await _userManager.GetUserAsync(User);
                var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress;
                if (user != null)
                {
                    await _model.PostInterest(0, user.Id, remoteIpAddress.ToString(), UserInterest.fbclid, $@"Register- {fbclid}");
                }
                else
                {
                    await _model.PostInterest(0,null, remoteIpAddress.ToString(), UserInterest.fbclid, $@"Register- {fbclid}", StaticHelpers.GetLocation(remoteIpAddress.ToString()));
                }
            }
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new ExtendedIdentityUser { UserName = Input.Email, Email = Input.Email, FirstName = Input.FirstName, LastName = Input.LastName, PhoneNumber = Input.PhoneNumber };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    var resultOfWallet =_userManager.CreateWallet(Input.Email);
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);
                    await _userManager.SetSubscribeNewsLetter(Input.Email, Input.Newsletter);
                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email", EronNew.Helpers.StaticHelpers.GetVerificationEmail(Input.LastName + " " + Input.FirstName, HtmlEncoder.Default.Encode(callbackUrl), "https://www.eron.gr/unsubscribe/"+ Input.Email));

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }


}
