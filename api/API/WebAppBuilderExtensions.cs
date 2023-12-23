namespace API;

internal static class WebAppBuilderExtensions
{
    public static void AddClientCorsPolicy(this WebApplicationBuilder webApplicationBuilder)
    {
        var clientUrl = webApplicationBuilder.Configuration["ClientUrl"]
                        ?? throw new ArgumentException(
                            "ClientUrl not found in app configuration");

        webApplicationBuilder.Services.AddCors(cors =>
        {
            cors.AddDefaultPolicy(p => p.AllowAnyHeader().AllowAnyMethod().WithOrigins(clientUrl));
        });
    }
}