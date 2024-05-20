using ArkFunds.Identity.Models;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ArkFunds.Identity.Pages.Account.Create;

[SecurityHeaders]
[AllowAnonymous]
public class Index : PageModel
{
    private readonly IIdentityServerInteractionService interaction;
    private readonly UserManager<ApplicationUser> userManager;
    private readonly SignInManager<ApplicationUser> signInManager;

    [BindProperty]
    public AppUserCreateInputModel Input { get; set; }
        
    public Index(
        IIdentityServerInteractionService interaction,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager)
    {
        this.interaction = interaction;
        this.userManager = userManager;
        this.signInManager = signInManager;
    }

    public IActionResult OnGet(string? returnUrl)
    {
        Input = new AppUserCreateInputModel { ReturnUrl = returnUrl };
        return Page();
    }
        
    public async Task<IActionResult> OnPost()
    {
        // check if we are in the context of an authorization request
        var context = await interaction.GetAuthorizationContextAsync(Input.ReturnUrl);

        // the user clicked the "cancel" button
        if (Input.Button != "create")
        {
            if (context == null)
            {
                // since we don't have a valid context, then we just go back to the home page
                return Redirect("~/");
            }
            
            await interaction.DenyAuthorizationAsync(context, AuthorizationError.AccessDenied);

            if (context.IsNativeClient())
            {
                return this.LoadingPage(Input.ReturnUrl);
            }

            if (Input.ReturnUrl != null)
            {
                return Redirect(Input.ReturnUrl);
            }
            return Page();
        }

        if (await userManager.FindByEmailAsync(Input.Email) != null)
        {
            ModelState.AddModelError("Input.Email", "User with this e-mail already exists");
        }

        if (ModelState.IsValid)
        {
            var appUserCreateModel = new AppUserCreateInputModel
            {
                Username = Input.Username,
                Password = Input.Password,
                Email = Input.Email
            };
            
            var user = new ApplicationUser()
            {
                UserName = appUserCreateModel.Username,
                Email = appUserCreateModel.Email
            };
            var result = await userManager.CreateAsync(user, appUserCreateModel.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }
            
            var loggedInUser = await userManager.FindByNameAsync(appUserCreateModel.Username);

            await signInManager.SignInAsync(loggedInUser!, false);

            if (context != null)
            {
                if (context.IsNativeClient())
                {
                    return this.LoadingPage(Input.ReturnUrl);
                }

                if (Input.ReturnUrl != null)
                {
                    // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                    return Redirect(Input.ReturnUrl);
                }
                return Page();
            }

            if (string.IsNullOrEmpty(Input.ReturnUrl))
            {
                return Redirect("~/");
            }
            
            return Redirect(Input.ReturnUrl);
        }

        return Page();
    }
}
