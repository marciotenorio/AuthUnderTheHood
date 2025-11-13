using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web_API.Dto;
using Web_API.Service;

namespace Web_API.Controllers;

[Route("auth")]
[ApiController]
public class AuthController(AuthService authService) : ControllerBase
{
    private readonly AuthService _authService = authService;

    [HttpPost]
    public IActionResult Authenticate([FromBody] Credential credential)
    {
        if (credential.Password != "admin" || credential.UserName != "admin" || !ModelState.IsValid)
        {
            ModelState.AddModelError("Unauthorized", "Your credentials are are invalid.");
            return Unauthorized(new ProblemDetails
            {
                Title = "Unauthorized problem details",
                Status = StatusCodes.Status401Unauthorized
            });    
        }

        //Token has only claims, not principal or something else.
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, "admin"),
            new(ClaimTypes.Email, "admin@mywebsite.com"),
            new("Department", "HR"), //custom claim for authorization policy
            new("Admin", "true"), //only checked if claim existis in specified identity, not his value.
            new("Manager", "true"),
            new("EmploymentDate", "2025-05-01")
        };

        var expiresAtSeconds = DateTime.UtcNow.AddSeconds(3600);

        return Ok(new
        {
            access_token = _authService.CreateToken(claims, expiresAtSeconds),
            expires_at = expiresAtSeconds
        });
    }
}
