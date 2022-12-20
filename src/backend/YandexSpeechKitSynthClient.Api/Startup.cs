using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System.Reflection;

namespace YandexSpeechKitSynthClient.Api;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services
            .AddCors()
            .AddControllers();

        services
            .AddLogging();
    }

    public void Configure(IApplicationBuilder app)
    {
        var provider = new ManifestEmbeddedFileProvider(Assembly.GetAssembly(type: typeof(Startup)), "wwwroot");
        var staticFileOptions = new StaticFileOptions
        {
            FileProvider = provider,
            RequestPath = "",
        };

        app.UseSpa(spaBuilder =>
        {
            spaBuilder.Options.DefaultPageStaticFileOptions = staticFileOptions;
        });

        app.UseSpaStaticFiles(staticFileOptions);
        app.UseRouting().UseEndpoints(endpoints =>
        {
            endpoints.MapDefaultControllerRoute();
        });
    }
}
