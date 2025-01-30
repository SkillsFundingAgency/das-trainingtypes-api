using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using SFA.DAS.CandidateAccount.Application.ReferenceData.Queries;
using SFA.DAS.CandidateAccount.Data.AdditionalQuestion;
using SFA.DAS.CandidateAccount.Data.Address;
using SFA.DAS.CandidateAccount.Data.Application;
using SFA.DAS.CandidateAccount.Data.Candidate;
using SFA.DAS.CandidateAccount.Data.Qualification;
using SFA.DAS.CandidateAccount.Data.ReferenceData;
using SFA.DAS.CandidateAccount.Data.TrainingCourse;
using SFA.DAS.CandidateAccount.Data.WorkExperience;
using SFA.DAS.TrainingTypes.Api.AppStart;
using SFA.DAS.TrainingTypes.Application.Application.Commands.DeleteQualification;
using SFA.DAS.TrainingTypes.Application.Application.Commands.UpsertApplication;
using SFA.DAS.TrainingTypes.Application.Application.Queries.GetApplicationQualificationsByType;
using SFA.DAS.TrainingTypes.Application.Application.Queries.GetQualification;
using SFA.DAS.TrainingTypes.Application.Application.Queries.GetQualifications;
using SFA.DAS.TrainingTypes.Application.ReferenceData.Queries.GetAvailableQualifications;
using SFA.DAS.TrainingTypes.Domain.Configuration;

namespace SFA.DAS.TrainingTypes.Api.UnitTests.AppStart;

public class WhenAddingServicesToTheContainer
{
    [TestCase(typeof(ICandidateRepository))]
    [TestCase(typeof(IApplicationRepository))]
    [TestCase(typeof(IAdditionalQuestionRepository))]
    [TestCase(typeof(IWorkHistoryRepository))]
    [TestCase(typeof(ITrainingCourseRepository))]
    [TestCase(typeof(IQualificationReferenceRepository))]
    [TestCase(typeof(IQualificationRepository))]
    [TestCase(typeof(IAddressRepository))]
    [TestCase(typeof(IRequestHandler<UpsertApplicationCommand, UpsertApplicationCommandResponse>))]
    [TestCase(typeof(IRequestHandler<GetAvailableQualificationsQuery, GetAvailableQualificationsQueryResult>))]
    [TestCase(typeof(IRequestHandler<GetQualificationQuery, GetQualificationQueryResult>))]
    [TestCase(typeof(IRequestHandler<GetApplicationQualificationsQuery, GetApplicationQualificationsQueryResult>))]
    [TestCase(typeof(IRequestHandler<GetApplicationQualificationsByTypeQuery, GetApplicationQualificationsByTypeQueryResult>))]
    [TestCase(typeof(IRequestHandler<DeleteQualificationCommand, Unit>))]
    public void Then_The_Dependencies_Are_Correctly_Resolved(Type toResolve)
    {
        var serviceCollection = new ServiceCollection();
        SetupServiceCollection(serviceCollection);
        var provider = serviceCollection.BuildServiceProvider();

        var type = provider.GetService(toResolve);
        Assert.That(type, Is.Not.Null);
    }

    private static void SetupServiceCollection(ServiceCollection serviceCollection)
    {
        var configuration = GenerateConfiguration();
        var candidateAccountConfiguration = configuration
            .GetSection("TrainingTypeConfiguration")
            .Get<TrainingTypeConfiguration>();

        serviceCollection.AddSingleton(Mock.Of<IWebHostEnvironment>());
        serviceCollection.AddSingleton(Mock.Of<IConfiguration>());
        serviceCollection.AddConfigurationOptions(configuration);
        serviceCollection.AddDistributedMemoryCache();
        serviceCollection.AddServiceRegistration();
        serviceCollection.AddDatabaseRegistration(candidateAccountConfiguration!, "DEV");

    }

    private static IConfigurationRoot GenerateConfiguration()
    {
        var configSource = new MemoryConfigurationSource
        {
            InitialData = new List<KeyValuePair<string, string>>
            {
                new("EnvironmentName", "test"),
                new("ConnectionString", "test"),
            }!
        };

        var provider = new MemoryConfigurationProvider(configSource);

        return new ConfigurationRoot(new List<IConfigurationProvider> { provider });
    }
}