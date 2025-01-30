using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.CandidateAccount.Data.Application;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.TrainingTypes.Application.Candidate.Queries.GetCandidatesByApplicationVacancy;
using SFA.DAS.TrainingTypes.Domain.Application;

namespace SFA.DAS.TrainingTypes.Application.UnitTests.Candidate;

public class WhenHandlingGetCandidatesByApplicationVacancy
{
    [Test, RecursiveMoqAutoData]
    public async Task Then_The_Query_Is_Handled_And_Applications_Returned(
        GetCandidatesByApplicationVacancyQuery query,
        List<ApplicationEntity> applicationEntities,
        [Frozen] Mock<IApplicationRepository> repository,
        GetCandidatesByApplicationVacancyQueryHandler handler)
    {
        repository.Setup(x =>
            x.GetApplicationsByVacancyReference(query.VacancyReference, query.StatusId, query.PreferenceId,
                query.CanEmailOnly)).ReturnsAsync(applicationEntities);

        var actual = await handler.Handle(query, CancellationToken.None);

        actual.Candidates.Should().BeEquivalentTo(applicationEntities);
    }

    [Test, RecursiveMoqAutoData]
    public async Task Then_If_No_Applications_Returned_Returns_Empty_List(
        GetCandidatesByApplicationVacancyQuery query,
        List<ApplicationEntity> applicationEntities,
        [Frozen] Mock<IApplicationRepository> repository,
        GetCandidatesByApplicationVacancyQueryHandler handler)
    {
        repository.Setup(x =>
            x.GetApplicationsByVacancyReference(query.VacancyReference, query.StatusId, query.PreferenceId,
                query.CanEmailOnly)).ReturnsAsync(new List<ApplicationEntity>());

        var actual = await handler.Handle(query, CancellationToken.None);

        actual.Candidates.Should().BeEmpty();
    }
}