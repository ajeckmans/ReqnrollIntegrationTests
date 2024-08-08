using Microsoft.AspNetCore.Mvc.Testing;

namespace WebApp.Tests.Support;

/// <summary>
/// A wrapper for the web application factory to ease making requests and verifying the responses.
/// </summary
public class WebApplicationContext(WebApplicationFactory<Program> webApp)
{
    public void Start()
    {
        // Creating a client actually starts the web application as well
        Client = webApp.CreateClient();
    }

    private HttpClient Client { get; set; } = null!;

    public async Task GetAsync(string url)
    {
        // make the request and store the response body
        var responseMessage =  await Client.GetAsync(url);

        BodyOfLastResponse = await responseMessage.Content.ReadAsStringAsync();
    }

    public string? BodyOfLastResponse { get; private set; }
}
