using Asp.Versioning;
using DLS.API.Configurations;
using DLS.API.Middleware;
using DLS.API.OpenApi;
using DLS.Application;
using DLS.Infrastructure;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureSerilog();
builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

builder.Services.AddControllers()
        .AddJsonOptions(x =>
        {
            x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });


builder.Services.AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1);
        options.ReportApiVersions = true;
        options.ApiVersionReader = new UrlSegmentApiVersionReader();
    })
    .AddMvc()
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'V";
        options.SubstituteApiVersionInUrl = true;
    });



builder.Services.AddApplicationStrapping();
builder.Services.AddInfrastructureStrapping(builder.Configuration);
builder.Services.AddAppServicesDIConfig();

builder.Services.AddHttpContextAccessor();

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandlerMiddleware>();

builder.Services.AddSwaggerGen(c =>
{
    c.UseOneOfForPolymorphism();
    c.DescribeAllParametersInCamelCase();
});

builder.Services.AddHealthChecks();

builder.Services.AddCorsConfig(builder.Configuration);

var app = builder.Build();

app.UseCors("CorsPolicy");

app.UseSwagger();

app.UseSwaggerUI();

app.UseRequestLocalization();

app.UseHsts();

app.UseHttpsRedirection();

app.UseSerilogRequestLogging();

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseMiddleware<RequestContextLoggingMiddleware>();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();