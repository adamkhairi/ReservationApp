using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using ReservationApp.Data;
using System.Text.RegularExpressions;

namespace ReservationApp.Areas.Identity.Pages.Account
{
     [AllowAnonymous]
     public class LoginModel : PageModel
     {
          private readonly UserManager<IdentityUser> _userManager;
          private readonly SignInManager<IdentityUser> _signInManager;
          private readonly ILogger<LoginModel> _logger;
          private readonly ApplicationDbContext _context;


          public LoginModel(SignInManager<IdentityUser> signInManager,
              ILogger<LoginModel> logger,
              UserManager<IdentityUser> userManager, ApplicationDbContext context)
          {
               _userManager = userManager;
               _context = context;
               _signInManager = signInManager;
               _logger = logger;
          }

          [BindProperty]
          public InputModel Input { get; set; }

          public IList<AuthenticationScheme> ExternalLogins { get; set; }

          public string ReturnUrl { get; set; }

          [TempData]
          public string ErrorMessage { get; set; }

          public class InputModel
          {
               [Required]
               [Display(Name = "Username or Email")]
               public string Email { get; set; }

               [Required]
               [DataType(DataType.Password)]
               public string Password { get; set; }

               [Display(Name = "Remember me?")]
               public bool RememberMe { get; set; }
          }

          public async Task OnGetAsync(string returnUrl = null)
          {
               if (!string.IsNullOrEmpty(ErrorMessage))
               {
                    ModelState.AddModelError(string.Empty, ErrorMessage);
               }

               returnUrl ??= Url.Content("~/");

               // Clear the existing external cookie to ensure a clean login process
               await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

               ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

               ReturnUrl = returnUrl;
          }

          public async Task<IActionResult> OnPostAsync(string returnUrl = null)
          {
               returnUrl ??= Url.Content("~/");

               //if (Input.Email.IndexOf('@') > -1)
               //{
               //     //Validate email format
               //     string emailRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
               //                            @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
               //                               @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
               //     Regex re = new Regex(emailRegex);
               //     if (!re.IsMatch(Input.Email))
               //     {
               //          ModelState.AddModelError("Email", "Email is not valid");
               //     }
               //}
               //else
               //{
               //     //validate Username format
               //     string emailRegex = @"^[a-zA-Z0-9]*$";
               //     Regex re = new Regex(emailRegex);
               //     if (!re.IsMatch(Input.Email))
               //     {
               //          ModelState.AddModelError("Email", "Username is not valid");
               //     }
               //}

               ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

               if (ModelState.IsValid)
               {
                   var userName = Input.Email;
                    if (userName.IndexOf('@') > -1)
                    {
                         var user = await _userManager.FindByEmailAsync(Input.Email);
                         if (user == null)
                         {
                              ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                              return Page();
                         }
                         else
                         {
                              userName = user.UserName;
                         }
                    }
                    var result = await _signInManager.PasswordSignInAsync(userName, Input.Password, Input.RememberMe, lockoutOnFailure: false);

                    // var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);

                    if (result.Succeeded)
                    {
                         _logger.LogInformation("User logged in.");
                         return LocalRedirect(returnUrl);
                    }
                    if (result.RequiresTwoFactor)
                    {
                         return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                    }
                    if (result.IsLockedOut)
                    {
                         _logger.LogWarning("User account locked out.");
                         return RedirectToPage("./Lockout");
                    }
                    else
                    {
                         ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                         return Page();
                    }
               }

               // If we got this far, something failed, redisplay form
               return Page();
          }
     }
}
