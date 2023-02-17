using System.Text;
using System.Text.Json;
using PlatformService.Dtos;

namespace PlatformService.SyncDataServices.HTTP;

public class HttpCommandDataClient : ICommandDataClient
{
    private readonly HttpClient _client;
    private readonly IConfiguration _configuration;

    public HttpCommandDataClient(HttpClient client, IConfiguration configuration)
    {
        _client = client;
        _configuration = configuration;
    }
    
    public async Task SendPlatformToCommand(PlatformReadDto platform)
    {
        var httpContent = new StringContent(
            JsonSerializer.Serialize(platform),
            Encoding.UTF8,
            "application/json"
        );

        var url = _configuration["CommandService"];
        var response = await _client.PostAsync(url, httpContent);

        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("--> Sync POST to CommandService was OK!");
        }
        else
        {
            Console.WriteLine("--> Sync POST to CommandService was NOT OK!");
        }
    }
}