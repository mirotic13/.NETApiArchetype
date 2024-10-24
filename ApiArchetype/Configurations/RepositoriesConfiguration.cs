using ApiArchetype.Repositories;

namespace ApiArchetype.Configurations
{
    public static class RepositoriesConfiguration
    {
        public static WebApplicationBuilder ConfigureRepositories(this WebApplicationBuilder builder)
        {
            builder.Services
                .AddScoped<ITemplateClassRepository, TemplateClassRepository>()
                .AddScoped<IUserRepository, UserRepository>();

            return builder;
        }
    }
}
