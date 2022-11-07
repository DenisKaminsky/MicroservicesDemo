using System.Text;
using System.Text.Json;
using PlatformService.Core.Interfaces.Http;
using PlatformService.DTOs;

namespace PlatformService.Core.Services.Http;

public class HttpCommandDataClient : ICommandDataClient
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public HttpCommandDataClient(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task SendPlatformToCommandAsync(PlatformReadDto platform)
    {
        var requestUri = _configuration["Dependencies:CommandsService:Platforms"];
        var requestContent = new StringContent(
            JsonSerializer.Serialize(platform),
            Encoding.UTF8,
            "application/json"
        );
        
        var response = await _httpClient.PostAsync(requestUri, requestContent);

        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("--> SendPlatformToCommandAsync - Ok");
        }
        else
        {
            Console.WriteLine("--> SendPlatformToCommandAsync - Error");
        }
    }
}