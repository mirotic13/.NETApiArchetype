using Microsoft.Data.SqlClient;
using System.Data;

namespace ApiArchetype.Configurations;
public static class DatabaseConfiguration
{
    public static WebApplicationBuilder ConfigureDatabase(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IDbConnection>(sp =>
            new SqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));

        return builder;
    }
}
