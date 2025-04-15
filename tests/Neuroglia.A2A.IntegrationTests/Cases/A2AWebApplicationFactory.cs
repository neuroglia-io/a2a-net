using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore;
using Microsoft.Extensions.Hosting;

namespace Neuroglia.A2A.IntegrationTests.Cases;

public class A2AWebApplicationFactory
    : WebApplicationFactory<TestWebApplicationStartup>
{

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseContentRoot("");
    }

    protected override IWebHostBuilder? CreateWebHostBuilder()
    {
        return WebHost.CreateDefaultBuilder().UseStartup<TestWebApplicationStartup>();
    }

}