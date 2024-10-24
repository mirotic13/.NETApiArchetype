using ApiArchetype.Configurations;
using ApiArchetype.Exceptions;

var builder = WebApplication.CreateBuilder(args);
var apiName = "API V1";

builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder
    .ConfigureSettings()
    .ConfigureAuthentication()
    .ConfigureLogging()
    .ConfigureDatabase()
    .ConfigureRepositories()
    .ConfigureServices()
    .ConfigureSwagger(apiName);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", apiName);
    });
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
