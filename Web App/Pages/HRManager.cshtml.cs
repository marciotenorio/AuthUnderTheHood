using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
        WeatherForeCasts = await _client
            .GetFromJsonAsync<List<WeatherForecastResponse>>("WeatherForecast");
    }
}
