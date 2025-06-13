using Microsoft.Extensions.Options;
using SFA.DAS.CourseTypes.Domain.Configuration;

namespace SFA.DAS.CourseTypes.Api.AppStart;

public static class AddConfigurationOptionsExtension
{
    public static void AddConfigurationOptions(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<CourseTypeConfiguration>(configuration.GetSection(nameof(CourseTypeConfiguration)));
        services.AddSingleton(cfg => cfg.GetService<IOptions<CourseTypeConfiguration>>().Value);
    }
}