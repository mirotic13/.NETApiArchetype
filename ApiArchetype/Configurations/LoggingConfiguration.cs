namespace ApiArchetype.Configurations
{
    public static class LoggingConfiguration
    {
        public static WebApplicationBuilder ConfigureLogging(this WebApplicationBuilder builder)
        {
            builder.Logging.ClearProviders()
                .AddConsole()
                .AddDebug();

            return builder;
        }
    }
}
