using Microsoft.AspNetCore.Mvc.Testing;

namespace WebApp.Tests.Support;

public class WebApplicationContext(WebApplicationFactory<Program> webApp)
{
    public void Start()
    {
        Client = webApp.CreateClient();
    }

    private HttpClient Client { get; set; } = null!;

    public async Task GetAsync(string url)
    {
        var responseMessage =  await Client.GetAsync(url);

        BodyOfLastResponse = await responseMessage.Content.ReadAsStringAsync();
    }

    public string? BodyOfLastResponse { get; private set; }
}