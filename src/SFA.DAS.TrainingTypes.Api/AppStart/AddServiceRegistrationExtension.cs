using SFA.DAS.TrainingTypes.Application.Application.Queries.GetLearnerAge;
using SFA.DAS.TrainingTypes.Domain.Factories;

namespace SFA.DAS.TrainingTypes.Api.AppStart;

public static class AddServiceRegistrationExtension
{
    public static void AddServiceRegistration(this IServiceCollection services)
    {
        services.AddTransient<ITrainingTypeFactory, TrainingTypeFactory>();
        services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(typeof(GetLearnerAgeQuery).Assembly));
    }
}