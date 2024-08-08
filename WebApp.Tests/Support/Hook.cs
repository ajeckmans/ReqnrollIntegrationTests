using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

using Reqnroll.Assist;

using Testcontainers.MsSql;

namespace WebApp.Tests.Support;

[Binding]
public class Hooks
{
    [BeforeTestRun]
    public static void AddValueRetrievers()
    {
        Service.Instance.ValueRetrievers.Register(new DateOnlyValueRetriever());
    }

    [BeforeScenario(Order = 0)]
    public static void SetUpDi(ScenarioContext scenarioContext)
    {
        // Set up database
        var container = new MsSqlBuilder().Build();
        scenarioContext.ScenarioContainer.RegisterInstanceAs(container);
    }

    [BeforeScenario(Order = 1)]
    public static async Task StartDatabase(ScenarioContext scenarioContext)
    {
        // start the mssql db test container
        var container = scenarioContext.ScenarioContainer.Resolve<MsSqlContainer>();
        await container.StartAsync();

        scenarioContext.ScenarioContainer.RegisterInstanceAs(new DatabaseInformation
        {
            ConnectionString = container.GetConnectionString()
        });
    }


    [BeforeScenario(Order = 2)]
    public static void StartWebApplication(ScenarioContext scenarioContext)
    {
        var databaseInformation = scenarioContext.ScenarioContainer.Resolve<DatabaseInformation>();

        // Set up Web application
        var webApp = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(c => c.UseConfiguration(new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["ConnectionStrings:DefaultConnection"] = databaseInformation.ConnectionString
                }.AsReadOnly())
                .Build()));
        var webAppContext = new WebApplicationContext(webApp);
        webAppContext.Start();
        scenarioContext.ScenarioContainer.RegisterInstanceAs(webAppContext);
    }

    [AfterScenario]
    public static async Task StopDatabase(ScenarioContext scenarioContext)
    {
        // stop the mssql db test container
        var container = scenarioContext.ScenarioContainer.Resolve<MsSqlContainer>();
        await container.DisposeAsync();
    }
}