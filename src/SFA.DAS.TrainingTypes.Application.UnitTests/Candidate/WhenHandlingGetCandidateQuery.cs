using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.CandidateAccount.Data.Candidate;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.TrainingTypes.Application.Candidate.Queries.GetCandidate;
using SFA.DAS.TrainingTypes.Domain.Candidate;

namespace SFA.DAS.TrainingTypes.Application.UnitTests.Candidate;

public class WhenHandlingGetCandidateQuery
{
    [Test, RecursiveMoqAutoData]
    public async Task Then_The_Candidate_Is_Found_By_Id_And_Returned_If_Guid(
        Guid id,
        GetCandidateQuery query,
        CandidateEntity entity,
        [Frozen] Mock<ICandidateRepository> repository,
        GetCandidateQueryHandler handler)
    {
        query.Id = id.ToString();
        repository.Setup(x => x.GetByGovIdentifier(It.IsAny<string>())).ReturnsAsync((CandidateEntity)null!);
        repository.Setup(x => x.GetById(id)).ReturnsAsync(entity);

        var actual = await handler.Handle(query, CancellationToken.None);

        actual.Candidate.Should().BeEquivalentTo((Domain.Candidate.Candidate)entity);
    }

    [Test, RecursiveMoqAutoData]
    public async Task Then_The_Candidate_Is_Found_By_GovIdentifier_And_Returned_If_NotGuid(
        GetCandidateQuery query,
        CandidateEntity entity,
        [Frozen] Mock<ICandidateRepository> repository,
        GetCandidateQueryHandler handler)
    {
        repository.Setup(x => x.GetByGovIdentifier(query.Id)).ReturnsAsync(entity);

        var actual = await handler.Handle(query, CancellationToken.None);

        actual.Candidate.Should().BeEquivalentTo((Domain.Candidate.Candidate)entity);
    }

    [Test, RecursiveMoqAutoData]
    public async Task Then_If_The_Candidate_Does_Not_Exist_Then_Null_Returned(
        GetCandidateQuery query,
        [Frozen] Mock<ICandidateRepository> repository,
        GetCandidateQueryHandler handler)
    {
        repository.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync((CandidateEntity)null!);
        repository.Setup(x => x.GetByGovIdentifier(It.IsAny<string>())).ReturnsAsync((CandidateEntity)null!);

        var actual = await handler.Handle(query, CancellationToken.None);

        actual.Candidate.Should().BeNull();
    }
}