using System.Net.Http.Headers;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Web_App.DTO;

namespace Web_App.Pages;

[Authorize(Policy = "HRManagerOnly")]
public class HRManagerModel : PageModel
{
    private readonly HttpClient _client;

    [BindProperty] //works without
    public List<WeatherForecastResponse>? WeatherForeCasts { get; set; }
    
    public HRManagerModel(IHttpClientFactory clientFactory)
    {
        _client = clientFactory.CreateClient("OurWebAPI");
    }

    public async Task OnGet()
    {
        //check if token already stored in session
        var token = new TokenResponse();
        var tokenStr = HttpContext.Session.GetString("access_token");
        if (string.IsNullOrWhiteSpace(tokenStr))
        {
            //authentication getting the token
            token = await Authenticate();

            if ((token?.ExpiresAt ?? DateTime.MinValue) <= DateTime.UtcNow)
            {
                token = await Authenticate();
            }
        }
        else
        {
            token = JsonConvert.DeserializeObject<TokenResponse>(tokenStr);
        }

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token?.AccessToken);
        WeatherForeCasts = await _client
            .GetFromJsonAsync<List<WeatherForecastResponse>>("WeatherForecast");
    }
    
    private async Task<TokenResponse>? Authenticate()
    {
        var res = await _client.PostAsJsonAsync<TokenRequest>("auth", new() { Password = "admin", UserName = "admin" });
        var str = await res.Content.ReadAsStringAsync();
        HttpContext.Session.SetString("access_token", str);
        return JsonConvert.DeserializeObject<TokenResponse>(str);
    }
}
