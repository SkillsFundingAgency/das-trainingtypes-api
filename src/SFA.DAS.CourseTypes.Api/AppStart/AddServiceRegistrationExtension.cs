using SFA.DAS.CourseTypes.Application.Application.Queries.GetLearnerAge;
using SFA.DAS.CourseTypes.Domain.Factories;

namespace SFA.DAS.CourseTypes.Api.AppStart;

public static class AddServiceRegistrationExtension
{
    public static void AddServiceRegistration(this IServiceCollection services)
    {
        services.AddTransient<ICourseTypeFactory, CourseTypeFactory>();
        services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(typeof(GetLearnerAgeQuery).Assembly));
    }
}