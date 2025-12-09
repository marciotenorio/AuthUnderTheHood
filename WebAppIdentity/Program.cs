using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebAppIdentity.Infra;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<WebAppIdentityDbContext>(options =>
{
    options.UseSqlServer("Server=localhost; Database=authdb; User Id=sa; Password=YourStrong!Passw0rd; TrustServerCertificate=True; Encrypt=False;");
});

//You can implement those interfaces and prove customizable user and role
// Identity has main classes:
// SignInManager<TUser> - verity credentials, generate security context, authentication, authorization with policies, etc
// UserManager<> - Deal with user on the database
// builder.Services.AddIdentityApiEndpoints<>();
// builder.Services.AddIdentityCore<>();
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
    {
        options.Password.RequiredLength = 6;
        options.Password.RequireUppercase = true;
        options.Password.RequireLowercase = true;
        
        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
        
        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<WebAppIdentityDbContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
    .WithStaticAssets();

app.Run();