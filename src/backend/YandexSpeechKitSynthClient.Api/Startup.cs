using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System.IO;
using System.Linq;
using System.Reflection;

namespace YandexSpeechKitSynthClient.Api;

public class Startup
{
    private const string STATIC_FILES_PATH = "wwwroot";

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
        ConfigureEmbeddedAssets(app);
        app.UseRouting().UseEndpoints(endpoints =>
        {
            endpoints.MapDefaultControllerRoute();
        });
    }

    private static void ConfigureEmbeddedAssets(IApplicationBuilder app)
    {
        if (Directory.EnumerateFiles(STATIC_FILES_PATH).Any())
        {
            var manifestEmbeddedFileProvider = new ManifestEmbeddedFileProvider(
                Assembly.GetExecutingAssembly(),
                STATIC_FILES_PATH);

            var staticFileOptions = new StaticFileOptions
            {
                FileProvider = manifestEmbeddedFileProvider,
                RequestPath = string.Empty,
            };

            app.UseSpaStaticFiles(staticFileOptions);
            app.UseSpa(spaBuilder => spaBuilder.Options.DefaultPageStaticFileOptions = staticFileOptions);
        }
    }
}
