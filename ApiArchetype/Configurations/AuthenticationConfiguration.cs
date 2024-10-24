using Domain.Entities.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ApiArchetype.Configurations;
public static class AuthenticationConfiguration
{
    public static WebApplicationBuilder ConfigureAuthentication(this WebApplicationBuilder builder)
    {
        var jwtSettings = builder.Services.BuildServiceProvider().GetRequiredService<IOptions<JwtSettings>>().Value;

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            };
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var token = context.Request.Cookies["AuthToken"];
                    if (!string.IsNullOrEmpty(token))
                    {
                        context.Token = token;
                    }
                    return Task.CompletedTask;
                }
            };
        });

        return builder;
    }
}
