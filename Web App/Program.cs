using Microsoft.AspNetCore.Authorization;
using Web_App.Authorization;

namespace Web_App;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRazorPages();

        //In previous versions you must define the default scheme
        builder.Services.AddAuthentication("MyCookieAuth")
            //I can have multiple authentication schemes
            //Cookie has some properties like: expires, path, domain, max-age, httpOnly (not be changed by JS), etc.
            .AddCookie("MyCookieAuth", opt =>
            {
                opt.Cookie.Name = "MyCookieAuth";
                opt.ExpireTimeSpan = TimeSpan.FromSeconds(2_000_000);
                
                //Default locations for login and logout
                opt.LoginPath = "/Account/Login";
                opt.LogoutPath = "/Account/Logout";
                opt.AccessDeniedPath = "/Account/AccessDenied";
            });

        builder.Services.AddAuthorization(opt =>
        {
            //claim with specific value
            opt.AddPolicy("MustBelongToHRDepartment", policy => policy.RequireClaim("Department", "HR"));
            //only claim
            opt.AddPolicy("AdminOnly", policy => policy.RequireClaim("Admin"));
            //both approaches
            opt.AddPolicy("HRManagerOnly", policy => policy
                .RequireClaim("Department", "HR")
                .RequireClaim("Manager")
                .Requirements.Add(new HRManagerProbationRequirement(3))); //you need to add the handler of requirement to DI
        });

        builder.Services.AddSingleton<IAuthorizationHandler, HRManagerProbationRequirementHandler>(); //handler registered here!

        builder.Services.AddHttpClient("OurWebAPI", client =>
        {
            client.BaseAddress = new Uri("https://localhost:8081/");
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

        //In previous versions is needed to explicitly declare the use here
        //right here: after routing and before authorization!
        //Use the authentication handler (IAuthenticationService) to interpret the security context from http request
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapStaticAssets();
        app.MapRazorPages()
            .WithStaticAssets();

        app.Run();
    }
}   