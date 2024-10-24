using Microsoft.OpenApi.Models;

namespace ApiArchetype.Configurations
{
    public static class SwaggerConfiguration
    {
        public static WebApplicationBuilder ConfigureSwagger(this WebApplicationBuilder builder, string apiName)
        {
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = apiName, Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Add 'Bearer' [whitespace] and your JWT token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            return builder;
        }
    }
}
