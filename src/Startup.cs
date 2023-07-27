using AzureFunctions.Extensions.Swashbuckle;
using MI.Platform.Api;
using MI.Platform.Api.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.IO;
using System.Linq;
using System.Reflection;

[assembly: FunctionsStartup(typeof(Startup))]

namespace MI.Platform.Api
{
    public class Startup : FunctionsStartup
    {
        public IConfiguration _configuration { get; private set; }

        public override void Configure(IFunctionsHostBuilder builder)
        {
            var serviceProvider = builder.Services.BuildServiceProvider();
            
            _configuration = serviceProvider.GetRequiredService<IConfiguration>();

            var connectionString = _configuration.GetValue<string>("SQLDbConnectionString");

            builder.Services.AddDbConnectionFactory<SqlConnection>(connectionString);

            builder.Services.Add(new ServiceDescriptor(typeof(ILogger<>), typeof(FunctionsLogger<>), ServiceLifetime.Transient));

            builder.AddSwashBuckle(Assembly.GetExecutingAssembly());

            builder.Services.AddSwaggerGen(c =>
            {
                c.UseInlineDefinitionsForEnums();
                c.SwaggerDoc("v1", new OpenApiInfo { Version = "v1", Title = "Mi.Platform", Description = "Mi.Platform - API" });
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                c.CustomSchemaIds(type => type.FullName);
            });

            builder.Services.AddHttpClient();

            builder.Services.AddLogging();

        }

        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            var context = builder.GetContext();

            builder.ConfigurationBuilder
                .AddJsonFile(Path.Combine(context.ApplicationRootPath, "settings.json"), 
                                          optional: true, 
                                          reloadOnChange: false)
                .AddEnvironmentVariables();
        }
    }
}