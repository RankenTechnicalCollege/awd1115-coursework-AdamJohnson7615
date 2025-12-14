using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StarLogger.Models;

namespace StarLogger.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager; // 1. Add UserManager

        // 2. Inject UserManager in the constructor
        public LoginModel(SignInManager<User> signInManager, UserManager<User> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }
            ReturnUrl = returnUrl ?? Url.Content("~/");
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                // --- FIX STARTS HERE ---
                // 3. Find the user by Email first
                var user = await _userManager.FindByEmailAsync(Input.Email);

                if (user != null)
                {
                    // 4. Sign in using the FOUND USERNAME (e.g. "StarCommander"), not the Email
                    var result = await _signInManager.PasswordSignInAsync(user.UserName, Input.Password, isPersistent: false, lockoutOnFailure: false);

                    if (result.Succeeded)
                    {
                        return LocalRedirect(returnUrl);
                    }
                }
                // --- FIX ENDS HERE ---

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return Page();
            }

            return Page();
        }
    }
}