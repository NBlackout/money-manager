namespace MoneyManager.Api.Extensions;

public static class WebApplicationExtensions
{
    public static void ConfigureHttpRequestPipeline(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwaggerComponents();
            app.UseWebAssemblyDebugging();
        }

        app.UseHttpsRedirection();
        app.MapControllers();

        app.UseBlazor();
    }

    private static void UseSwaggerComponents(this IApplicationBuilder app)
    {
        app.UseSwagger(options => options.RouteTemplate = $"api/{options.RouteTemplate}");
        app.UseSwaggerUI(options => options.RoutePrefix = $"api/{options.RoutePrefix}");
    }

    private static void UseBlazor(this WebApplication app)
    {
        app.UseBlazorFrameworkFiles();
        app.UseStaticFiles();

        app.UseRouting();
        app.MapRazorPages();
        app.MapFallbackToFile("index.html");
    }
}