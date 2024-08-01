using System;
using System.Linq;
using System.Threading.Tasks;
using Asp.Versioning;
using BBT.MyProjectName.Data;
using BBT.MyProjectName.EntityFrameworkCore;
using BBT.MyProjectName.Extensions;
using BBT.MyProjectName.HealthChecks;
using BBT.MyProjectName.Swagger;
using BBT.Prism;
using BBT.Prism.AspNetCore;
using BBT.Prism.AspNetCore.Dapr.EventBus;
using BBT.Prism.AspNetCore.HealthChecks;
using BBT.Prism.AspNetCore.HealthChecks.Dapr;
using BBT.Prism.AspNetCore.Microsoft.AspNetCore.Cors;
using BBT.Prism.AspNetCore.Serilog;
using BBT.Prism.EventBus.Dapr;
using BBT.Prism.Modularity;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BBT.MyProjectName;

[Modules(
    typeof(MyProjectNameApplicationModule),
    typeof(MyProjectNameEntityFrameworkCoreModule),
    typeof(PrismAspNetCoreModule),
    typeof(PrismAspNetCoreSerilogModule),
    typeof(PrismAspNetCoreHealthChecksModule),
    typeof(PrismAspNetCoreHealthChecksDaprModule),
    typeof(PrismAspNetCoreDaprEventBusModule)
)]
public class MyProjectNameHttpApiHostModule : PrismModule
{
    public override void ConfigureServices(ModuleConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        // var hostingEnvironment = context.Services.GetHostingEnvironment();

        context.Services.AddControllers();
        ConfigureEventBus();
        ConfigureDbContext(context, configuration);
        ConfigureObservability(context, configuration);
        ConfigureSerilog();
        ConfigureHealthChecks(context);
        ConfigureCors(context, configuration);
        ConfigureSwaggerAndVersioning(context);
        ConfigureEndpoints(context);
    }

    private void ConfigureEventBus()
    {
        Configure<PrismDaprEventBusOptions>(options =>
        {
            options.PubSubName = "myprojectname-pubsub";
        });
    }

    private void ConfigureDbContext(ModuleConfigurationContext context, IConfiguration configuration)
    {
        context.Services.AddPrismDbContext<MyProjectNameDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("Default"), b =>
            {
                b.MigrationsHistoryTable("__MyProjectName_Migrations");
            });
        });
    }

    private void ConfigureObservability(ModuleConfigurationContext context, IConfiguration configuration)
    {
        context.Services.AddObservability(configuration);
    }

    private void ConfigureSerilog()
    {
        Configure<PrismAspNetCoreSerilogOptions>(options =>
        {
            options.ShouldBodyBeTracked = true;
            // options.AddHeader(new []{ "example" });
            // options.AddWildcard(new []{ "example" });
        });
    }

    private void ConfigureHealthChecks(ModuleConfigurationContext context)
    {
        context.Services.AddHostedService<StartupBackgroundService>();
        context.Services.AddAppHealthChecks();
    }

    private void ConfigureCors(ModuleConfigurationContext context, IConfiguration configuration)
    {
        context.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder
                    .WithOrigins(configuration["App:CorsOrigins"]?
                        .Split(",", StringSplitOptions.RemoveEmptyEntries)
                        .Select(o => o.RemovePostFix("/"))
                        .ToArray() ?? [])
                    .WithPrismExposedHeaders()
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });
    }

    private void ConfigureSwaggerAndVersioning(ModuleConfigurationContext context)
    {
        context.Services.AddEndpointsApiExplorer();
        context.Services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1);
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ApiVersionReader = ApiVersionReader.Combine(
                    new HeaderApiVersionReader("X-Api-Version"),
                    new QueryStringApiVersionReader("v"),
                    new UrlSegmentApiVersionReader());
            })
            .EnableApiVersionBinding()
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });
        context.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        context.Services.AddSwaggerGen(options =>
        {
            options.OperationFilter<SwaggerDefaultValues>();
            options.CustomSchemaIds(type => type.FullName);
        });
    }

    private void ConfigureEndpoints(ModuleConfigurationContext context)
    {
        context.Services.AddEndpoints<MyProjectNameHttpApiHostModule>();
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseHsts();
        }

        app.UseCloudEvents();
        app.UseHttpsRedirection();
        app.UseCorrelationId();
        app.UseSecurityHeaders();
        app.UseCurrentUser();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseCors();
        // app.UseHttpMetrics();
        app.UsePrismSerilogEnrichers();
        app.UseExceptionHandler();
        
        app.UseConfiguredEndpoints(endpoints =>
        {
        });
        app.UseAppSwagger();
    }

    public async override Task OnPostApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        if (!context.GetEnvironment().IsProduction())
        {
            using var scope = context.ServiceProvider.CreateScope();
            await scope.ServiceProvider
                .GetRequiredService<MyProjectNameDbMigrationService>()
                .MigrateAsync();
        }
    }
}