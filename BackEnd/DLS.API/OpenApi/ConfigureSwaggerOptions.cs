using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace DLS.API.OpenApi;


internal sealed class ConfigureSwaggerOptions : IConfigureNamedOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;

    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
    {
        _provider = provider;
    }

    public void Configure(SwaggerGenOptions options)
    {
        // Add JWT Authentication
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = @"JWT Authorization header using the Bearer scheme.
                        Enter 'Bearer' [space] and then your token in the text input below.
                        Example: 'Bearer 12345abcdef'",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });

        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    },
                    Scheme = "oauth2",
                    Name = "Bearer",
                    In = ParameterLocation.Header
                },
                Array.Empty<string>()
            }
        });

        // Add API version documentation
        foreach (ApiVersionDescription description in _provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, CreateVersionInfo(description));
        }
    }

    public void Configure(string? name, SwaggerGenOptions options)
    {
        Configure(options);
    }

    private static OpenApiInfo CreateVersionInfo(ApiVersionDescription apiVersionDescription)
    {
        var openApiInfo = new OpenApiInfo
        {
            Title = $"DLS.API v{apiVersionDescription.ApiVersion}",
            Version = apiVersionDescription.ApiVersion.ToString(),
            Description = "DLS REST API with JWT Authentication",
            Contact = new OpenApiContact
            {
                Name = "Mohamed Mohanad",
                Email = "mohamedmohanad0852@gmail.com",
                Url = new Uri("https://www.linkedin.com/in/mohamed-mohanad/")
            }
        };

        if (apiVersionDescription.IsDeprecated)
        {
            openApiInfo.Description += " This API version has been deprecated.";
        }

        return openApiInfo;
    }
}
