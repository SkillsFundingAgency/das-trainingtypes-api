using SFA.DAS.Configuration.AzureTableStorage;

namespace SFA.DAS.CourseTypes.Api.AppStart;

public static class ConfigurationExtensions
{
    public static IConfigurationRoot LoadConfiguration(this IConfiguration config)
    {
        var configBuilder = new ConfigurationBuilder()
            .AddConfiguration(config)
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddEnvironmentVariables();


        if (!config["EnvironmentName"].Equals("DEV", StringComparison.CurrentCultureIgnoreCase))
        {

            configBuilder
                .AddJsonFile("appsettings.json", true)
                .AddJsonFile("appsettings.Development.json", true);

            configBuilder.AddAzureTableStorage(options =>
                {
                    options.ConfigurationKeys = config["ConfigNames"].Split(",");
                    options.StorageConnectionString = config["ConfigurationStorageConnectionString"];
                    options.EnvironmentName = config["EnvironmentName"];
                    options.PreFixConfigurationKeys = false;
                }
            );
        }

        return configBuilder.Build();
    }
}