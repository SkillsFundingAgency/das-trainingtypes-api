using Microsoft.Extensions.Options;
using SFA.DAS.TrainingTypes.Domain.Configuration;

namespace SFA.DAS.TrainingTypes.Api.AppStart;

public static class AddConfigurationOptionsExtension
{
    public static void AddConfigurationOptions(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<TrainingTypeConfiguration>(configuration.GetSection(nameof(TrainingTypeConfiguration)));
        services.AddSingleton(cfg => cfg.GetService<IOptions<TrainingTypeConfiguration>>().Value);
    }
}