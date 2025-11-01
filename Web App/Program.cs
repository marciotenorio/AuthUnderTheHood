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
            .AddCookie("MyCookieAuth", opt =>
            {
                opt.Cookie.Name = "MyCookieAuth";
                opt.LoginPath = "/Account/Login";
                opt.LogoutPath = "/Account/Logout";
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

        //In previous versions is needed to explicitily declare the use here
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