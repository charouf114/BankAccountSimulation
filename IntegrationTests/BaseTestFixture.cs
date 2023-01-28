using Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationTests;

[TestFixture]
public abstract class BaseTestFixture
{
    private static IServiceScope serviceScope = null!;

    private static CustomWebApplicationFactory _factory;

    protected static ApplicationDBContext datalayer { get; set; }

    protected static HttpClient httpClient { get; set; }

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        _factory = new CustomWebApplicationFactory();

        var _scopeFactory = _factory.Services.GetRequiredService<IServiceScopeFactory>();

        serviceScope = _scopeFactory.CreateScope();

        datalayer = GetRequiredService<ApplicationDBContext>();

        httpClient = _factory.Server.CreateClient();
    }

    [SetUp]
    public async Task TestSetUp()
    {

    }

    private IType GetRequiredService<IType>()
    {
        return serviceScope.ServiceProvider.GetRequiredService<IType>();
    }

    [TearDown]
    public void TearDown()
    {

    }

    [OneTimeTearDown]
    public void RunAfterAnyTests()
    {
        serviceScope?.Dispose();
        httpClient?.Dispose();
    }
}
