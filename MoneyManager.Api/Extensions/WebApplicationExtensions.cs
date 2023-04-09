namespace MoneyManager.Api.Extensions;

public static class WebApplicationExtensions
{
    public static void ConfigureHttpRequestPipeline(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
            UseSwagger(app);

        app.UseHttpsRedirection();
        app.MapControllers();
    }

    private static void UseSwagger(IApplicationBuilder builder)
    {
        builder.UseSwagger();
        builder.UseSwaggerUI();
    }
}