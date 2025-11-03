using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web_App.Pages.Account;

public class LogoutModel : PageModel
{
    public async Task<IActionResult> OnPostAsync()
    {
        //Implies that the cookie will be erased of browser, sending: 
        //MyCookieAuth=; expires=Thu, 01 Jan 1970 00:00:00 GMT; path=/; samesite=lax; httponly
        await HttpContext.SignOutAsync("MyCookieAuth");
        return RedirectToPage("/Index");
    }
}