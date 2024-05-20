using System.ComponentModel.DataAnnotations;

namespace ArkFunds.Identity.Pages.Account.Create;

public class AppUserCreateInputModel
{
    [Required]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }

    public string Name { get; set; }
    public string Email { get; set; }

    public string? ReturnUrl { get; set; }

    public string Button { get; set; }
}