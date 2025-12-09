using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebAppIdentity.Infra;

public class WebAppIdentityDbContext : IdentityDbContext
{
    public WebAppIdentityDbContext(DbContextOptions<WebAppIdentityDbContext> options) : base(options)
    {
    }
}