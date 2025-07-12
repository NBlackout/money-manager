namespace Api.Extensions;

public static class WebApplicationExtensions
{
    public static void ConfigureHttpRequestPipeline(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
            app.UseWebAssemblyDebugging();

        app.UseSwaggerComponents();
        app.UseHttpsRedirection();
        app.MapControllers();
        app.UseBlazor();
    }

    private static void UseSwaggerComponents(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    private static void UseBlazor(this WebApplication app)
    {
        app.UseBlazorFrameworkFiles();
        app.UseStaticFiles();
        app.UseRouting();
        app.MapRazorPages();
    }
}