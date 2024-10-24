using ApiArchetype.Services;

namespace ApiArchetype.Configurations
{
    public static class ServicesConfiguration
    {
        public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
        {
            builder.Services
                .AddScoped<ITemplateClassService, TemplateClassService>();

            return builder;
        }
    }
}
