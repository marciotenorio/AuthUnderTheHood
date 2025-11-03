using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web_App.Pages.Account; 

public class LoginModel : PageModel
{
    [BindProperty]
    public Credential Credential { get; set; }

    public void OnGet()
    {
        //Here can i see the user/claimsprincipal attributes (isAuthenticated, anonymous, etc)
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (Credential.Password != "admin" || Credential.UserName != "admin" || !ModelState.IsValid)
            return Page();
        
        //Creating security context
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, "admin"),
            new(ClaimTypes.Email, "admin@mywebsite.com"),
            new("Department", "HR"), //custom claim for authorization policy
            new("Admin", "true"), //only checked if claim existis in specified identity, not his value.
            new("Manager", "true"),
            new("EmploymentDate", "2025-05-01")
        };
        //I can have multiple authentication types, here i'm creating one.
        var identity = new ClaimsIdentity(claims, "MyCookieAuth");
        
        //Contains the security context
        var principal = new ClaimsPrincipal(identity);

        //serialize as a string (claims principal), encrypt and return as a cookie to the http context.
        //Under the hood the signInAsync uses authentication handler, thats need to be configured! IAuthenticationService.
        await HttpContext.SignInAsync("MyCookieAuth", principal, new()
        {
            //configure cookie lifetime to not be browser session.
            IsPersistent = Credential.RememberMe
        });

        return RedirectToPage("/Index");
    }
}

public class Credential
{
    [Required]
    [Display(Description = "User name")]
    public string UserName { get; set; }
        
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    
    public bool RememberMe { get; set; }
}