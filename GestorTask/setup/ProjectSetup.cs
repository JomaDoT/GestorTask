using Asp.Versioning.ApiExplorer;
using GestorTask.setup.Middleware;
using ModularPrestaMix.WebApi.Setup.Middleware;

namespace GestorTask.setup;

public static class ProjectSetup
{
    public static WebApplication ConfigureProject(this WebApplication app, string MyAllowSpecificOrigins)
    {
        #region middleware
        app.UseMiddleware<ErrorHandlerMiddleware>();
        app.UseMiddleware<SecureHeadersMiddleware>();

        #endregion

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseCors(MyAllowSpecificOrigins);

        app.UseAuthentication();
        app.UseAuthorization();

        var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
            {
                options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                    description.GroupName.ToUpperInvariant());
            }
        });


        app.UseStatusCodePages();
        app.MapControllers();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        return app;
    }
}