using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.CandidateAccount.Data.Application;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.TrainingTypes.Application.Application.Queries.GetApplications;
using SFA.DAS.TrainingTypes.Domain.Application;

namespace SFA.DAS.TrainingTypes.Application.UnitTests.Application;

public class WhenHandlingGetApplicationsQuery
{
    [Test, RecursiveMoqAutoData]
    public async Task Then_The_Applications_For_The_Candidate_Are_Returned(
        Guid candidateId,
        ApplicationStatus status,
        GetApplicationsQuery query,
        List<ApplicationEntity> entities,
        [Frozen] Mock<IApplicationRepository> repository,
        GetApplicationsQueryHandler handler)
    {
        query.CandidateId = candidateId;
        query.Status = status;
        entities.ForEach(x => x.CandidateId = candidateId);
        entities.ForEach(x => x.Status = (short)status);
        repository.Setup(x => x.GetByCandidateId(query.CandidateId, (short)status)).ReturnsAsync(entities);

        var actual = await handler.Handle(query, CancellationToken.None);

        actual.Applications.Should().BeEquivalentTo(entities.Select(x => (Domain.Application.Application)x));
    }
}