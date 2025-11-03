using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web_App.Pages;

//Hey authorization middleware, look for this requirement here!
[Authorize(Policy = "MustBelongToHRDepartment")]
public class HumanResourceModel : PageModel
{
    public void OnGet()
    {
    }
}
