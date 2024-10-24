using Domain.Entities.Settings;

namespace ApiArchetype.Configurations
{
    public static class SettingsConfiguration
    {
        public static WebApplicationBuilder ConfigureSettings(this WebApplicationBuilder builder)
        {
            builder.Services
                .Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));

            return builder;
        }
    }
}
