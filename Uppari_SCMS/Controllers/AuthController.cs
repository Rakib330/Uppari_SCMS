using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Uppari_SCMS.Data.Auth;

[Route("auth")]
public class AuthController : Controller
{
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AuthController(SignInManager<ApplicationUser> signInManager)
    {
        _signInManager = signInManager;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(string username, string password, bool rememberMe = false, string? returnUrl = null)
    {
        var result = await _signInManager.PasswordSignInAsync(username, password, rememberMe, false);
        if (result.Succeeded)
        {
            return Redirect(returnUrl ?? "/");
        }

        // Optional: handle failure (you can show error on query string)
        return Redirect("/login?failed=true");
    }
}
