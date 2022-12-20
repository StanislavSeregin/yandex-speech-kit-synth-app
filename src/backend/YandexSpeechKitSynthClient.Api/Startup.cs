using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using System;

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
        app
            .UseRouting()
            .UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            })
            .UseStaticFiles()
            .UseSpa(spaBuilder =>
            {
                var cacheControlHeaderValue = new CacheControlHeaderValue
                {
                    NoCache = true,
                    NoStore = true,
                    MustRevalidate = true,
                    MaxAge = TimeSpan.Zero
                };

                var staticFileOptions = new StaticFileOptions
                {
                    OnPrepareResponse = ctx =>
                    {
                        var headers = ctx.Context.Response.GetTypedHeaders();
                        headers.CacheControl = cacheControlHeaderValue;
                    }
                };

                spaBuilder.Options.DefaultPageStaticFileOptions = staticFileOptions;
            });
    }
}
